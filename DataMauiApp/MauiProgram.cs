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
        Debug.WriteLine($"Databasen finns här: {desktopPath}"); //Eftesom daabasen la sig huller om buller innan fick den så snällt hamna på skrivbordet istället. och med en debug bara för att se att den faktiskt hamnar där.

        builder.Services.AddDbContext<DataDbContext>(options =>
            options.UseSqlite($"Data Source={desktopPath}"));

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<IServiceService, ServiceService>();
        builder.Services.AddScoped<IRoleService, RoleService>();

        builder.Services.AddTransient<ProjectViewModel>();
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddTransient<CustomerViewModel>();
        builder.Services.AddTransient<EmployeeViewModel>();

        builder.Services.AddTransient<ProjectPage>();
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<CustomerPage>();
        builder.Services.AddTransient<EmployeePage>();

        builder.Services.AddTransient<EmployeeEditViewModel>();
        builder.Services.AddTransient<EmployeeEditPage>();

        builder.Services.AddTransient<CustomerEditViewModel>();
        builder.Services.AddTransient<CustomerEditPage>();

        builder.Services.AddTransient<ProjectEditPage>();
        builder.Services.AddTransient<ProjectEditViewModel>();



        //majoriteten av denna delen generead med chatGPT
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
            var dbInitializer = new DbInitializer($"Data Source={desktopPath}");

            // context.Database.EnsureDeleted(); // Nukar databasen vid uppstart för att spara tid under utvecklingen. 
            dbInitializer.InitializeDatabase();  //  Skapar tabeller enligt schema
            dbInitializer.SeedData();           // Lägger in roller & tjänster
            //dbInitializer.TestData();           // Lägger in testdata
        }

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
