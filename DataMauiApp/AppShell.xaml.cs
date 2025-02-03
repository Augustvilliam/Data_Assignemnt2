using DataMauiApp.Views;

namespace DataMauiApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
        }
    }
}
