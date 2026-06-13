using TaskOrganizerMaui.ViewModels;

namespace TaskOrganizerMaui;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

        Appearing += OnPageAppearing;
    }

    private void OnPageAppearing(Object? sender, EventArgs e)
    {
        _viewModel.RefreshTasks();
    }
}