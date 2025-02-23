
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Dtos;
using Busniess.Helper;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataMauiApp.Helpers;


namespace DataMauiApp.ViewModels;

 
public partial class ProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;
    private readonly ICustomerService _customerService;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceService _serviceService;

    [ObservableProperty]
    private ObservableCollection<ProjectDto>  projects = new();
    [ObservableProperty]
    private ObservableCollection<CustomerDto> customers = new();
    [ObservableProperty]
    private ObservableCollection<EmployeeDto> employees = new();
    [ObservableProperty]
    private ObservableCollection<ServiceDto> services = new();

    [ObservableProperty]
    private ProjectDto newProject = new();

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
            Projects = new ObservableCollection<ProjectDto>(await _projectService.GetAllProjectsAsync());
            Customers = new ObservableCollection<CustomerDto>(await _customerService.GetAllCustomersAsync());
            Employees = new ObservableCollection<EmployeeDto>(await _employeeService.GetAllEmployeesAsync());
            Services = new ObservableCollection<ServiceDto>(await _serviceService.GetAllServicesAsync());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Fel vid laddning av data: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SaveProject()
    {
        if (SelectedCustomer == null || SelectedEmployee == null || SelectedService == null) //är någon null? ajabaja
        {
            await AlertHelper.ShowSelectionAlert("Project, Customer, Employee");
            return;
        }

        await Application.Current.MainPage.DisplayAlert("Saving Project...", "Chilla lite, detta kommer ta ungefär en livstid...", "OK"); //epic prank lol Xd
        await Task.Delay(2000);

        var projectDto = new ProjectDto //gör en dto
        {
            Name = NewProject.Name,
            Description = NewProject.Description,
            StartDate = NewProject.StartDate,
            EndDate = NewProject.EndDate,
            CustomerId = SelectedCustomer.Id,
            CustomerName = SelectedCustomer.FirstName + " " + SelectedCustomer.LastName,
            EmployeeId = SelectedEmployee.Id,
            EmployeeName = SelectedEmployee.FirstName + " " + SelectedEmployee.LastName,
            ServiceId = SelectedService.Id,
            ServiceName = SelectedService.Name,
            Status = SelectedStatus
        };


        var errors = ValidationHelper.ValidateProject(projectDto); //kör validationhelper för att kolla så allt är giltigt. 

        if (errors.Count > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", string.Join("\n", errors), "OK");
            return;
        }

        await _projectService.AddProject(projectDto); //lägger till projectet
        await LoadData(); //laddar om samtlig data så det är nytt och fräsch
        ClearProjectForm(); //rensar formuläret. 

        await Application.Current.MainPage.DisplayAlert("Success", "Skojar med dig hans, jag bara duuuuuumar maaaaaj", "OK");
    }
    [RelayCommand]
    public async Task DeleteProject()
    {
        if (SelectedProject != null && SelectedProject.Id != 0)
            {
            await _projectService.DeleteProjectAsync(SelectedProject.Id);
            LoadData();
            SelectedProject = null; //nollar selected project så det inte ligger kvar någon/crashar
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
            await AlertHelper.ShowSelectionAlert("project");
        }
    }
    [RelayCommand]
    public async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("//MainMenuPage");
    }

    private void ClearProjectForm() //nollar pickers och labels, status sätts till not started.
    {
        NewProject = new();
        SelectedCustomer = null;
        SelectedEmployee = null;
        SelectedService = null;
        SelectedStatus = "Not started";
    }
}
