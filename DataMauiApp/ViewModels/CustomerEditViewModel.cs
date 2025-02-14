

using Busniess.Interface;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using System.Diagnostics;
using Busniess.Helper;

namespace DataMauiApp.ViewModels
{
    public partial class CustomerEditViewModel : ObservableObject
    {
        private readonly ICustomerService _customerService
            ;

        [ObservableProperty]
        private ObservableCollection<CustomerEntity> customers = new();

        [ObservableProperty]
        private CustomerEntity selectedCustomer;

        public CustomerEditViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
            _ = LoadCustomers();
        }

        public async Task LoadCustomers()
        {
            try
            {
                Customers = new ObservableCollection<CustomerEntity>(await _customerService.GetAllCustomersAsync());
                if (Customers.Count > 0)
                    SelectedCustomer = Customers.First();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"something went wrong when loading Custoemrs {ex.Message}.");
            }
        }

        [RelayCommand]
        public async Task SaveChanges()
        {
            if (SelectedCustomer == null)
            {
                Debug.WriteLine("❌ Ingen kund vald.");
                return;
            }

            var errors = ValidationHelper.ValidateCustomer(SelectedCustomer);
            if (errors.Count > 0)
            {
                Debug.WriteLine($"❌ Valideringsfel: {string.Join(", ", errors)}");
                await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
                return;
            }

            await _customerService.UpdateCustomersAsync(SelectedCustomer);
            Debug.WriteLine($"✅ Kund '{SelectedCustomer.FirstName} {SelectedCustomer.LastName}' uppdaterad.");

            await LoadCustomers();
        }

        [RelayCommand]
        public async Task NavigateBack()
        {
            Debug.WriteLine("Navigerar tillbaka till CustomerPage...");
            await Shell.Current.GoToAsync("//CustomerPage");
        }
    }
}
