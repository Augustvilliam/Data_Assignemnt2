

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

    [ObservableProperty]
    private ProjectEntity currentProject = new();
    public ProjectViewModel(IProjectService projectService)
    {
        _projectService = projectService;
    }
    [RelayCommand]
    private async Task SaveProject()
    {
        if (currentProject != null)
        {
            if (currentProject.Id == 0)
            {
                await _projectService.AddProject(currentProject);
            }
            else
            {
                await _projectService.UpdateProjectAsync(currentProject);
            }
        }
        await Shell.Current.GoToAsync("..");
    }
}
