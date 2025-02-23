


using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Helpers;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataMauiApp.ViewModels;

[QueryProperty(nameof(SelectedEmployee), "Employee")]
public partial class EmployeeEditViewModel : ObservableObject
{
    private readonly IEmployeeService _employeeService;
    private readonly IRoleService _roleService;
    private readonly IServiceService _serviceService;

    [ObservableProperty]
    private ObservableCollection<EmployeeDto> employees = new();
    [ObservableProperty]
    private ObservableCollection<RoleDto> roles = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> availableServices = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> selectedServices = new();
    [ObservableProperty]
    private ObservableCollection<ProjectDto> projects = new();
    [ObservableProperty]
    private EmployeeDto selectedEmployee;
    [ObservableProperty]
    private RoleDto selectedRole;


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
            Employees = new ObservableCollection<EmployeeDto>(await _employeeService.GetAllEmployeesAsync());
            Roles = new ObservableCollection<RoleDto>(await _roleService.GetAllRolesAsync());
            AvailableServices = new ObservableCollection<ServiceDto>(await _serviceService.GetAllServicesAsync());

            if (SelectedEmployee != null)
            {
                SelectedServices = new ObservableCollection<ServiceDto>(
                    SelectedEmployee.Services.Select(s => new ServiceDto { Id = s.Id, Name = s.Name }).ToList());

                Projects = new ObservableCollection<ProjectDto>(
                    SelectedEmployee.Projects.Select(p => new ProjectDto
                    {
                        Id = p.Id,
                        Name = p.Name
                    }).ToList()
                );

                SelectedRole = Roles.FirstOrDefault(r => r.Id == SelectedEmployee.RoleId);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error loading data: {ex.Message}");
        }
    }



    partial void OnSelectedEmployeeChanged(EmployeeDto value)
    {
        if (value != null)
        {
            SelectedServices = new ObservableCollection<ServiceDto>(
                value.Services.Select(s => new ServiceDto { Id = s.Id, Name = s.Name }).ToList()
            );

            Projects = new ObservableCollection<ProjectDto>(
                value.Projects.Select(p => new ProjectDto { Id = p.Id, Name = p.Name }).ToList()
            );
        }
    }

    public void ToggleServiceSelection(ServiceDto service, bool isChecked) //tanken var att denna skulle toggla så att vi Employees bara kunde utföra vissa services. Men fick aldrig skiten att funka...
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
    }


    [RelayCommand]
    public async Task SaveChanges()
    {
        if (SelectedEmployee != null)
        {
            var employeeDto = new EmployeeDto
            {
                Id = SelectedEmployee.Id,
                FirstName = SelectedEmployee.FirstName,
                LastName = SelectedEmployee.LastName,
                Email = SelectedEmployee.Email,
                PhoneNumber = SelectedEmployee.PhoneNumber,
                RoleId = SelectedRole?.Id ?? 0, 
                RoleName = SelectedRole?.Name ?? "",
                HourlyRate = SelectedRole?.Price ?? 0,
                Services = SelectedServices.ToList(),
                Projects = SelectedEmployee.Projects
            };

            await _employeeService.UpdateEmployeeAsync(employeeDto);

            Employees = new ObservableCollection<EmployeeDto>(await _employeeService.GetAllEmployeesAsync());

            SelectedEmployee = Employees.FirstOrDefault(e => e.Id == employeeDto.Id);
            SelectedRole = Roles.FirstOrDefault(r => r.Id == employeeDto.RoleId);
        }
        else
        {
            await AlertHelper.ShowSelectionAlert("Employee");
        }
    }



    [RelayCommand]
    public async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("//EmployeePage");
    }
}
