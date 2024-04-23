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
// using Swagger;
using ADOPSE.Configurations;
using HtmlAgilityPack;
using Google.Api;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Mvc.Filters;
using AngleSharp.Text;
using System.Reflection;
using ADOPSE.Extensions;



var builder = WebApplication.CreateBuilder(args);
ConfigureLogs();
builder.Host.UseSerilog();


builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IEnrolledRepository, EnrolledRepository>();
builder.Services.AddScoped<IEnrolledService, EnrolledService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILuceneRepository, LuceneRepository>();

builder.Services.AddScoped<IOpenAiService, OpenAiService>();
builder.Services.Configure<OpenAiConfig>(builder.Configuration.GetSection("OpenAI"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
 builder.Services.AddSwaggerGen();
 
 
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
    
    
 var web = new HtmlWeb();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found.");


// Add services to the container.


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString,ServerVersion.Parse("5.7.35-mysql")));

builder.Services.AddScoped<HttpClient>();
Console.Write(connectionString + " Connection String \n");

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

builder.Services.AddElasticSearch(builder.Configuration);

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

  //Configure the HTTP request pipeline.
   if (!app.Environment.IsDevelopment())
   {
       // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
       app.UseHsts();
    
   }
//     Configure the HTTP request pipeline.
//   if (!app.Environment.IsDevelopment())
//   {
//    
//      app.UseSwagger();
//       app.UseSwaggerUI();
//   }
  if (!app.Environment.IsDevelopment())
  {
      //apply migrations
  
      using var scope = app.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
      dbContext.Database.Migrate();
  }



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

 app.MapFallbackToFile("index.html");

app.Run();


#region helper
void ConfigureLogs()
{//get the environment
     var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//get the configuration
var configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false , reloadOnChange:true)
.Build();

//CREATE LOGGER
Log.Logger = new LoggerConfiguration()
.Enrich.FromLogContext()
//.Enrich.With<ExceptionContext>
.WriteTo.Debug()
.WriteTo.Console()
.WriteTo.Elasticsearch(ConfigureELS(configuration, env))
.CreateLogger();
}


ElasticsearchSinkOptions ConfigureELS(IConfiguration configuration, string env )
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"])) //apo appsettings.json
{

    AutoRegisterTemplate = true,
    IndexFormat=$"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{env.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}" //cspoj

} ;
}
#endregion