
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using Busniess.Interface;
using MobileApp.Pages;

namespace MobileApp.ViewModels;

public partial class ProjectViewModel : ObservableObject
{
    private readonly IProjectService _projectService;

    [ObservableProperty]
    private ObservableCollection<ProjectEntity> projects = new();
    public ICommand ViewProjectCommand { get; }
    public ICommand AddProjectCommand { get; }

    public ProjectViewModel(IProjectService projectService)
    {
        _projectService = projectService;

        ViewProjectCommand = new RelayCommand<int>(async (projectId) => await ViewProjectDetails(projectId));

        AddProjectCommand = new RelayCommand(async () => await AddNewProject());

        LoadProjects();
    } 

    private async void LoadProjects()
    {
        var projectList = await _projectService.GetAllProjectsAsync();
        Projects = new ObservableCollection<ProjectEntity>(projectList);
    }
    private async Task ViewProjectDetails(int projectId)
    {
        await Shell.Current.GoToAsync($"{nameof(ProjectPage)}?id={projectId}");
    }

    private async Task AddNewProject()
    {
        await Shell.Current.GoToAsync(nameof(ProjectPage));
    }

}

