

using Busniess.Interface;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using System.Diagnostics;
using Busniess.Helper;
using Busniess.Dtos;
using Busniess.Services;
using DataMauiApp.Helpers;

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

    public async Task LoadCustomers() //laddar en fräsch lista över employees. 
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
        if (SelectedCustomer == null) //ingen användare vald, ajabaja
        {
            await AlertHelper.ShowSelectionAlert("Customer");
            return;
        }

        var errors = await ValidationHelper.ValidateCustomer(SelectedCustomer, _customerService);
        if (errors.Count > 0)
        {
            Debug.WriteLine($"Valideringsfel: {string.Join(", ", errors)}");
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _customerService.UpdateCustomersAsync(SelectedCustomer); //allt som det ska? suuuuperbra då får du spara 
        await LoadCustomers(); //laddar om listan med Customers
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("//CustomerPage");
    }
}
