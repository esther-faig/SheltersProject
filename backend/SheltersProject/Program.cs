using BLL;
using BLL.Mapper;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using Microsoft.Extensions.FileProviders;
using System.Text;
using SheltersProject.Filters;
using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;
using BLL.Agent;
using System.Text.Json;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// 1. שליפה ובדיקה של מחרוזת החיבור למסד הנתונים
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is missing. " +
        "Set it with User Secrets or an environment variable.");

var geminiKey = builder.Configuration["Gemini:ApiKey"]
    ?? throw new InvalidOperationException("Gemini:ApiKey is missing");

var openAiOptions = new OpenAIClientOptions
{
    Endpoint = new Uri(
        "https://generativelanguage.googleapis.com/v1beta/openai/"
    )
};

IChatClient innerClient =
    new OpenAIClient(
        new ApiKeyCredential(geminiKey),
        openAiOptions
    )
    .GetChatClient("gemini-2.5-flash")
    .AsIChatClient();

// הנתיב המלא לתיקיית התמונות, מתוך appsettings.json
var ImagesFolder = Path.GetFullPath(Path.Combine(
    builder.Environment.ContentRootPath,
    builder.Configuration["ImagesFolder"] ?? "../Images"));
Directory.CreateDirectory(ImagesFolder);   // יוצר את התיקייה אם אינה קיימת

var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("Jwt:Key is missing. Set the environment variable Jwt__Key.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            //אין דקות של סבלנות אחרי שהטוקן פג

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // ברירת מחדל: הכול נעול. פותחים במפורש עם [AllowAnonymous]
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();
});

// Filter מרכזי לתפיסת שגיאות: מחליף try/catch בכל Controller
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// CORS - לאפשר את ריאקט
builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy => policy
        .WithOrigins("http://localhost:3000", "http://localhost:5173")   // הכתובת של שרת ה-React שלך
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
// אפשר לבדוק טוקנים ב-Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "הדביקי את הטוקן מ-login (בלי המילה Bearer)"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference
            { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

// מסד נתונים
builder.Services.AddDbContext<DB_Shelters>(options =>
    options.UseSqlServer(connStr, x => x.UseNetTopologySuite()));

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Repositories (DAL)
builder.Services.AddScoped<IAlertZoneRepository, AlertZoneRepository>();
builder.Services.AddScoped<IPreferenceRepository, PreferenceRepository>();
builder.Services.AddScoped<IProtectionTimeRepository, ProtectionTimeRepository>();
builder.Services.AddScoped<IShelterFeatureRepository, ShelterFeatureRepository>();
builder.Services.AddScoped<IShelterRepository, ShelterRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services (BLL)
builder.Services.AddScoped<IAlertZoneService, AlertZoneService>();
builder.Services.AddScoped<IPreferenceService, PreferenceService>();
builder.Services.AddScoped<IProtectionTimeService, ProtectionTimeService>();
builder.Services.AddScoped<IShelterService, ShelterService>();
builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IImageService>(new ImageService(ImagesFolder));
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// === כלים של ה-Agent ===
builder.Services.AddScoped<ShelterTools>();

// גיאוקודינג דרך Nominatim (OpenStreetMap): בלי מפתח API
builder.Services.AddHttpClient<GeocodingTools>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    // חובה לפי כללי השימוש של Nominatim: שם מזהה לאפליקציה
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "SheltersProject-StudentApp/1.0 (your-email@example.com)");
});

// === רישום ה-ChatClient ב-DI Container ===
builder.Services.AddChatClient(innerClient)
    .UseFunctionInvocation();

var app = builder.Build();


//זמני

// ==========================================
// ייבוא חד-פעמי של מקלטים מקובץ shelters.tsv
// ==========================================

if (args.Contains("--import-shelters"))
{
    try
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<DB_Shelters>();

        // נתיב לקובץ הנתונים
        string filePath = Path.Combine(
            app.Environment.ContentRootPath,
            "Data",
            "shelters.tsv"
        );

        // בדיקה שקובץ הנתונים קיים
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"לא נמצא קובץ הייבוא: {filePath}"
            );
        }

        // בדיקה שמשתמש מספר 1 קיים
        var user = await db.User
            .FirstOrDefaultAsync(u => u.UserId == 1);

        if (user == null)
        {
            throw new Exception(
                "לא נמצא משתמש עם UserId = 1."
            );
        }

        // מניעת ייבוא כפול
        bool alreadyImported = await db.Shelter
            .AnyAsync(s => s.IsImported && s.City == "פתח תקווה");

        if (alreadyImported)
        {
            throw new Exception(
                "כבר קיימים מקלטים מיובאים בפתח תקווה. הייבוא נעצר."
            );
        }

        // קריאת כל השורות מהקובץ
        // קריאה זו תומכת גם בקובץ Unicode עם BOM
        string[] lines = await File.ReadAllLinesAsync(filePath);

        if (lines.Length <= 1)
        {
            throw new Exception(
                "קובץ הייבוא ריק או מכיל כותרת בלבד."
            );
        }

        List<Shelter> sheltersToImport = new();

        // מעבר על כל שורות הנתונים, החל משורה 2
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] columns = lines[i].Split('\t');

            if (columns.Length < 8)
            {
                throw new Exception(
                    $"שגיאה בשורה {i + 1}: חסרות עמודות בקובץ."
                );
            }

            // קריאת הנתונים מהעמודות הרלוונטיות
            string address = columns[0].Trim();
            string shelterType = columns[1].Trim();
            string openText = columns[3].Trim();

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new Exception(
                    $"שגיאה בשורה {i + 1}: חסרה כתובת."
                );
            }

            // המרת קווי הרוחב והאורך למספרים
            if (!double.TryParse(
                columns[6].Trim(),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double latitude))
            {
                throw new Exception(
                    $"שגיאה בשורה {i + 1}: ערך Latitude אינו תקין."
                );
            }

            if (!double.TryParse(
                columns[7].Trim(),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out double longitude))
            {
                throw new Exception(
                    $"שגיאה בשורה {i + 1}: ערך Longitude אינו תקין."
                );
            }

            // המרת מצב המקלט לערך IsActive
            bool isActive;

            if (openText == "פתוח")
            {
                isActive = true;
            }
            else if (openText == "סגור")
            {
                isActive = false;
            }
            else
            {
                throw new Exception(
                    $"שגיאה בשורה {i + 1}: ערך פתיחה לא מוכר: {openText}"
                );
            }

            // יצירת אובייקט מקלט לפי הישות הקיימת
            Shelter shelter = new Shelter
            {
                Name = address,
                Address = address,
                ShelterType = shelterType,
                City = "פתח תקווה",

                Latitude = latitude,
                Longitude = longitude,

                MaxCapacity = null,

                Status = ShelterStatus.Approved,

                IsActive = isActive,

                IsImported = true,

                AlertZoneId = null,

                CreatedByUserId = 1,
                User = user,

                Files = new List<string>(),
                ShelterFeatures = new List<ShelterFeature>()
            };

            sheltersToImport.Add(shelter);
        }

        if (sheltersToImport.Count == 0)
        {
            throw new Exception(
                "לא נמצאו מקלטים תקינים לייבוא."
            );
        }

        // שמירת הנתונים במסד הנתונים בתוך טרנזקציה
        await using var transaction =
            await db.Database.BeginTransactionAsync();

        await db.Shelter.AddRangeAsync(sheltersToImport);

        await db.SaveChangesAsync();

        await transaction.CommitAsync();

        Console.WriteLine("------------------------------------");
        Console.WriteLine("הייבוא הסתיים בהצלחה!");
        Console.WriteLine($"מספר המקלטים שיובאו: {sheltersToImport.Count}");
        Console.WriteLine("------------------------------------");
    }

    catch (Exception ex)
    {
        Console.WriteLine("אירעה שגיאה במהלך ייבוא המקלטים:");

        Exception? currentException = ex;

        while (currentException != null)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("סוג השגיאה: " + currentException.GetType().Name);
            Console.WriteLine("הודעת השגיאה: " + currentException.Message);

            currentException = currentException.InnerException;
        }

        Environment.ExitCode = 1;
    }


    // עצירת התוכנית לאחר פעולת הייבוא
    return;
}





app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(ImagesFolder),
    RequestPath = "/Images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // הפניה ל-HTTPS רק בפרודקשן. בפיתוח היא שוברת את הקריאות של React
    app.UseHttpsRedirection();
}
app.UseCors("React");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();