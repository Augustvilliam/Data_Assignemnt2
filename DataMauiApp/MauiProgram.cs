using System.Diagnostics;
using Busniess.Interface;
using Busniess.Services;
using Data.Context;
using Data.Interface;
using Data.Repositories;
using DataMauiApp;
using DataMauiApp.ViewModels;
using DataMauiApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "project.db");
        Debug.WriteLine($"📂 Databasen finns här: {desktopPath}");

        builder.Services.AddDbContext<DataDbContext>(options =>
            options.UseSqlite($"Data Source={desktopPath}"));

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IServiceService, ServiceService>();
        builder.Services.AddScoped<IRoleService, RoleService>();

        builder.Services.AddSingleton<ProjectsViewModel>();
        builder.Services.AddTransient<ProjectViewModel>();
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddSingleton<CustomerViewModel>();
        builder.Services.AddSingleton<EmployeeViewModel>();

        builder.Services.AddSingleton<ProjectsPage>();
        builder.Services.AddTransient<ProjectPage>();
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddSingleton<CustomerPage>();
        builder.Services.AddSingleton<EmployeePage>();

        // 🔄 Initialisera databasen
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
            var dbInitializer = new DbInitializer($"Data Source={desktopPath}");

            Debug.WriteLine("🔄 Återställer och initialiserar databasen...");
            context.Database.EnsureDeleted();
            dbInitializer.InitializeDatabase();  // ✅ Skapar tabeller enligt schema
            dbInitializer.TestData();           // ✅ Lägger in testdata
        }

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
