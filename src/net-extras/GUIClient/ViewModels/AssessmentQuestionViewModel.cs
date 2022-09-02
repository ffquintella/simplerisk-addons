using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using System.Text;
using Avalonia.Controls;
using DAL.Entities;
using DynamicData;
using GUIClient.Tools;
using MessageBox.Avalonia.DTO;
using ReactiveUI;
using Tools;

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

    private float _txtRisk = 0;
    
    private float TxtRisk { 
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
    
    private bool _btSaveEnabled = false;

    private bool BtSaveEnabled
    {
        get => _btSaveEnabled;
        set=> this.RaiseAndSetIfChanged(ref _btSaveEnabled, value);
    }

    private bool _gridEnabled = true;

    private bool GridEnabled
    {
        get => _gridEnabled;
        set=> this.RaiseAndSetIfChanged(ref _gridEnabled, value);
    }
    
    private ObservableCollection<AssessmentAnswer> _answers = new ObservableCollection<AssessmentAnswer>();
    private ObservableCollection<AssessmentAnswer> Answers
    {
        get => _answers;
        set => this.RaiseAndSetIfChanged(ref _answers, value);
    }

    private AssessmentAnswer? _selectedAnswer = null;
    private AssessmentAnswer? SelectedAnswer
    {
        get => _selectedAnswer;
        set
        {
            if (value is not null)
            {
                TxtAnswer = value.Answer;
                TxtRisk = value.RiskScore;
                TxtSubject = System.Text.Encoding.UTF8.GetString(value.RiskSubject);
                BtSaveEnabled = true;
                InputEnabled = true;
            }

            this.RaiseAndSetIfChanged(ref _selectedAnswer, value);
        }
        
    }

    private Assessment SelectedAssessment { get; }
    public ReactiveCommand<Unit, Unit> BtAddAnswerClicked { get; }
    public ReactiveCommand<Unit, Unit> BtCancelAddAnswerClicked { get; }
    
    public ReactiveCommand<Unit, Unit> BtDeleteAnswerClicked { get; }
    public ReactiveCommand<bool, Unit> BtSaveAnswerClicked { get; }
    public AssessmentQuestionViewModel(Window displayWindow, Assessment selectedAssessment)
    {
        DisplayWindow = displayWindow;
        SelectedAssessment = selectedAssessment;
        StrQuestion = Localizer["Question"];
        StrAnswers = Localizer["Answers"];
        StrAnswer = Localizer["Answer"];
        StrRisk = Localizer["Risk"];
        StrSubject = Localizer["Subject"];
        
        BtAddAnswerClicked = ReactiveCommand.Create(ExecuteAddAnswer);
        BtCancelAddAnswerClicked = ReactiveCommand.Create(ExecuteCancelAddAnswer);
        BtSaveAnswerClicked = ReactiveCommand.Create<bool>(ExecuteSaveAnswer);
        BtDeleteAnswerClicked = ReactiveCommand.Create(ExecuteDeleteAnswer);

    }
    
    private void ExecuteDeleteAnswer()
    {
        if (SelectedAnswer is null)
        {
            Logger.Error("Button delete answer clicked but no answer selected.");
            return;
        }
        Answers.Remove(SelectedAnswer);
        SelectedAnswer = null;
        GridEnabled = true;
        CleanAndUpdateButtonStatus(false);
    }
    private void ExecuteSaveAnswer(bool update = false)
    {
        if (update)
        {
            
            if ( TxtAnswer != SelectedAnswer.Answer && Answers.Any(ans => ans.Answer == TxtAnswer))
            {
                var msgBox1 = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                    {
                        ContentTitle = Localizer["Warning"],
                        ContentMessage = Localizer["AnswerAlreadyExistsMSG"],
                        Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    });
                            
                msgBox1.Show(); 
                return;  
            }

            var editAnwser = SelectedAnswer.DeepCopy();
            
            editAnwser.Answer = TxtAnswer;
            editAnwser.RiskScore = TxtRisk;
            editAnwser.RiskSubject = Encoding.UTF8.GetBytes(TxtSubject);

            var location = Answers.IndexOf(SelectedAnswer);
            Answers.RemoveAt(location);
            SelectedAnswer = null;
            Answers.Insert(location, editAnwser);
            //Answers.Add(editAnwser);
        }
        else
        {
            if (Answers.Any(ans => ans.Answer == TxtAnswer))
            {
                var msgBox1 = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                    {
                        ContentTitle = Localizer["Warning"],
                        ContentMessage = Localizer["AnswerAlreadyExistsMSG"],
                        Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    });
                            
                msgBox1.Show(); 
                return;  
            }
            
            var answer = new AssessmentAnswer
            {
                Answer = TxtAnswer,
                AssessmentId = SelectedAssessment.Id,
                QuestionId = -1,
                RiskScore = TxtRisk,
                RiskSubject = Encoding.UTF8.GetBytes(TxtSubject)
            };
        
            Answers.Add(answer);
        }

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
        SelectedAnswer = null;
        GridEnabled = true;
        CleanAndUpdateButtonStatus(false);
    }

    private void CleanAndUpdateButtonStatus(bool enable)
    {
        TxtAnswer = "";
        TxtRisk = 0;
        TxtSubject = "";
        BtSaveEnabled = enable;
        InputEnabled = enable;
    }
}