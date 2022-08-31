using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media.TextFormatting.Unicode;
using DAL.Entities;
using GUIClient.Services;
using MessageBox.Avalonia.DTO;
using Microsoft.Extensions.Localization;
using Model.Exceptions;
using ReactiveUI;
using Splat;

namespace GUIClient.ViewModels;

public class AssessmentViewModel: ViewModelBase
{
    
    public string StrAssessments { get; }

    private string _strAnswers;
    public string StrAnswers => _strAnswers;
    
    public string StrQuestions { get; }

    private bool _isInitialized = false;
    
    private IAssessmentsService _assessmentsService;
    
    private ObservableCollection<Assessment> _assessments;
    
    public ObservableCollection<Assessment> Assessments
    {
        get => _assessments;
        set => this.RaiseAndSetIfChanged(ref _assessments, value);
    }
    private Assessment? _selectedAssessment;

    public Assessment? SelectedAssessment
    {
        get
        {
            return _selectedAssessment;
        }
        set
        {
            if (value != null)
            {
                switch(SelectedTabIndex)
                {
                    case 0:
                        UpdateSelectedQuestions( value.Id);
                        UpdateSelectedAnswers(value.Id);
                        break;
                }                
            }
            this.RaiseAndSetIfChanged(ref _selectedAssessment, value);
        }
    }

    private AssessmentQuestion? _selectedAssessmentQuestion;
    
    public AssessmentQuestion? SelectedAssessmentQuestion
    {
        get
        {
            return _selectedAssessmentQuestion;
        }
        set
        {
            if (value != null)
            {

                UpdateAssessmentQuestionAnswers( value.Id);
            
            }
            this.RaiseAndSetIfChanged(ref _selectedAssessmentQuestion, value);
        }
    }
    
    public int SelectedTabIndex { get; set; } = 0;

    private List<AssessmentQuestion> _assessmentQuestions;

    public List<AssessmentQuestion> AssessmentQuestions
    {
        get => _assessmentQuestions;
        set => this.RaiseAndSetIfChanged(ref _assessmentQuestions, value);
    }

    private void UpdateSelectedQuestions(int assessmentId)
    {
        var questions = _assessmentsService.GetAssessmentQuestions(assessmentId);
        if (questions == null) return;
        AssessmentQuestions = questions;
    }
    
    private List<AssessmentAnswer> _assessmentAnswers;

    public List<AssessmentAnswer> AssessmentAnswers
    {
        get => _assessmentAnswers;
        set => this.RaiseAndSetIfChanged(ref _assessmentAnswers, value);
    }
    
    private void UpdateSelectedAnswers(int assessmentId)
    {
        var answers = _assessmentsService.GetAssessmentAnswers(assessmentId);
        if (answers == null) return;
        AssessmentAnswers = answers;
    }
    
    private List<AssessmentAnswer> _assessmentQuestionAnswers;
    public List<AssessmentAnswer> AssessmentQuestionAnswers
    {
        get => _assessmentQuestionAnswers;
        set => this.RaiseAndSetIfChanged(ref _assessmentQuestionAnswers, value);
    }

    private void UpdateAssessmentQuestionAnswers(int assessmentQuestionId)
    {
        AssessmentQuestionAnswers = AssessmentAnswers.Where(answ => answ.QuestionId == assessmentQuestionId).ToList();
    }

    private bool _assessmentAddBarVisible = false;
    public bool AssessmentAddBarVisible
    {
        get => _assessmentAddBarVisible;
        set => this.RaiseAndSetIfChanged(ref _assessmentAddBarVisible, value);
    }
    public string StrAnswer { get; }

    private string _txtAssessmentAddValue = "";
    public string TxtAssessmentAddValue
    {
        get => _txtAssessmentAddValue; 
        set => this.RaiseAndSetIfChanged(ref _txtAssessmentAddValue, value); 
    } 
    public ReactiveCommand<Unit, Unit> BtAddAssessmentClicked { get; }
    public ReactiveCommand<Unit, Unit> BtCancelAddAssessmentClicked { get; }
    public ReactiveCommand<Unit, Unit> BtSaveAssessmentClicked { get; }
    public AssessmentViewModel() : base()
    {
        
        _assessments = new ObservableCollection<Assessment>();
        _assessmentsService = GetService<IAssessmentsService>();
        _assessmentQuestions = new List<AssessmentQuestion>();
        
        StrAssessments = Localizer["Assessments"];
        _strAnswers = Localizer["Answers"];
        StrQuestions = Localizer["Questions"];
        StrAnswer = Localizer["Answer"];
        
        BtAddAssessmentClicked = ReactiveCommand.Create(ExecuteAddAssessment);
        BtCancelAddAssessmentClicked = ReactiveCommand.Create(ExecuteCancelAddAssessment);
        BtSaveAssessmentClicked = ReactiveCommand.Create(ExecuteSaveAssessment);
        
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
    }
    
    private void ExecuteAddAssessment()
    {
        TxtAssessmentAddValue = "";
        AssessmentAddBarVisible = true;
    }
    
    private void ExecuteCancelAddAssessment()
    {
        TxtAssessmentAddValue = "";
        AssessmentAddBarVisible = false;
    }
    
    private async void ExecuteSaveAssessment()
    {
        if(TxtAssessmentAddValue.Trim() == "")
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = Localizer["Warning"],
                    ContentMessage = Localizer["AssessmentNameInvalidMSG"],
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                });
                            
            await messageBoxStandardWindow.Show(); 
            return;
        }
        
        //TxtAssessmentAddValue = "";
        //AssessmentAddBarVisible = false;
    }
    
    private void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            var assessments = _assessmentsService.GetAssessments();
            if (assessments == null)
            {
                Logger.Error("Assessments are null");
                throw new RestComunicationException("Error getting assessments");
            }
            Assessments = new ObservableCollection<Assessment>(assessments);
            
        }
    }
    
    
}