using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ClientServices.Views;

public partial class RisksPanelView : UserControl
{
    public RisksPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}