﻿using System.Diagnostics;
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

            builder.Services.AddDbContext<DataDbContext>(options =>
                options.UseSqlite("Data Source=project.db"));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();

            builder.Services.AddSingleton<ProjectsViewModel>();
            builder.Services.AddTransient<ProjectViewModel>();

            builder.Services.AddSingleton<ProjectsPage>();
            builder.Services.AddTransient<ProjectPage>();


            //samtliga serviecs copy-pastade från chatGPT, ja, jag är fortfarande lat.


#if DEBUG
            builder.Logging.AddDebug();
#endif

            try
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
