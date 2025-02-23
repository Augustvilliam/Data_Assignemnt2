

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Factories;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Helpers;

namespace DataMauiApp.ViewModels;

public partial class EmployeeViewModel : ObservableObject
{

    private readonly IRoleService _roleService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;



    [ObservableProperty]
    private ObservableCollection<RoleDto> roles = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> availableServices = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> selectedServices = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeDto> employees = new();
    [ObservableProperty]
    private EmployeeDto selectedEmployee;
    [ObservableProperty]
    private RoleDto selectedRole;
    [ObservableProperty]
    private EmployeeDto newEmployee = new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        Email = string.Empty,
        PhoneNumber = string.Empty,
        Services = new List<ServiceDto>()
    };

    public EmployeeViewModel(IRoleService roleService, IEmployeeService employeeService, IServiceService serviceservice)
    {
        _roleService = roleService;
        _employeeService = employeeService;
        _serviceService = serviceservice;

        _ = LoadRoles();
        _ = LoadEmployee();
        _ = LoadServices();
    }
    public async Task LoadServices()
    {
        try
        {
            var serviceList = await _serviceService.GetAllServicesAsync();

            foreach (var service in serviceList)
            {
                service.IsSelected = SelectedServices.Any(s => s.Id == service.Id);
            }

            AvailableServices = new ObservableCollection<ServiceDto>(serviceList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av tjänster: {ex.Message}");
        }
    }
    public async Task LoadRoles()
    {
        Roles = new ObservableCollection<RoleDto>(await _roleService.GetAllRolesAsync());

        try
        {
            if (Roles.Count > 0 && SelectedRole == null)
            {
                SelectedRole = Roles.First();
            }
            var roleList = await _roleService.GetAllRolesAsync();
            Roles = new ObservableCollection<RoleDto>(roleList);
            Debug.WriteLine($"Antal roller laddade: {Roles.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av roller. { ex.Message} ");
        }
    }
    partial void OnSelectedRoleChanged(RoleDto? value)
    {
        SelectedRole = value ?? new RoleDto { Id = 0, Name = "Unknown", Price = 0 };
    }
    public async Task LoadEmployee()
    {
        try
        {
            var employeeList = await _employeeService.GetAllEmployeesAsync();
            Employees = new ObservableCollection<EmployeeDto>(employeeList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av anställda: {ex.Message}");
        }
    }
    [RelayCommand]
    public async Task SaveEmployee()
    {
        var employee = new EmployeeDto
        {
            FirstName = NewEmployee.FirstName,
            LastName = NewEmployee.LastName,
            Email = NewEmployee.Email,
            PhoneNumber = NewEmployee.PhoneNumber,
            RoleId = SelectedRole.Id,
            RoleName = SelectedRole.Name,
            HourlyRate = SelectedRole.Price,
            Services = SelectedServices.ToList()
        };

        var errors = await ValidationHelper.ValidateEmployee(employee, _employeeService);
        if (errors.Count > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _employeeService.AddEmployee(employee);
        await LoadEmployee();
        NewEmployee = new();
    }
    [RelayCommand]
    public async Task DeleteEmployee()
    {
        if (SelectedEmployee != null)
        {
            await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);
            await LoadEmployee();
        }
        else
        {
            await AlertHelper.ShowSelectionAlert("Employee");
        }
    }
    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("//MainMenuPage");
    }
    public void ToggleServiceSelection(ServiceDto service, bool isChecked)
    {
        if (isChecked)
        {
            if (!SelectedServices.Any(s => s.Id == service.Id))
            {
                SelectedServices.Add(service);
            }
        }
        else
        {
            var existingService = SelectedServices.FirstOrDefault(s => s.Id == service.Id);
            if (existingService != null)
            {
                SelectedServices.Remove(existingService);
            }
        }
        Debug.WriteLine($"🛠️ Valda tjänster: {string.Join(", ", SelectedServices.Select(s => s.Name))}");
    }
    [RelayCommand]
    public async Task OpenEditMode()
    {
        if (SelectedEmployee != null)
        {
            await Shell.Current.GoToAsync("//EmployeeEditPage", new Dictionary<string, object>
            {
                ["Employee"] = SelectedEmployee
            });
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select an employee before editing!", "OK");
            Debug.WriteLine("No employee Selected");
        }
    }

}
