using Avalonia.Controls;

namespace GUIClient.ViewModels;

public class AssessmentQuestionViewModel: ViewModelBase
{
    private string StrQuestion { get; }
    private string StrAnswers { get; }
    private string TxtQuestion { get; set; } = "";
    
    private Window DisplayWindow { get; }

    public AssessmentQuestionViewModel(Window displayWindow)
    {
        DisplayWindow = displayWindow;
        StrQuestion = Localizer["Question"];
        StrAnswers = Localizer["Answers"];
    }
}