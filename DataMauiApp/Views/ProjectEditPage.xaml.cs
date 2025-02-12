using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class ProjectEditPage : ContentPage
{
	public ProjectEditPage(ProjectEditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }
}