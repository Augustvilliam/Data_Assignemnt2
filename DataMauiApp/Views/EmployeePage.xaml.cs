using System.Diagnostics;
using Busniess.Dtos;
using Data.Entities;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class EmployeePage : ContentPage
{
	public EmployeePage(EmployeeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    private void OnServiceCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is ServiceDto service)
        {
            if (BindingContext is EmployeeViewModel viewModel)
            {
                viewModel.ToggleServiceSelection(service, e.Value);
            }
        }
    }

}