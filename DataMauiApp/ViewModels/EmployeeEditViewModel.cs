


using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Helper;
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
    private readonly IRoleService _roleService;
    private readonly IServiceService _serviceService;

    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees = new();
    [ObservableProperty]
    private ObservableCollection<RoleEntity> roles = new();
    [ObservableProperty]
    private ObservableCollection<ServiceEntity> availableServices = new();
    [ObservableProperty]
    private ObservableCollection<ServiceEntity> selectedServices = new();
    [ObservableProperty]
    private ObservableCollection<ProjectEntity> projects = new();
    [ObservableProperty]
    private EmployeeEntity selectedEmployee;



    public EmployeeEditViewModel(IEmployeeService employeeService, IRoleService roleService, IServiceService serviceService)
    {
        _employeeService = employeeService;
        _roleService = roleService;
        _serviceService = serviceService;
        _ = LoadData();
    }

    public async Task LoadData()
    {
        try
        {
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());
            Roles = new ObservableCollection<RoleEntity>(await _roleService.GetAllRolesAsync());
            AvailableServices = new ObservableCollection<ServiceEntity>(await _serviceService.GetAllServicesAsync());

            if (SelectedEmployee != null)
            {
                SelectedServices = new ObservableCollection<ServiceEntity>(SelectedEmployee.Services);
                Projects = new ObservableCollection<ProjectEntity>(SelectedEmployee.Projects);
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error loading data: {ex.Message}");
        }
    }

    partial void OnSelectedEmployeeChanged(EmployeeEntity value)
    {
        if (value != null)
        {
            Debug.WriteLine($"🟢 Selected Employee: {value.FirstName}");

            SelectedServices = new ObservableCollection<ServiceEntity>(value.Services);
            Projects = new ObservableCollection<ProjectEntity>(value.Projects);
        }
    }

    public void ToggleServiceSelection(ServiceEntity service, bool isChecked)
    {
        if (isChecked)
        {
            if (!SelectedServices.Any(s => s.Id == service.Id))
                SelectedServices.Add(service);
        }
        else
        {
            var existingService = SelectedServices.FirstOrDefault(s => s.Id == service.Id);
            if (existingService != null)
                SelectedServices.Remove(existingService);
        }

        Debug.WriteLine($"🛠️ Selected Services: {string.Join(", ", SelectedServices.Select(s => s.Name))}");
    }


    [RelayCommand]
    public async Task SaveChanges()
    {
        if (SelectedEmployee != null)
        {
            SelectedEmployee.Services = SelectedServices.ToList();
            await _employeeService.UpdateEmployeeAsync(SelectedEmployee);

            // 🟢 Ladda om hela listan från databasen efter uppdatering
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());

            // 🔄 Se till att den valda anställda fortfarande är markerad
            SelectedEmployee = Employees.FirstOrDefault(e => e.Id == SelectedEmployee.Id);

            Debug.WriteLine("✅ Employee Updated and UI Refreshed");
        }
        else
        {
            Debug.WriteLine("❌ Error: No employee selected for update.");
        }
    }


    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka till EmployeePage...");
        await Shell.Current.GoToAsync("//EmployeePage");
    }
}
