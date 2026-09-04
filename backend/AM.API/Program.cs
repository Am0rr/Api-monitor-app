using AM.API.Middleware;
using DotNetEnv;
using AM.DAL;
using AM.BLL;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = $"Server={Env.GetString("DB_HOST", "localhost")},{Env.GetString("DB_PORT", "1433")};" +
                       $"Database={Env.GetString("DB_NAME", "ApiMonitorDb")};" +
                       $"User Id=sa;" +
                       $"Password={Env.GetString("MSSQL_SA_PASSWORD")};" +
                       $"Encrypt=False;" +
                       $"TrustServerCertificate=True;";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDataAccessLayer();
builder.Services.AddBusinessLogicLayer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

if (!string.IsNullOrEmpty(connectionString))
{
    DatabaseInitializer.EnsureDatabaseSetup(connectionString);
}
    
app.MapControllers();

app.Run();

