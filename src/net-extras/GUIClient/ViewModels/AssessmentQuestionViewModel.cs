using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
using System.Text;
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

    private int _txtRisk = 0;
    
    private int TxtRisk { 
        get => _txtRisk;
        set
        {
            if (value > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "0-10.");
            }
            this.RaiseAndSetIfChanged(ref _txtRisk, value);  
        }
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
    public ReactiveCommand<Unit, Unit> BtSaveAnswerClicked { get; }
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
        BtSaveAnswerClicked = ReactiveCommand.Create(ExecuteSaveAnswer);
        

    }
    private void ExecuteSaveAnswer()
    {
        var answer = new AssessmentAnswer
        {
            Answer = TxtAnswer,
            AssessmentId = 0,
            QuestionId = -1,
            RiskScore = TxtRisk,
            RiskSubject = Encoding.UTF8.GetBytes(TxtSubject)
        };
        
        Answers.Add(answer);
        
        
        ExecuteCancelAddAnswer();
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
        TxtRisk = 0;
        TxtSubject = "";

        InputEnabled = enable;
    }
}