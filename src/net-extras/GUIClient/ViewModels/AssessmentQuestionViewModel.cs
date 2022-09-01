using System.Collections.Generic;
using System.Reactive;
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


    private string _txtAnser = "";
    private string TxtAnswer
    {
        get => _txtAnser; 
        set => this.RaiseAndSetIfChanged(ref _txtAnser, value); 
    }

    private string _txtRisk = "";
    private string TxtRisk { 
        get => _txtRisk; 
        set => this.RaiseAndSetIfChanged(ref _txtRisk, value);  
    }

    private string _txtSubject = "";
    private string TxtSubject
    {
        get => _txtSubject; 
        set => this.RaiseAndSetIfChanged(ref _txtSubject, value);
        
    }

    private bool _inputEnabled = false;

    private bool InputEnabled
    {
        get => _inputEnabled;
        set=> this.RaiseAndSetIfChanged(ref _inputEnabled, value);
    }

    private bool _gridEnabled = true;

    private bool GridEnabled
    {
        get => _gridEnabled;
        set=> this.RaiseAndSetIfChanged(ref _gridEnabled, value);
    }
    
    private List<AssessmentAnswer> _answers = new List<AssessmentAnswer>();
    private List<AssessmentAnswer> Answers
    {
        get => _answers;
        set => this.RaiseAndSetIfChanged(ref _answers, value);
        
    }

    private AssessmentAnswer _selectedAnswer = null;
    private AssessmentAnswer SelectedAnswer
    {
        get => _selectedAnswer;
        set => this.RaiseAndSetIfChanged(ref _selectedAnswer, value);
        
    }

    public ReactiveCommand<Unit, Unit> BtAddAnswerClicked { get; }
    public ReactiveCommand<Unit, Unit> BtCancelAddAnswerClicked { get; }
    
    public AssessmentQuestionViewModel(Window displayWindow)
    {
        DisplayWindow = displayWindow;
        StrQuestion = Localizer["Question"];
        StrAnswers = Localizer["Answers"];
        StrAnswer = Localizer["Answer"];
        StrRisk = Localizer["Risk"];
        StrSubject = Localizer["Subject"];
        
        BtAddAnswerClicked = ReactiveCommand.Create(ExecuteAddAnswer);
        BtCancelAddAnswerClicked = ReactiveCommand.Create(ExecuteCancelAddAnswer);
    }
    
    private void ExecuteAddAnswer()
    {
        SelectedAnswer = null;
        GridEnabled = false;
        CleanAndUpdateButtonStatus(true);
    }
    
    private void ExecuteCancelAddAnswer()
    {
        GridEnabled = true;
        CleanAndUpdateButtonStatus(false);
    }

    private void CleanAndUpdateButtonStatus(bool enable)
    {
        TxtAnswer = "";
        TxtRisk = "";
        TxtSubject = "";

        InputEnabled = enable;
    }
}