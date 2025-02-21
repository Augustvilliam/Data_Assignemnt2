
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Factories;
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
    private ObservableCollection<CustomerDto> customers = new();
    [ObservableProperty]
    private CustomerDto newCustomer = new();
    [ObservableProperty]
    private CustomerDto selectedCustomer;

    public CustomerViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        _ = LoadCustomers();
    }

    public async Task LoadCustomers()
    {
        try
        {
            var customerList = await _customerService.GetAllCustomersAsync(); 
            Customers = new ObservableCollection<CustomerDto>(customerList); 
            if (Customers.Count > 0)
                SelectedCustomer = Customers.First();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error loading customers: {ex.Message}");
        }
    }
    [RelayCommand]
    public async Task AddCustomers()
    {
        var customer = CustomerFactory.CreateCustomer(NewCustomer); 

        var errors = ValidationHelper.ValidateCustomer(NewCustomer);
        if (errors.Count > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _customerService.AddCustomersAsync(customer); 
        await LoadCustomers();
        NewCustomer = new();
    }
    [RelayCommand]
    public async Task DeleteCustomer()
    {
        if (SelectedCustomer != null)
        {
            await _customerService.DeleteCustomersAsync(SelectedCustomer.Id);
            await LoadCustomers();
        }
        else
        {
            Debug.WriteLine("❌ Ingen kund vald.");
        }
    }
    [RelayCommand]
    public async Task NavigateBack()
    {
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
            await Application.Current.MainPage.DisplayAlert("Error", "You must select a customer before editing!", "OK");
        }
    }
}
