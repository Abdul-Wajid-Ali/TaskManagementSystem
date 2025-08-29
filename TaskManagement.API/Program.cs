using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TaskManagement.Application.Config;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Mapping;
using TaskManagement.Application.Services;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Bind JwtSettings from appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Retrieve JwtSettings for later use
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

// ---------------------------------------------------
// 1. Add services to the DI container
// ---------------------------------------------------

// Add MVC Controllers support
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add Swagger/OpenAPI for API documentation & testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AutoMapper and load mapping profiles
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"], typeof(TaskProfile));

// Registering DbContext with SQL Server provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registering the TaskRepository for dependency injection
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Registering the TaskService for dependency injection
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();

// Registering the PasswordService for dependency injection
builder.Services.AddScoped<IPasswordService, PasswordService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

builder.Services.AddAuthorization();

// ---------------------------------------------------
// 2. Build the application
// ---------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------
// 3. Configure the HTTP request pipeline (middleware)
// ---------------------------------------------------

// Enable Swagger UI only in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS
app.UseHttpsRedirection();

// Enable Authorization middleware (Authentication can be added above if needed)
app.UseAuthorization();

// Map API endpoints (Controller routes)
app.MapControllers();

// Run the application
app.Run();