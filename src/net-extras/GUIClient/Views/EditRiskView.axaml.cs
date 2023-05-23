using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUIClient.Views;

public partial class EditRiskView : Window
{
    public EditRiskView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}