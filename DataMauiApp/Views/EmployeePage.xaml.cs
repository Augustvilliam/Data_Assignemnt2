using System.Diagnostics;
using Data.Entities;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class EmployeePage : ContentPage
{
	public EmployeePage(EmployeeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        Debug.WriteLine("EmployeePage Bunden till ViewModel!");
    }
    private void OnServiceCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is ServiceEntity service)
        {
            if (BindingContext is EmployeeViewModel viewModel)
            {
                viewModel.ToggleServiceSelection(service, e.Value);
            }
        }
    }

}