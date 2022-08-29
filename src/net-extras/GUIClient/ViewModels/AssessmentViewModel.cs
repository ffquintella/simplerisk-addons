using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Media.TextFormatting.Unicode;
using DAL.Entities;
using GUIClient.Services;
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
                        break;
                }                
            }
            this.RaiseAndSetIfChanged(ref _selectedAssessment, value);
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
    public AssessmentViewModel() : base()
    {
        
        _assessments = new ObservableCollection<Assessment>();
        _assessmentsService = GetService<IAssessmentsService>();
        _assessmentQuestions = new List<AssessmentQuestion>();
        
        StrAssessments = Localizer["Assessments"];
        _strAnswers = Localizer["Answers"];
        StrQuestions = Localizer["Questions"];
        
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };


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