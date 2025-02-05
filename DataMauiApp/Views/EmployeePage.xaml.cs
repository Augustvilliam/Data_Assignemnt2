using System.Diagnostics;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class EmployeePage : ContentPage
{
	public EmployeePage(EmployeeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Debug.WriteLine("MainMenuPage Bunden till ViewModel!");
    }
}