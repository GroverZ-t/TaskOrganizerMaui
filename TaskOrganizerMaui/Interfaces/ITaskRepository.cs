using TaskOrganizerMaui.Models;

namespace TaskOrganizerMaui.Interfaces;

public interface ITaskRepository
{
    void Add(TaskItem task);
    void Delete(Int32 id);
    void Update(TaskItem task);
    IEnumerable<TaskItem> GetAll();
}