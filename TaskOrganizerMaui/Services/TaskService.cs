using TaskOrganizerMaui.Interfaces;
using TaskOrganizerMaui.Models;

namespace TaskOrganizerMaui.Services;

public class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly INotificationService _notification;

    public TaskService(ITaskRepository repository, INotificationService notification)
    {
        _repository = repository;
        _notification = notification;
    }

    public void AddTask(String title, String description, TaskCategory category)
    {
        if (String.IsNullOrWhiteSpace(title))
        {
            _notification.ShowNotification("Ошибка: Название задачи не может быть пустым!");
            return;
        }

        TaskItem newTask = new()
        {
            Title = title,
            Description = description,
            Category = category,
            IsCompleted = false
        };

        _repository.Add(newTask);
        _notification.ShowNotification($"Задача '{title}' успешно добавлена!");
    }

    public void UpdateTask(TaskItem updatedTask)
    {
        _repository.Update(updatedTask);
        _notification.ShowNotification($"Задача '{updatedTask.Title}' успешно обновлена!");
    }

    public void DeleteTask(Int32 id)
    {
        _repository.Delete(id);
        _notification.ShowNotification("Задача удалена.");
    }

    public void ToggleTaskCompletion(Int32 id)
    {
        TaskItem? task = _repository.GetAll().FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            _repository.Update(task);
        }
    }

    public IEnumerable<TaskItem> GetFilteredTasks(TaskCategory? category = null, Boolean? isCompleted = null)
    {
        IEnumerable<TaskItem> tasks = _repository.GetAll();

        if (category.HasValue)
        {
            tasks = tasks.Where(t => t.Category == category.Value);
        }

        if (isCompleted.HasValue)
        {
            tasks = tasks.Where(t => t.IsCompleted == isCompleted.Value);
        }

        return tasks.OrderByDescending(t => t.CreatedAt).ToList();
    }
}