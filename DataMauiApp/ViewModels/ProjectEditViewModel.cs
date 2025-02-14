
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;
[QueryProperty(nameof(SelectedProject), "Project")]
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

    public List<string> StatusOptions { get; set; } = new() { "Not Started", "Ongoing", "Completed" };

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

    partial void OnSelectedProjectChanged(ProjectEntity value)
    {
        if (value != null)
        {

            _ = LoadData();
        }
    }
    private async Task LoadData()
    {
        try
        {
            Projects = new ObservableCollection<ProjectEntity>(await _projectService.GetAllProjectsAsync());
            Customers = new ObservableCollection<CustomerEntity>(await _customerService.GetAllCustomersAsync());
            Employees = new ObservableCollection<EmployeeEntity>(await _employeeService.GetAllEmployeesAsync());
            Services = new ObservableCollection<ServiceEntity>(await _serviceService.GetAllServicesAsync());

            if (SelectedProject != null)
            {
                SelectedCustomer = Customers.FirstOrDefault(c => c.Id == SelectedProject.CustomerId);
                SelectedEmployee = Employees.FirstOrDefault(e => e.Id == SelectedProject.EmployeeId);
                SelectedService = Services.FirstOrDefault(s => s.Id == SelectedProject.ServiceId);
                SelectedStatus = SelectedProject.Status;
            }

            Debug.WriteLine($"✅ Loaded {Projects.Count} projects, {Customers.Count} customers, {Employees.Count} employees, {Services.Count} services.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Error loading data: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        if (SelectedProject == null)
        {
            Debug.WriteLine("❌ No project selected.");
            return;
        }

        var errors = ValidationHelper.ValidateProject(SelectedProject);
        if (errors.Count > 0)
        {
            Debug.WriteLine($"❌ Validation errors: {string.Join(", ", errors)}");
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        SelectedProject.CustomerId = SelectedCustomer?.Id ?? 0;
        SelectedProject.EmployeeId = SelectedEmployee?.Id ?? 0;
        SelectedProject.ServiceId = SelectedService?.Id ?? 0;
        SelectedProject.Status = SelectedStatus;

        await _projectService.UpdateProjectAsync(SelectedProject);
        Debug.WriteLine($"✅ Project '{SelectedProject.Name}' updated.");

        await LoadData();
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka till ProjectPage...");
        await Shell.Current.GoToAsync("//ProjectPage");
    }
}
