using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using DAL.Entities;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Model.Exceptions;
using ReactiveUI;
using Splat;

namespace GUIClient.ViewModels;

public class AssessmentViewModel: ViewModelBase
{
    
    public ListBox ListBox { get; set; }
    public string StrAssessments { get; }
    private bool _isInitialized = false;
    
    private IAssessmentsService _assessmentsService;
    
    private ObservableCollection<Assessment> _assessments;
    
    public ObservableCollection<Assessment> Assessments
    {
        get => _assessments;
        set => this.RaiseAndSetIfChanged(ref _assessments, value);
    }
    
    public AssessmentViewModel()
    {
        
        _assessments = new ObservableCollection<Assessment>();
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
            Assessments = new ObservableCollection<Assessment>(assessments);
            ListBox.Items = Assessments;

        }
    }
    
    
}