

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;

public partial class EmployeeViewModel : ObservableObject
{

    private readonly IRoleService _roleService;
    private readonly IEmployeeService _employeeService;



    [ObservableProperty]
    private ObservableCollection<RoleEntity> roles = new();

    [ObservableProperty]
    private RoleEntity selectedRole;

    [ObservableProperty]
    private EmployeeEntity newEmployee = new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        Email = string.Empty,
        PhoneNumber = string.Empty
    };


public EmployeeViewModel(IRoleService roleService, IEmployeeService employeeService)
    {
        _roleService = roleService;
        _employeeService = employeeService;
        LoadRoles();
    }

    public async void LoadRoles()
    {
        try
        {
            var roleList = await _roleService.GetAllRolesAsync();
            Roles = new ObservableCollection<RoleEntity>(roleList);
            Debug.WriteLine($"Antal roller laddade: {Roles.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av roller. { ex.Message} ");
        }
    }

    [RelayCommand]
    public async Task SaveEmployee()
    {
        if (string.IsNullOrWhiteSpace(NewEmployee.FirstName) ||
        string.IsNullOrWhiteSpace(NewEmployee.LastName) ||
        string.IsNullOrWhiteSpace(NewEmployee.Email) ||
        string.IsNullOrWhiteSpace(NewEmployee.PhoneNumber) || 
        SelectedRole == null)
        {
            Debug.Write("Alla fält måste fyllas i");
            return;

        }
        NewEmployee.RoleId = SelectedRole.Id;
        await _employeeService.AddEmployee(NewEmployee);
        Debug.WriteLine($"Nu antälld tilllagd.");
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("..");
    }
}
