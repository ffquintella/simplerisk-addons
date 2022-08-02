using System.Globalization;
using System.Resources;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Client.Resources;

namespace Client;

public partial class Settings : Window
{
    
    
    public Settings()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        // Create a resource manager.
        ResourceManager rm = new ResourceManager("Client.Resources.Localize.pt-BR", assembly: typeof(Program).Assembly);

        this.DataContext = new SettingsViewModel();
        // Obtain the es-MX culture.
        //CultureInfo ci = new CultureInfo("pt-BR");
        //var strSystem  = rm.GetString("System", ci);
        //strSystem = Localize_en_US.System;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}