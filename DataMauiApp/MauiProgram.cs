using System.Diagnostics;
using DataMauiApp.ViewModels;
using DataMauiApp.Views;
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
            builder.Services.AddSingleton<ProjectsViewModel>();
            Debug.WriteLine("ProjectssssViewmodel Funkar.");

            builder.Services.AddTransient<ProjectViewModel>();
            Debug.WriteLine("ProjectViewmode Funkar.");

            builder.Services.AddSingleton<ProjectsPage>();
            Debug.WriteLine("ProjectssssPage Funkar.");

            builder.Services.AddTransient<ProjectPage>();
            Debug.WriteLine("ProjectPage Funkar.");


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
