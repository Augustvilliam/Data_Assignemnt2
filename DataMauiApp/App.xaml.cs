using System.Diagnostics;

namespace DataMauiApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Debug.WriteLine("App Funkar.");

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());

        }
    }
}