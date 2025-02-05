
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DataMauiApp.ViewModels;

public partial class CustomerViewModel : ObservableObject
{
    [RelayCommand]
    private async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("..");
    }
}
