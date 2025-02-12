
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataMauiApp.Views;

namespace DataMauiApp.ViewModels
{
    public partial class MainMenuViewModel : ObservableObject

    {
        [RelayCommand]
        private async Task NavigateToCustomerMenu()
        {
            Debug.WriteLine("Navigerar Till Customers...");
            await Shell.Current.GoToAsync("//CustomerPage");
        }

        [RelayCommand]
        private async Task NavigateToEmployeeMenu()
        {
            Debug.WriteLine("Navigerar Till Employees...");
            await Shell.Current.GoToAsync("//EmployeePage");
        }

        [RelayCommand]
        private async Task NavigateToProjectMenu()
        {
            Debug.WriteLine("Navigerar Till Projects...");
            await Shell.Current.GoToAsync("//ProjectPage");
        }
    }
}
