using System.Diagnostics;
using TaskOrganizerMaui.Interfaces;

namespace TaskOrganizerMaui.Services;

public class DebugNotificationService : INotificationService
{
    public void ShowNotification(String message)
    {
        Debug.WriteLine($"[УВЕДОМЛЕНИЕ]: {message}");
    }
}