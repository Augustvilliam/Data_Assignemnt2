using Data.Entities;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class CustomerEditPage : ContentPage
{
	public CustomerEditPage(CustomerEditViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

    }

}