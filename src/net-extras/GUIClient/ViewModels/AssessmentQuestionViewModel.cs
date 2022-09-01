using System.Collections.Generic;
using Avalonia.Controls;
using DAL.Entities;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class AssessmentQuestionViewModel: ViewModelBase
{
    private string StrQuestion { get; }
    private string StrAnswers { get; }
    
    private string StrAnswer { get; }
    
    private string StrRisk { get; }
    private string StrSubject { get; }
    private string TxtQuestion { get; set; } = "";
    
    private Window DisplayWindow { get; }
    
    private string TxtAnswer { get; set; } = "";
    private string TxtRisk { get; set; } = "";
    private string TxtSubject { get; set; } = "";

    private List<AssessmentAnswer> _answers = new List<AssessmentAnswer>();
    private List<AssessmentAnswer> Answers
    {
        get => _answers;
        set => this.RaiseAndSetIfChanged(ref _answers, value);
        
    }

    public AssessmentQuestionViewModel(Window displayWindow)
    {
        DisplayWindow = displayWindow;
        StrQuestion = Localizer["Question"];
        StrAnswers = Localizer["Answers"];
        StrAnswer = Localizer["Answer"];
        StrRisk = Localizer["Risk"];
        StrSubject = Localizer["Subject"];
    }
}