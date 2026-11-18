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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using OpenAI;
using SheltersProject.Filters;
using System.ClientModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. שליפה ובדיקה של מחרוזת החיבור למסד הנתונים
var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is missing. " +
        "Set it with User Secrets or an environment variable.");

// בלי מפתח הצ'אט לא יעבוד, אבל שאר האתר ימשיך לעלות
var geminiKey = builder.Configuration["Gemini:ApiKey"];
if (string.IsNullOrWhiteSpace(geminiKey)) geminiKey = "missing-key";


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

    // הודעות ה-Binding של המסגרת, בעברית
    var m = options.ModelBindingMessageProvider;
    m.SetValueMustBeANumberAccessor(field => $"השדה '{field}' חייב להיות מספר");
    m.SetAttemptedValueIsInvalidAccessor((value, field) => $"הערך '{value}' אינו תקין בשדה '{field}'");
    m.SetValueIsInvalidAccessor(value => $"הערך '{value}' אינו תקין");
    m.SetUnknownValueIsInvalidAccessor(field => $"הערך בשדה '{field}' אינו תקין");
    m.SetMissingBindRequiredValueAccessor(field => $"חסר ערך בשדה '{field}'");
    m.SetMissingRequestBodyRequiredValueAccessor(() => "גוף הבקשה חסר");
})
.ConfigureApiBehaviorOptions(options =>
{
    // מחזיר את הודעת הוולידציה הראשונה בעברית ב-title, במבנה אחיד עם שאר השגיאות
    options.InvalidModelStateResponseFactory = context =>
    {
        var first = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault(msg => System.Text.RegularExpressions.Regex.IsMatch(msg, "[\u0590-\u05FF]"))
            ?? "הנתונים שנשלחו אינם תקינים";

        return new ObjectResult(new ProblemDetails { Status = 400, Title = first })
        {
            StatusCode = 400
        };
    };
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
builder.Services.AddScoped<IAddressCheckService, AddressCheckService>();

// Google Routes. Timeout קצר: בזמן אזעקה עדיף הערכה מהירה מהמתנה
builder.Services.AddHttpClient<IDistanceService, GoogleMapsDistanceService>(client =>
{
    client.BaseAddress = new Uri("https://routes.googleapis.com/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

// ───── התרעות: קריאה ברקע, מכמה שרתים (מדומה ואמיתי) ─────
builder.Services.Configure<AlertServerOptions>(
    builder.Configuration.GetSection(AlertServerOptions.SectionName));

builder.Services.AddHttpClient("alerts", client =>
{
    client.Timeout = TimeSpan.FromSeconds(
        builder.Configuration.GetValue<int?>("AlertServer:TimeoutSeconds") ?? 3);
    // השרת האמיתי דורש את הכותרות האלה. לשרת המדומה הן לא מפריעות
    client.DefaultRequestHeaders.Add("Referer", "https://www.oref.org.il/");
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0 Safari/537.36");
})
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),   // מרענן DNS, ומשאיר חיבור חם
    ConnectTimeout = TimeSpan.FromSeconds(2),
    AutomaticDecompression = System.Net.DecompressionMethods.All
});

builder.Services.AddSingleton<IActiveAlertSource>(sp =>
{
    var options = sp.GetRequiredService<IOptions<AlertServerOptions>>().Value;
    var factory = sp.GetRequiredService<IHttpClientFactory>();

    var sources = options.ResolveUrls().Select(url =>
    {
        var client = factory.CreateClient("alerts");
        client.BaseAddress = new Uri(url);
        return (IActiveAlertSource)new OrefActiveAlertSource(client);
    });
    return new CompositeActiveAlertSource(sources);
});

builder.Services.AddSingleton<ActiveAlertCache>();
builder.Services.AddSingleton<IActiveAlertCache>(sp => sp.GetRequiredService<ActiveAlertCache>());
builder.Services.AddHostedService<AlertPollingService>();

// גיאוקודינג דרך Nominatim (OpenStreetMap): בלי מפתח API
builder.Services.AddHttpClient<GeocodingTools>(client =>
{
    client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
    client.Timeout = TimeSpan.FromSeconds(8);
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