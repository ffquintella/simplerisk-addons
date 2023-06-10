using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientServices.ViewModels;

namespace ClientServices.Views;

public partial class RiskView : UserControl
{
    public RiskView()
    {
        DataContext = new RiskViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}