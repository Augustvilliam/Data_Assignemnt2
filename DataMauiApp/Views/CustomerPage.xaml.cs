using System.Diagnostics;
using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class CustomerPage : ContentPage
{
	public CustomerPage(CustomerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}