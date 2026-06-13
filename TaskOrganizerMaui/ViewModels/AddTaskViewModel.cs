using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskOrganizerMaui.Models;
using TaskOrganizerMaui.Services;

namespace TaskOrganizerMaui.ViewModels;

public partial class AddTaskViewModel : ObservableObject
{
    private readonly TaskService _taskService;
    private TaskItem? _taskToEdit;

    [ObservableProperty] private String _title = String.Empty;
    [ObservableProperty] private String _description = String.Empty;
    [ObservableProperty] private TaskCategory _selectedCategory = TaskCategory.Work;
    [ObservableProperty] private String _pageTitle = "Новая задача";
    [ObservableProperty] private String _saveButtonText = "Сохранить";

    public ObservableCollection<TaskCategory> Categories { get; } = [
        TaskCategory.Work, 
        TaskCategory.Study, 
        TaskCategory.Personal
    ];

    public AddTaskViewModel(TaskService taskService)
    {
        _taskService = taskService;
    }

    public void SetTaskToEdit(TaskItem? task)
    {
        _taskToEdit = task;
        if (task != null)
        {
            Title = task.Title;
            Description = task.Description;
            SelectedCategory = task.Category;
            PageTitle = "Редактирование задачи";
            SaveButtonText = "Обновить";
        }
        else
        {
            Title = String.Empty;
            Description = String.Empty;
            SelectedCategory = TaskCategory.Work;
            PageTitle = "Новая задача";
            SaveButtonText = "Сохранить";
        }
    }

    [RelayCommand]
    private void SaveTask()
    {
        if (_taskToEdit != null)
        {
            _taskToEdit.Title = Title;
            _taskToEdit.Description = Description;
            _taskToEdit.Category = SelectedCategory;

            _taskService.UpdateTask(_taskToEdit);
        }
        else
        {
            _taskService.AddTask(Title, Description, SelectedCategory);
        }

        Application.Current.MainPage.Navigation.PopAsync();
    }

    [RelayCommand]
    private void Cancel()
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}