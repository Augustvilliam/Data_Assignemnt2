
using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;
using DataMauiApp.Views;

namespace DataMauiApp.ViewModels
{
    public partial class ProjectsViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;

        [ObservableProperty]
        private ObservableCollection<ProjectEntity> projects;

        public ProjectsViewModel(IProjectService projectService)
        {
            _projectService = projectService;
            LoadProjects();
        }
        private async void LoadProjects()
        {
            try
            {
                var projectList = await _projectService.GetAllProjectsAsync();
                Projects = new ObservableCollection<ProjectEntity>(projectList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading projects: {ex.Message}");
            }
            Debug.WriteLine("LoadProject Funkar.");
        }


        [RelayCommand]
        private async Task AddProject()
        {
            await Shell.Current.GoToAsync(nameof(ProjectPage));
        }
        [RelayCommand]
        private async Task ViewProject(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CurrentProject", Projects.FirstOrDefault(p => p.Id == id) }
            };
            await Shell.Current.GoToAsync(nameof(ProjectPage), parameters);
            Debug.WriteLine("ViewProjects Funkar.");

        }
    }
}
