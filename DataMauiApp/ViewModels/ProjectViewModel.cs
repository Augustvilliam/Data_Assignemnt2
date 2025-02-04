

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

    private ProjectEntity _currentProject = new();

    public ProjectEntity CurrentProject
    { 
        get => _currentProject;
        set => SetProperty(ref _currentProject, value);
    }

    public List<String> StatusOptions { get; set; } = new() { "Not Started", "Ongoing", "Compleate" };

    private string _selectedStatus;
    public string SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (_selectedStatus != value)
            {
                _selectedStatus = value;
                CurrentProject.Status = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        
        }
    }

    public ProjectViewModel(IProjectService projectService)
    {
        _projectService = projectService;

        if (!string.IsNullOrEmpty(CurrentProject.Status))
        {
            SelectedStatus = CurrentProject.Status;
        }
        else
        {
            SelectedStatus = StatusOptions.First();
        }
    }


    [RelayCommand]
    private async Task SaveProject()
    {
        if (CurrentProject != null)
        {
            CurrentProject.Status = SelectedStatus;

            if (CurrentProject.Id == 0)
            {
                await _projectService.AddProject(CurrentProject);
            }
            else
            {
                await _projectService.UpdateProjectAsync(CurrentProject);
            }
        }
        await Shell.Current.GoToAsync("..");
    }
}
