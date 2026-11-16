using BLL;
using BLL.Agent;
using BLL.Mapper;
using BLL.Search;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using OpenAI;
using SheltersProject.Filters;
using System.ClientModel;
using System.Text;
using System.Threading.RateLimiting;

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

// הגבלת קצב לצ'אט: לפי כתובת IP
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("chat", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 15,                 // 15 הודעות
            Window = TimeSpan.FromMinutes(1), // לדקה
            QueueLimit = 0
        }));
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
builder.Services.AddScoped<IAddressCheckService, AddressCheckService>();

// === כלים של ה-Agent ===
builder.Services.AddScoped<ShelterTools>();

// === חיפוש מקלטים ===
builder.Services.Configure<ShelterSearchOptions>(
    builder.Configuration.GetSection(ShelterSearchOptions.SectionName));
builder.Services.AddScoped<IShelterEligibilityService, ShelterEligibilityService>();
builder.Services.AddScoped<IShelterRankingService, ShelterRankingService>();
builder.Services.AddScoped<IEmergencyGuidanceService, EmergencyGuidanceService>();
builder.Services.AddScoped<IShelterSearchService, ShelterSearchService>();

// Google Routes. Timeout קצר: בזמן אזעקה עדיף הערכה מהירה מהמתנה
builder.Services.AddHttpClient<IDistanceService, GoogleMapsDistanceService>(client =>
{
    client.BaseAddress = new Uri("https://routes.googleapis.com/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

// שרת ההתרעות (המדומה כרגע). בהמשך מחליפים את ה-BaseUrl בקונפיגורציה
builder.Services.AddHttpClient<IActiveAlertSource, OrefActiveAlertSource>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AlertServer:BaseUrl"] ?? "http://localhost:8000/");
    client.Timeout = TimeSpan.FromSeconds(2);   // בזמן אזעקה לא מחכים
    // השרת האמיתי דורש את הכותרות האלה. לשרת המדומה הן לא מפריעות
    client.DefaultRequestHeaders.Add("Referer", "https://www.oref.org.il/");
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
});

// גיאוקודינג דרך Nominatim (OpenStreetMap): בלי מפתח API
builder.Services.AddHttpClient<GeocodingTools>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    // חובה לפי כללי השימוש של Nominatim: שם מזהה לאפליקציה
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "SheltersProject-StudentApp/1.0 (estherfaig865@gmail.com)");
});

// === רישום ה-ChatClient ב-DI Container ===
builder.Services.AddChatClient(innerClient)
    .UseFunctionInvocation();

var app = builder.Build();

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
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();