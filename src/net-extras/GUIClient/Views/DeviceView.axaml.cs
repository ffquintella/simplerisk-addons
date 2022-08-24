using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GUIClient.Views;

public partial class DeviceView : UserControl
{
    
    public DeviceView()
    {
        DataContext = new DeviceViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}