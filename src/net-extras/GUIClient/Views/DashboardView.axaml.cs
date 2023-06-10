using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientServices.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Model.Statistics;

namespace ClientServices.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        DataContext = new DashboardViewModel();
        InitializeComponent();
    }

    private void OnInitialized(object sender, System.EventArgs e)
    {
        //((DashboardViewModel) DataContext).Initialize();
    }
    
    private void InitializeComponent()
    {
       
        AvaloniaXamlLoader.Load(this);
    }
}