

using Busniess.Interface;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using System.Diagnostics;

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
            if (SelectedCustomer != null)
            {
                await _customerService.UpdateCustomersAsync(SelectedCustomer);
                Debug.WriteLine("Customer Updated");
            }
            else
            {
                Debug.WriteLine("Somehting went wrong");
            }
        }

        [RelayCommand]
        public async Task NavigateBack()
        {
            Debug.WriteLine("Navigerar tillbaka till CustomerPage...");
            await Shell.Current.GoToAsync("//CustomerPage");
        }
    }
}
