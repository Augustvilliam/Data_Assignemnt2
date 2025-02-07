

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;

[QueryProperty(nameof(CurrentProject), "CurrentProject")]

public partial class ProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;

    [ObservableProperty]
    private ProjectEntity currentProject = new();
    [ObservableProperty]
    private ObservableCollection<CustomerEntity> customers = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees = new();
    [ObservableProperty]
    private ObservableCollection<ServiceEntity> services = new();
    [ObservableProperty]
    private CustomerEntity selectedCustomer;
    [ObservableProperty]
    private EmployeeEntity selectedEmployee;
    [ObservableProperty]
    private ServiceEntity selectedService;
    [ObservableProperty]
    private string selectedStatus;

    public List<string> StatusOptions { get; set; } = new() { "Not Started", "Ongoing", "Compleated" };

    public ProjectViewModel(
        IProjectService projectService,
        ICustomerService customerService,
        IEmployeeService employeeService,
        IServiceService serviceService)
    {
        _projectService = projectService;
        _customerService = customerService;
        _employeeService = employeeService;
        _serviceService = serviceService;

        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            Customers = new ObservableCollection<CustomerEntity>(await _customerService.GetAllCustomersAsync());
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());
            Services = new ObservableCollection<ServiceEntity>(await _serviceService.GetAllServicesAsync());

            Debug.WriteLine($"Laddade {Customers.Count} kunder, {Employees.Count} anställda, {Services.Count} tjänster.");
            SelectedStatus = CurrentProject.Status ?? StatusOptions.First();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av data: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveProject()
    {
        if (SelectedCustomer != null && SelectedEmployee != null && SelectedService != null)
        {
            CurrentProject.CustomerId = SelectedCustomer.Id;
            CurrentProject.EmployeeId = SelectedEmployee.Id;
            CurrentProject.ServiceId = SelectedService.Id;
            CurrentProject.Status = SelectedStatus;

            if (CurrentProject.Id == 0)
            {
                await _projectService.AddProject(CurrentProject);
            }
            else
            {
                await _projectService.UpdateProjectAsync(CurrentProject);
            }
            Debug.WriteLine("✅ Projekt sparat!");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            Debug.WriteLine("❌ Du måste välja kund, anställd och tjänst!");
        }
    }
    
    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("..");
    }
}
