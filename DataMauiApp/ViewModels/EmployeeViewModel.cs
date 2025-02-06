

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
    private EmployeeEntity newEmployee = new();



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
        if (SelectedRole != null)
        {
            newEmployee.RoleId = selectedRole.Id;
            await _employeeService.AddEmployee(newEmployee);
            Debug.WriteLine($"Nu antälld tilllagd.");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            Debug.WriteLine("❌ Ingen roll vald!");
        }
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("..");
    }
}
