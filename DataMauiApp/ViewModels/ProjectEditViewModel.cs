
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Helpers;

namespace DataMauiApp.ViewModels;
[QueryProperty(nameof(SelectedProject), "Project")]
public partial class ProjectEditViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;


    [ObservableProperty]
    private ObservableCollection<ProjectDto> projects = new();
    [ObservableProperty]
    private ObservableCollection<CustomerDto> customers = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeDto> employees = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> services = new();
    [ObservableProperty]
    private ProjectDto selectedProject;
    [ObservableProperty]
    private CustomerDto selectedCustomer;
    [ObservableProperty]
    private EmployeeDto selectedEmployee;
    [ObservableProperty]
    private ServiceDto selectedService;
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

    partial void OnSelectedProjectChanged(ProjectDto value) //genererad av chatGPt
    {
        if (value != null)
        {

            _ = LoadData();
        }
    }
    private async Task LoadData() //laddar samtliga saker some behövs 
    {
        try
        {
            Projects = new ObservableCollection<ProjectDto>(await _projectService.GetAllProjectsAsync());
            Customers = new ObservableCollection<CustomerDto>(await _customerService.GetAllCustomersAsync());
            Employees = new ObservableCollection<EmployeeDto>(await _employeeService.GetAllEmployeesAsync());
            Services = new ObservableCollection<ServiceDto>(await _serviceService.GetAllServicesAsync());
            if (SelectedProject != null)
            {
                SelectedCustomer = Customers.FirstOrDefault(c => c.Id == SelectedProject.CustomerId);
                SelectedEmployee = Employees.FirstOrDefault(e => e.Id == SelectedProject.EmployeeId);
                SelectedService = Services.FirstOrDefault(s => s.Id == SelectedProject.ServiceId);
                SelectedStatus = SelectedProject.Status;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        if (SelectedProject == null)
        {
            await AlertHelper.ShowSelectionAlert("Project");
            return;
        }
        var errors = ValidationHelper.ValidateProject(SelectedProject);
        if (errors.Count > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }
        SelectedProject.CustomerId = SelectedCustomer?.Id ?? 0;
        SelectedProject.EmployeeId = SelectedEmployee?.Id ?? 0;
        SelectedProject.ServiceId = SelectedService?.Id ?? 0;
        SelectedProject.Status = SelectedStatus;

        await _projectService.UpdateProjectAsync(SelectedProject);
        await LoadData();
    }

    [RelayCommand]
    public async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("//ProjectPage");
    }
}
