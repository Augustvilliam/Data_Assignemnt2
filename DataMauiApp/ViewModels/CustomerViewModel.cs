﻿
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        LoadCustomers();
    }

    public async void LoadCustomers()
    {
        try
        {
            var customerList = await _customerService.GetAllCustomersAsync();
            Customers = new ObservableCollection<CustomerEntity>(customerList);
            Debug.WriteLine($"{Customers.Count} Customers Laddades");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Customers laddades inte {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task AddCustomers()
    {
        if (!string.IsNullOrEmpty(NewCustomer.FirstName) &&
            !string.IsNullOrEmpty(NewCustomer.LastName) &&
            !string.IsNullOrEmpty(NewCustomer.Email) &&
            !string.IsNullOrEmpty(NewCustomer.PhoneNumber))
        {
            await _customerService.AddCustomersAsync(NewCustomer);
            Debug.WriteLine("Customer tillagd");
            LoadCustomers();
            NewCustomer = new();
        }
        else
        {
            Debug.WriteLine("Samtliga fält måste fyllas i.");
        }
    }
    [RelayCommand]
    public async Task DeleteCustomer()
    {
        if (SelectedCustomer != null)
        {
            await _customerService.DeleteCustomersAsync(SelectedCustomer.Id);
            Debug.WriteLine($"{SelectedCustomer.FirstName} raderad.");
            LoadCustomers();
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
        await Shell.Current.GoToAsync("..");
    }
}
