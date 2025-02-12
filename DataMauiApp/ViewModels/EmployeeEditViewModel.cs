
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataMauiApp.ViewModels;

[QueryProperty(nameof(SelectedEmployee), "Employee")]
public partial class EmployeeEditViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;

    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees  = new();

    [ObservableProperty]
    private EmployeeEntity selectedEmployee;

    public EmployeeEditViewModel(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
        _ = LoadEmployees();
    }

    public async Task LoadEmployees()
    {
        try
        {
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());
            if (Employees.Count > 0)
                SelectedEmployee = Employees.First();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"something went wrong when loading users {ex.Message}.");
        }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        if (SelectedEmployee != null)
        {
            await _employeeService.UpdateEmployeeAsync(SelectedEmployee);
            Debug.WriteLine("Employee Updated");
        }
        else
        {
            Debug.WriteLine("Somehting went wrong");
        }
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka till EmployeePage...");
        await Shell.Current.GoToAsync("//EmployeePage");
    }
}
