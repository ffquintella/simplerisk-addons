using System;
using System.Collections.Generic;
using DAL.Entities;
using GUIClient.Models;
using GUIClient.Services;
using Model.Exceptions;

namespace GUIClient.ViewModels;

public class EditRiskViewModel: ViewModelBase
{
    public string StrRisk { get; }
    public string StrOperation { get; }
    public string StrOperationType { get; }
    public string StrSubject { get; }
    public string StrSource { get; }
    public bool ShowEditFields { get; }
    
    public List<Source>? RiskSources { get; }
    public Source SelectedRiskSource { get; set; }
    
    public List<Category>? Categories { get; }
    public Category SelectedCategory { get; set; }

    private OperationType _operationType;
    private IRisksService _risksService;
    
    public EditRiskViewModel(OperationType operation, Risk? risk = null)
    {
        if (operation == OperationType.Edit && risk == null)
        {
            throw new InvalidParameterException("risk", "Risk cannot be null");
        }

        
        _operationType = operation;
        StrRisk = Localizer["Risk"];
        StrOperation = Localizer["Operation"] + ": ";
        StrSubject = Localizer["Subject"] + ": ";
        StrSource = Localizer["Source"] + ": ";
        
        StrOperationType = _operationType == OperationType.Create ? Localizer["Creation"] : Localizer["Edit"];
        if (_operationType == OperationType.Create)
        {
            Risk = new Risk();
            ShowEditFields = false;
        }
        else
        {
            Risk = risk!;
            ShowEditFields = true;
        }

        _risksService = GetService<IRisksService>();

        RiskSources = _risksService.GetRiskSources();

        if (RiskSources == null) throw new Exception("Unable to load risk list");


    }
    
    public Risk Risk { get; set; }
}