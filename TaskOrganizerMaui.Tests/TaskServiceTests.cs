using Moq;
using TaskOrganizerMaui.Interfaces;
using TaskOrganizerMaui.Models;
using TaskOrganizerMaui.Services;
using Xunit;

namespace TaskOrganizerMaui.Tests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly Mock<INotificationService> _mockNotification;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _mockNotification = new Mock<INotificationService>();

        _service = new TaskService(_mockRepo.Object, _mockNotification.Object);
    }

    [Fact]
    public void AddTask_ValidData_ShouldAddAndNotify()
    {
        _service.AddTask("Сделать тесты", "Описание задачи", TaskCategory.Work);

        _mockRepo.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Once);
        _mockNotification.Verify(n => n.ShowNotification(It.Is<String>(s => s.Contains("успешно добавлена"))), Times.Once);
    }

    [Fact]
    public void AddTask_EmptyTitle_ShouldNotAddAndNotifyError()
    {
        _service.AddTask("", "Описание", TaskCategory.Work);

        _mockRepo.Verify(r => r.Add(It.IsAny<TaskItem>()), Times.Never);
        _mockNotification.Verify(n => n.ShowNotification(It.Is<String>(s => s.Contains("Ошибка"))), Times.Once);
    }

    [Fact]
    public void GetFilteredTasks_NoFilter_ShouldReturnAllSorted()
    {
        var now = DateTime.Now;
        List<TaskItem> tasks =
        [
            new() { Id = 1, Category = TaskCategory.Work, CreatedAt = now.AddDays(-1) },
            new() { Id = 2, Category = TaskCategory.Study, CreatedAt = now }
        ];
        _mockRepo.Setup(r => r.GetAll()).Returns(tasks);

        var result = _service.GetFilteredTasks().ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[0].Id);
    }

    [Fact]
    public void GetFilteredTasks_ByCategory_ShouldReturnOnlyMatching()
    {
        List<TaskItem> tasks =
        [
            new() { Id = 1, Category = TaskCategory.Work },
            new() { Id = 2, Category = TaskCategory.Study },
            new() { Id = 3, Category = TaskCategory.Work }
        ];
        _mockRepo.Setup(r => r.GetAll()).Returns(tasks);

        var result = _service.GetFilteredTasks(category: TaskCategory.Work).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(TaskCategory.Work, t.Category));
    }

    [Fact]
    public void ToggleTaskCompletion_ExistingTask_ShouldUpdate()
    {
        var task = new TaskItem { Id = 1, IsCompleted = false };
        var tasks = new List<TaskItem> { task };
        _mockRepo.Setup(r => r.GetAll()).Returns(tasks);

        _service.ToggleTaskCompletion(1);

        Assert.True(task.IsCompleted);
        _mockRepo.Verify(r => r.Update(task), Times.Once);
    }

    [Fact]
    public void DeleteTask_ShouldCallDeleteOnRepository()
    {
        _service.DeleteTask(5);

        _mockRepo.Verify(r => r.Delete(5), Times.Once);
        _mockNotification.Verify(n => n.ShowNotification(It.Is<String>(s => s.Contains("удалена"))), Times.Once);
    }

    [Fact]
    public void UpdateTask_ShouldCallUpdateOnRepositoryAndNotify()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Старое название", Category = TaskCategory.Work };

        // Act
        task.Title = "Новое название";
        _service.UpdateTask(task);

        // Assert
        _mockRepo.Verify(r => r.Update(task), Times.Once);
        _mockNotification.Verify(n => n.ShowNotification(It.Is<String>(s => s.Contains("обновлена"))), Times.Once);
    }
}