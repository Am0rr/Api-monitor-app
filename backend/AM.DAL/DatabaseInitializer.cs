using System.Reflection;
using DbUp;

namespace AM.DAL;

public static class DatabaseInitializer
{
    public static void EnsureDatabaseSetup(string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
            throw new Exception("Database DbUP migration failed.", result.Error);
    }
}	
