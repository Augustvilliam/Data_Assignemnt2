using System.Diagnostics;
using DataMauiApp.Views;

namespace DataMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Debug.WriteLine("Appshell Initialize Funkar.");

            Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
            Debug.WriteLine("AppShell routring funkar Funkar.");

        }
    }
}
