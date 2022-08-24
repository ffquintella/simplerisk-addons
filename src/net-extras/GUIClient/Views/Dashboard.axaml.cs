using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.ViewModels;

namespace GUIClient.Views;

public partial class Dashboard : UserControl
{
    public Dashboard()
    {
        DataContext = new DashboardViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}