

using Busniess.Interface;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using System.Diagnostics;
using Busniess.Helper;
using Busniess.Dtos;
using Busniess.Services;

namespace DataMauiApp.ViewModels;

public partial class CustomerEditViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;


    [ObservableProperty]
    private ObservableCollection<CustomerDto> customers = new();
    [ObservableProperty]
    private CustomerDto selectedCustomer;

    public CustomerEditViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        _ = LoadCustomers();
    }

    public async Task LoadCustomers()
    {
        try
        {
            var previousSelectedCustomerId = SelectedCustomer?.Id;

            Customers = new ObservableCollection<CustomerDto>(await _customerService.GetAllCustomersAsync());

            if (previousSelectedCustomerId.HasValue)
            {
                SelectedCustomer = Customers.FirstOrDefault(c => c.Id == previousSelectedCustomerId) ?? Customers.FirstOrDefault();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"something went wrong when loading Customers {ex.Message}.");
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

        var errors = await ValidationHelper.ValidateCustomer(SelectedCustomer, _customerService);
        if (errors.Count > 0)
        {
            Debug.WriteLine($"❌ Valideringsfel: {string.Join(", ", errors)}");
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _customerService.UpdateCustomersAsync(SelectedCustomer);
        await LoadCustomers();
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka till CustomerPage...");
        await Shell.Current.GoToAsync("//CustomerPage");
    }
}
