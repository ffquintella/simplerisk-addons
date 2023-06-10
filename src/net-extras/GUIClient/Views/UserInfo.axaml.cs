using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientServices.ViewModels;

namespace ClientServices.Views;

public partial class UserInfo : Window
{
    public UserInfo()
    {

        //DataContext = new UserInfoViewModel();
        
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