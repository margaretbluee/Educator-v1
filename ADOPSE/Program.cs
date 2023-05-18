using System.Data.Common;
using System.Text;
using ADOPSE.Data;
using ADOPSE.Repositories;
using ADOPSE.Repositories.IRepositories;
using ADOPSE.Services;
using ADOPSE.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IEnrolledRepository, EnrolledRepository>();
builder.Services.AddScoped<IEnrolledService, EnrolledService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// string connectionString;

bool hasDbEnvironment = Environment.GetEnvironmentVariables().Keys.Cast<string>().Any(key => key.StartsWith("DB"));

string connectionString = builder.Configuration.GetConnectionString(
                        hasDbEnvironment ? "TestConnection" : "DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string not found.");

connectionString = connectionString.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1")
                                     .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "3306")
                                     .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "mysql")
                                     .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "root")
                                     .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root");

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.35-mysql")));

Console.Write(connectionString + " Connection String \n");


builder.Services.AddControllersWithViews();

var app = builder.Build();

//test DB connection

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    try
    {
        dbContext.Database.CanConnect();
        Console.WriteLine("Database connected successfully");
    }
    catch (DbException ex)
    {
        Console.WriteLine("Failed to connect to database");
        Console.WriteLine(ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// string allowedOrigin = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN") ?? "https://localhost:44442";
// Console.Write(allowedOrigin + " Allowed Origin \n");

// app.UseCors(builder =>
//     builder.WithOrigins(allowedOrigin)
//         .AllowAnyHeader()
//         .AllowAnyMethod()
//         .AllowCredentials()
// );

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();