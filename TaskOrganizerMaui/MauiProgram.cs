using Microsoft.Extensions.Logging;
using TaskOrganizerMaui.Interfaces;
using TaskOrganizerMaui.Services;
using TaskOrganizerMaui.ViewModels;

namespace TaskOrganizerMaui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<ITaskRepository, JsonTaskRepository>();

            builder.Services.AddSingleton<INotificationService, DebugNotificationService>();

            builder.Services.AddSingleton<TaskService>();

            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<AddTaskViewModel>();

            builder.Services.AddTransient<AddTaskPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}