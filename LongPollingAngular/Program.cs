using LongPollingAngular;
using Microsoft.Extensions.Configuration;
using SharedLibraries;

IConfiguration configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables()
.AddCommandLine(args)
.Build();

#region PostgreSQL

var sqlHostName = configuration.GetSection("AppSettings")["sqlhost"];
var sqlUserName = configuration.GetSection("AppSettings")["sqluser"];
var sqlPassName = configuration.GetSection("AppSettings")["sqlpass"];
var sqlDbName = configuration.GetSection("AppSettings")["sqldb"];

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IPostgreSQLService, PostgreSQLService>(services => new PostgreSQLService(sqlHostName, sqlUserName, sqlPassName, sqlDbName)); ;

var app = builder.Build();

//var hostBuilder = Host.CreateDefaultBuilder(args);

//var host = hostBuilder.ConfigureServices(services =>
//{
//    services.AddSingleton<IPostgreSQLService, PostgreSQLService>(services => new PostgreSQLService(sqlHostName, sqlUserName, sqlPassName, sqlDbName));
//}).Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<MessageHub>("/messageHub");


app.Run();
//host.Run();
