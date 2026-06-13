using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskOrganizerMaui.Models;

public partial class TaskItem : ObservableObject
{
    [ObservableProperty] private Int32 _id;
    [ObservableProperty] private String _title = String.Empty;
    [ObservableProperty] private String _description = String.Empty;
    [ObservableProperty] private TaskCategory _category;
    [ObservableProperty] private Boolean _isCompleted;
    [ObservableProperty] private DateTime _createdAt = DateTime.Now;
}