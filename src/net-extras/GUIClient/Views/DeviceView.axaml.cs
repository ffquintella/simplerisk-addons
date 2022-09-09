using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.Services;
using GUIClient.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Splat;

namespace GUIClient.Views;

public partial class DeviceView : UserControl
{
    
    public DeviceView()
    {
        //DataContext = new DeviceViewModel(GetService<IClientService>());
        //((DeviceViewModel)DataContext).Initialize();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private static T GetService<T>() => Locator.Current.GetService<T>();
}