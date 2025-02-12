

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Views;

namespace DataMauiApp.ViewModels;

public partial class EmployeeViewModel : ObservableObject
{

    private readonly IRoleService _roleService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;



    [ObservableProperty]
    private ObservableCollection<RoleEntity> roles = new();

    [ObservableProperty]
    private ObservableCollection<ServiceEntity> availableServices = new();

    [ObservableProperty]
    private ObservableCollection<ServiceEntity> selectedServices = new();

    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees = new();

    [ObservableProperty]
    private EmployeeEntity selectedEmployee;

    [ObservableProperty]
    private RoleEntity selectedRole;

    [ObservableProperty]
    private EmployeeEntity newEmployee = new()
    {
        FirstName = string.Empty,
        LastName = string.Empty,
        Email = string.Empty,
        PhoneNumber = string.Empty,
        Services = new List<ServiceEntity>()
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
            AvailableServices = new ObservableCollection<ServiceEntity>(serviceList);
            Debug.WriteLine($"Antal tjänster laddade: {AvailableServices.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av tjänster: {ex.Message}");
        }
    }
    public async Task LoadRoles()
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
    public async Task LoadEmployee()
    {
        try
        {
            var employeeList = await _employeeService.GetAllEmployeesAsync();
            Employees = new ObservableCollection<EmployeeEntity>(employeeList); // 🟢 Använd den genererade egenskapen!
            Debug.WriteLine($"Antal anställda laddade: {Employees.Count}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av roller. {ex.Message} ");
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
            Debug.WriteLine("Alla fält måste fyllas i.");
            return;
      

        }
        NewEmployee.RoleId = SelectedRole.Id;
        NewEmployee.Services = SelectedServices.ToList();

        await _employeeService.AddEmployee(NewEmployee);
        Debug.WriteLine($"✅ Ny anställd tillagd med {SelectedServices.Count} tjänster.");

        LoadEmployee();
        NewEmployee = new();

        await Shell.Current.GoToAsync("..");
    }
    [RelayCommand]
    public async Task DeleteEmployee()
    {
        if (SelectedEmployee != null)
        {
            await _employeeService.DeleteEmployeeAsync(SelectedEmployee.Id);
            Debug.WriteLine($"{SelectedEmployee.FirstName} raderad.");

            LoadEmployee();
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
    public void ToggleServiceSelection(ServiceEntity service, bool isChecked)
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
