using ADOPSE.Data;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);




// string connectionString;

string connectionString = builder.Configuration.GetConnectionString(
                        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_NAME")) ? "TestConnection" : "DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string not found.");

connectionString = connectionString.Replace("${DB_HOST}", Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1")
                                     .Replace("${DB_PORT}", Environment.GetEnvironmentVariable("DB_PORT") ?? "3306")
                                     .Replace("${DB_NAME}", Environment.GetEnvironmentVariable("DB_NAME") ?? "MyDB")
                                     .Replace("${DB_USER}", Environment.GetEnvironmentVariable("DB_USER") ?? "root")
                                     .Replace("${DB_PASSWORD}", Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "root");

// Add services to the container.

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString,Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.35-mysql")));

Console.Write(connectionString + " Connection String \n"); 

// var options = new DbContextOptionsBuilder<MyDbContext>()
//     .UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.35-mysql"))
//     .Options;




builder.Services.AddControllersWithViews();

var app = builder.Build();

//test DB connection

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    Console.WriteLine(dbContext.Database.CanConnect() ? "Database connected successfully" : "Failed to connect to database");
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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();