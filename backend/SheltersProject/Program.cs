using BLL;
using BLL.Mapper;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//CORS- לאפשר את ריאקט
builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy => policy
        .WithOrigins("http://localhost:3000", "http://localhost:5173")   // הכתובת של שרת ה-React שלך
        .AllowAnyHeader()
        .AllowAnyMethod());
});


// ����� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

// ����� ������
builder.Services.AddDbContext<DB_Shelters>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.UseNetTopologySuite()));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.UseCors("React");

app.UseAuthorization();

app.MapControllers();

app.Run();