using System.Text.Json;
using TaskOrganizerMaui.Interfaces;
using TaskOrganizerMaui.Models;

namespace TaskOrganizerMaui.Services;

public class JsonTaskRepository : ITaskRepository
{
    private readonly String _filePath;
    private List<TaskItem> _tasks = [];

    public JsonTaskRepository()
    {
        String appDataDirectory = FileSystem.AppDataDirectory;
        _filePath = Path.Combine(appDataDirectory, "tasks.json");

        LoadTasks();
    }

    private void LoadTasks()
    {
        if (File.Exists(_filePath))
        {
            try
            {
                String json = File.ReadAllText(_filePath);

                if (String.IsNullOrWhiteSpace(json))
                {
                    _tasks = [];
                }
                else
                {
                    _tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
                }
            }
            catch (JsonException)
            {
                _tasks = [];
            }
        }
        else
        {
            _tasks = [];
        }
    }

    private void SaveTasks()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        String json = JsonSerializer.Serialize(_tasks, options);
        File.WriteAllText(_filePath, json);
    }

    public void Add(TaskItem task)
    {
        task.Id = _tasks.Count != 0 ? _tasks.Max(t => t.Id) + 1 : 1;
        task.CreatedAt = DateTime.Now;
        _tasks.Add(task);
        SaveTasks();
    }

    public void Delete(Int32 id)
    {
        TaskItem? task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
            SaveTasks();
        }
    }

    public void Update(TaskItem task)
    {
        Int32 index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index != -1)
        {
            _tasks[index] = task;
            SaveTasks();
        }
    }

    public IEnumerable<TaskItem> GetAll()
    {
        return _tasks.OrderByDescending(t => t.CreatedAt).ToList();
    }
}