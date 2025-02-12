
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class EmployeeEditPage : ContentPage
{
	public EmployeeEditPage(EmployeeEditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}