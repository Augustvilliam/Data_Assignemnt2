using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Busniess.Interface;
using Busniess.Services;
using System.Diagnostics;
using MobileApp.ViewModels;

namespace MobileApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Ange fullständig sökväg till databasen
        string dbPath = Path.Combine(AppContext.BaseDirectory, "project.db");
        string connectionString = $"Data Source={dbPath}";


        // Konfigurera SQLite och registrera DbContext
        builder.Services.AddDbContext<DataDbContext>(options =>
            options.UseSqlite(connectionString));

        // Registrera services
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IServiceService, ServiceService>();
        builder.Services.AddSingleton<ProjectViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Initialisera databasen och lägga till debug-loggar
        var dbInitializer = new DbInitializer(connectionString);
        Debug.WriteLine("Startar DbInitializer...");
        dbInitializer.InitializeDatabase();
        dbInitializer.TestData();
        dbInitializer.FetchTestData();
        Debug.WriteLine("DbInitializer färdig!");

        // Kontrollera om databasen skapades
        if (File.Exists(dbPath))
        {
            Debug.WriteLine("Databasen finns!");
        }
        else
        {
            Debug.WriteLine("Databasen saknas!");
        }

        return app;
    }
}

