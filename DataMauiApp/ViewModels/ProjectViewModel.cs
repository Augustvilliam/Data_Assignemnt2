

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Views;

namespace DataMauiApp.ViewModels;

 
public partial class ProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;

    [ObservableProperty]
    private ObservableCollection<ProjectEntity>  projects = new();
    [ObservableProperty]
    private ObservableCollection<CustomerEntity> customers = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeEntity> employees = new();
    [ObservableProperty]
    private ObservableCollection<ServiceEntity> services = new();
    [ObservableProperty]
    private ProjectEntity newProject = new();
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
    private async Task SaveProject()
    {
        NewProject.CustomerId = SelectedCustomer?.Id ?? 0;
        NewProject.EmployeeId = SelectedEmployee?.Id ?? 0;
        NewProject.ServiceId = SelectedService?.Id ?? 0;
        NewProject.Status = SelectedStatus;

        var errors = ValidationHelper.ValidateProject(NewProject);

        if (errors.Count > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _projectService.AddProject(NewProject);
        await LoadData();
        NewProject = new();
    }

    [RelayCommand]
    public async Task DeleteProject()
    {
        if (SelectedProject != null && SelectedProject.Id != 0)
            {
            await _projectService.DeleteProjectAsync(SelectedProject.Id);
            Debug.WriteLine($"🗑️ Projekt '{SelectedProject.Name}' raderat.");
            LoadData();
            SelectedProject = null;
            }
    }

    [RelayCommand]
    public async Task OpenEditMode()
    {
        if (SelectedProject != null)
        {
            await Shell.Current.GoToAsync("//ProjectEditPage", new Dictionary<string, object>
            {
                ["Project"] = SelectedProject
            });
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select an employee before editing!", "OK");
            Debug.WriteLine("No Project Selected");
        }
    }
    [RelayCommand]
    public async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("//MainMenuPage");
    }
}
