using ADOPSE.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString,Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.35-mysql")));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews();

var app = builder.Build();

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