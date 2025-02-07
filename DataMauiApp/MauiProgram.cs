using System.Diagnostics;
using Busniess.Interface;
using Busniess.Services;
using Data.Context;
using Data.Interface;
using Data.Repositories;
using DataMauiApp.ViewModels;
using DataMauiApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataMauiApp
{
    public static class MauiProgram
    {
        //samtliga serviecs copy-pastade från chatGPT, ja, jag är fortfarande lat.
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



            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataDbContext>();
                Debug.WriteLine("🔄 Återställer databasen en gång vid uppstart...");
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.SeedRoles();
            }

            //Här slutar copypast. 
#if DEBUG
            builder.Logging.AddDebug();
#endif

            try //Hade en jävla massa problem med att få appen att starta, därav debug. Visade sig att jag glömde en service...
            {
                var app = builder.Build();
                Debug.WriteLine("Nu borde appen starta,.l");

                return app;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"🔥 ERROR: {ex.Message}");
                throw;
            }

        }
    }
}
