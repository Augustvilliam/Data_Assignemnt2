
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;

public partial class CustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    private ObservableCollection<CustomerEntity> customers = new();

    [ObservableProperty]
    private CustomerEntity newCustomer = new();

    [ObservableProperty]
    private CustomerEntity selectedCustomer;

    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        _ = LoadCustomers();
        Debug.WriteLine("Jag har Laddat anvädnare.");

    }

    public async Task LoadCustomers()
    {
        try
        {
            Customers = new ObservableCollection<CustomerEntity>(await _customerService.GetAllCustomersAsync());
            if (Customers.Count > 0)
                SelectedCustomer = Customers.First();
            Debug.WriteLine("Jag har Laddat anvädnare.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"something went wrong when loading Custoemrs {ex.Message}.");
        }
    }

    [RelayCommand]
    public async Task AddCustomers()
    {
        var errors = ValidationHelper.ValidateCustomer(NewCustomer);

        if (errors.Count > 0)
        {
            Debug.WriteLine($"❌ Valideringsfel: {string.Join(", ", errors)}");
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _customerService.AddCustomersAsync(NewCustomer);
        Debug.WriteLine($"✅ Customer tillagd: {NewCustomer.FirstName} {NewCustomer.LastName}");

        await LoadCustomers();
        NewCustomer = new();
    }

    [RelayCommand]
    public async Task DeleteCustomer()
    {
        if (SelectedCustomer != null)
        {
            await _customerService.DeleteCustomersAsync(SelectedCustomer.Id);
            Debug.WriteLine($"{SelectedCustomer.FirstName} raderad.");
            await LoadCustomers();
        }
        else
        {
            Debug.WriteLine("Ingen kund vald.");
        }
    }


    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("//MainMenuPage");
    } 
   
    
    [RelayCommand]
    public async Task OpenEditMode()
    {
        if (SelectedCustomer != null)
        {
            await Shell.Current.GoToAsync("//CustomerEditPage", new Dictionary<string, object>
            {
                ["Customer"] = SelectedCustomer
            });
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select an Customer before editing!", "OK");
            Debug.WriteLine("No Customer Selected");
        }
    }
}
