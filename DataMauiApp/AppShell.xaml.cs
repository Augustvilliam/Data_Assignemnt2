using DataMauiApp.Views;

namespace DataMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainMenuPage), typeof(MainMenuPage));
            Routing.RegisterRoute(nameof(EmployeePage), typeof(EmployeePage));
            Routing.RegisterRoute(nameof(CustomerPage), typeof(CustomerPage));
            Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));

        }
    }
}
