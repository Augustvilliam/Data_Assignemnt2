using System.Diagnostics;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class ProjectsPage : ContentPage
{
    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Debug.WriteLine("ProjectsPage ViewModel Bunden!");

    }
}
