using DataMauiApp.ViewModels;

namespace DataMauiApp.Views;

public partial class MainMenuPage : ContentPage
{
	public MainMenuPage(MainMenuViewModel viewModle)
	{
		InitializeComponent();
		BindingContext = viewModle;
	}
}