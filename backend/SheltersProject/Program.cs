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
    .GetChatClient("gemini-3.8-flash")
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
// ייבוא ראשוני של מקלטים ציבוריים מירושלים
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<DB_Shelters>();

    // 1. מציאת משתמש המנהל
    string adminEmail = "estherfaig865@gmail.com";

    var admin = await context.User
        .FirstOrDefaultAsync(u =>
            u.Email == adminEmail &&
            u.Role == UserRole.Admin);

    if (admin == null)
    {
        throw new Exception(
            "מנהל המערכת לא נמצא במסד הנתונים."
        );
    }

    // 2. איתור קובץ הנתונים
    string filePath = Path.Combine(
        app.Environment.ContentRootPath,
        "Data",
        "JerusalemShelters_ImportReady.geojson"
    );

    if (!File.Exists(filePath))
    {
        throw new FileNotFoundException(
            "קובץ נתוני המקלטים לא נמצא.",
            filePath
        );
    }

    // 3. קריאת קובץ ה-JSON
    string json = await File.ReadAllTextAsync(filePath);

    using JsonDocument document = JsonDocument.Parse(json);

    JsonElement features =
        document.RootElement.GetProperty("features");

    // 4. שליפת המקלטים שכבר יובאו
    var existingShelters = await context.Shelter
        .Where(s =>
            s.IsImported &&
            s.City == "ירושלים" &&
            s.ShelterType == "PublicShelter")
        .Select(s => new
        {
            s.Latitude,
            s.Longitude
        })
        .ToListAsync();

    // 5. מניעת ייבוא כפול לפי מיקום
    var existingLocations =
        new HashSet<(double Latitude, double Longitude)>(
            existingShelters.Select(s =>
                (
                    Math.Round(s.Latitude, 7),
                    Math.Round(s.Longitude, 7)
                )
            )
        );

    var newShelters = new List<Shelter>();

    // 6. מעבר על כל המקלטים בקובץ
    foreach (JsonElement feature in features.EnumerateArray())
    {
        JsonElement properties =
            feature.GetProperty("properties");

        JsonElement coordinates =
            feature.GetProperty("geometry")
                   .GetProperty("coordinates");

        string name =
      "מקלט ציבורי ירושלים " +
      properties.GetProperty("SourceShelterNumber").ToString();

        double longitude = coordinates[0].GetDouble();
        double latitude = coordinates[1].GetDouble();

        var locationKey = (
            Math.Round(latitude, 7),
            Math.Round(longitude, 7)
        );

        // אם המקלט כבר קיים, מדלגים עליו
        if (!existingLocations.Add(locationKey))
        {
            continue;
        }

        // 7. יצירת אובייקט Shelter
        var shelter = new Shelter
        {
            Name = name,

            ShelterType = "מקלט ציבורי",

            Address = null,

            City = "ירושלים",

            Latitude = latitude,

            Longitude = longitude,

            MaxCapacity = null,

            Status = ShelterStatus.Approved,

            IsActive = true,

            IsImported = true,

            AlertZoneId = null,

            CreatedByUserId = admin.UserId
        };

        newShelters.Add(shelter);
    }

    // 8. שמירת המקלטים החדשים במסד הנתונים
    if (newShelters.Count > 0)
    {
        context.Shelter.AddRange(newShelters);

        await context.SaveChangesAsync();
    }

    Console.WriteLine(
        $"נוספו {newShelters.Count} מקלטים חדשים לירושלים."
    );
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