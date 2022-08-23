﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.Configuration;
using GUIClient.Services;
using GUIClient.ViewModels;
using Splat;

namespace GUIClient.Views;

public partial class NavigationBar : UserControl
{
    
    public static readonly StyledProperty<Window> ParentWindowProperty =
        AvaloniaProperty.Register<NavigationBar, Window>(nameof(MainWindow));

    public Window ParentWindow
    {
        get { return GetValue(ParentWindowProperty); }
        set { SetValue(ParentWindowProperty, value); }
    }
    public NavigationBar()
    {
        var localizationService = GetService<ILocalizationService>();
        var serverConfiguration = GetService<ServerConfiguration>();
        DataContext = new NavigationBarViewModel(localizationService,serverConfiguration);
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}