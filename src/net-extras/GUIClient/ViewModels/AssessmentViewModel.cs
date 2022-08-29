using System.Collections.Generic;
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
    private bool _isInitialized = false;
    
    private IAssessmentsService _assessmentsService;
    
    private List<Assessment> _assessments;
    
    public List<Assessment> Assessments
    {
        get => _assessments;
        set => this.RaiseAndSetIfChanged(ref _assessments, value);
    }
    
    public AssessmentViewModel()
    {
        
        _assessments = new List<Assessment>();
        _assessmentsService = GetService<IAssessmentsService>();
        
        StrAssessments = Localizer["Assessments"];
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
    }


    private async void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            var assessments = await _assessmentsService.GetAssessments();
            if (assessments == null)
            {
                Logger.Error("Assessments are null");
                throw new RestComunicationException("Error getting assessments");
            }
            Assessments = assessments;
            
        }
    }
    
    
}