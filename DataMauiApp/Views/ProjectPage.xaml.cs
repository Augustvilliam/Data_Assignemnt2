using System.Diagnostics;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class ProjectPage : ContentPage
{
    public ProjectPage(ProjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Debug.WriteLine("ProjectPage Funkar.");

    }
}
