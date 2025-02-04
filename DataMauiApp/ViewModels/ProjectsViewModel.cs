
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
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
        private ObservableCollection<ProjectEntity> projects = new();

        public AsyncRelayCommand<int> ViewProjectCommand { get; }
        public AsyncRelayCommand AddProjectCommand { get; }

        public ProjectsViewModel(IProjectService projectService)
        {
            _projectService = projectService;
            LoadProjects();

            ViewProjectCommand = new AsyncRelayCommand<int>(ViewProject);
            AddProjectCommand = new AsyncRelayCommand(AddProject);
        }

        private async void LoadProjects()
        {
            try
            {
                var projectList = await _projectService.GetAllProjectsAsync();
                Projects = new ObservableCollection<ProjectEntity>(projectList);
                Debug.WriteLine($"Antal projekt laddade: {Projects.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading projects: {ex.Message}");
            }
            
        }

        private async Task ViewProject(int id)
        {
            var selectedProject = Projects.FirstOrDefault(p => p.Id == id);
            if (selectedProject != null)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CurrentProject", selectedProject }
                };
                await Shell.Current.GoToAsync(nameof(ProjectPage), parameters);
            }
            else
            {
                Debug.WriteLine($"Projekt med ID {id} hittades ej!");
            }
        }
        private async Task AddProject()
        {
            await Shell.Current.GoToAsync(nameof(ProjectPage));
        }

    }
}
