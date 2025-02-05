

using System.Collections.ObjectModel;
using System.Diagnostics;
using Busniess.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Entities;

namespace DataMauiApp.ViewModels;

public partial class EmployeeViewModel : ObservableObject
{

    private readonly IRoleService _roleService;
    [ObservableProperty]
    private ObservableCollection<RoleEntity> roles = new();

    [ObservableProperty]
    private RoleEntity selectedRole;

    public EmployeeViewModel(IRoleService roleService)
    {
        _roleService = roleService;
        LoadRoles();
    }

    public async void LoadRoles()
    {
        var roleList = await _roleService.GetAllRolesAsync();
        Roles = new ObservableCollection<RoleEntity>(roleList);
    }

    [RelayCommand]
    private async Task NavigateBack()
    {
        Debug.WriteLine("Navigerar tillbaka...");
        await Shell.Current.GoToAsync("..");
    }
}
