using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUIClient.Views;

public partial class AssessmentView : UserControl
{
    public AssessmentView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}