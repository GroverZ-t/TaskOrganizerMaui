using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskOrganizerMaui.Models;
using TaskOrganizerMaui.Services;

namespace TaskOrganizerMaui.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private readonly IServiceProvider _serviceProvider;
    private Boolean _isFilterUpdating = false;

    [ObservableProperty]
    private ObservableCollection<TaskItem> _tasks = [];

    [ObservableProperty]
    private TaskCategory? _selectedCategory;

    public ObservableCollection<String> FilterCategories { get; } =
    [
        "Все категории",
        "Работа",
        "Учёба",
        "Личное"
    ];

    [ObservableProperty]
    private String _selectedFilterString = "Все категории";

    public MainViewModel(TaskService taskService, IServiceProvider serviceProvider)
    {
        _taskService = taskService;
        _serviceProvider = serviceProvider;
        LoadTasks();
    }

    partial void OnSelectedFilterStringChanged(String value)
    {
        if (_isFilterUpdating)
        {
            return;
        }

        _isFilterUpdating = true;
        try
        {
            SelectedCategory = value switch
            {
                "Работа" => TaskCategory.Work,
                "Учёба" => TaskCategory.Study,
                "Личное" => TaskCategory.Personal,
                _ => null
            };

            LoadTasks();
        }
        finally
        {
            _isFilterUpdating = false;
        }
    }

    private void LoadTasks()
    {
        List<TaskItem> filteredTasks = _taskService.GetFilteredTasks(SelectedCategory).ToList();

        Tasks.Clear();
        foreach (TaskItem task in filteredTasks)
        {
            Tasks.Add(task);
        }
    }

    public void RefreshTasks()
    {
        LoadTasks();
    }

    [RelayCommand]
    private void ToggleTaskCompletion(TaskItem task)
    {
        _taskService.ToggleTaskCompletion(task.Id);
    }

    [RelayCommand]
    private void DeleteTask(TaskItem task)
    {
        _taskService.DeleteTask(task.Id);

        Tasks.Remove(task);
    }

    [RelayCommand]
    private async Task OpenAddTaskPageAsync()
    {
        var addVm = _serviceProvider.GetRequiredService<AddTaskViewModel>();

        addVm.SetTaskToEdit(null);

        var page = new AddTaskPage { BindingContext = addVm };

        await Application.Current.MainPage.Navigation.PushAsync(page);
    }

    [RelayCommand]
    private async Task EditTask(TaskItem task)
    {
        AddTaskViewModel editVm = _serviceProvider.GetRequiredService<AddTaskViewModel>();
        editVm.SetTaskToEdit(task);

        AddTaskPage page = new() { BindingContext = editVm };
        await Application.Current.MainPage.Navigation.PushAsync(page);
    }
}