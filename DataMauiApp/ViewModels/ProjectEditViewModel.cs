
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;

public partial class ProjectEditViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;


    [ObservableProperty]
    private ObservableCollection<ProjectEntity> projects = new();
    [ObservableProperty]
    private ObservableCollection<CustomerEntity> customers = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees = new();
    [ObservableProperty]
    private ObservableCollection<ServiceEntity> services = new();
    [ObservableProperty]
    private ProjectEntity selectedProject;
    [ObservableProperty]
    private CustomerEntity selectedCustomer;
    [ObservableProperty]
    private EmployeeEntity selectedEmployee;
    [ObservableProperty]
    private ServiceEntity selectedService;
    [ObservableProperty]
    private string selectedStatus;

    public ProjectEditViewModel(
    IProjectService projectService,
    ICustomerService customerService,
    IEmployeeService employeeService,
    IServiceService serviceService)
    {
        _projectService = projectService;
        _customerService = customerService;
        _employeeService = employeeService;
        _serviceService = serviceService;

        _ = LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            Projects = new ObservableCollection<ProjectEntity>(await _projectService.GetAllProjectsAsync());
            Customers = new ObservableCollection<CustomerEntity>(await _customerService.GetAllCustomersAsync());
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());
            Services = new ObservableCollection<ServiceEntity>(await _serviceService.GetAllServicesAsync());

            Debug.WriteLine($"Laddade {Projects.Count} projekt, {Customers.Count} kunder, {Employees.Count} anställda, {Services.Count} tjänster.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av data: {ex.Message}");
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
        Debug.WriteLine("Navigerar tillbaka till ProjectPage...");
        await Shell.Current.GoToAsync("//ProjectPage");
    }
}
