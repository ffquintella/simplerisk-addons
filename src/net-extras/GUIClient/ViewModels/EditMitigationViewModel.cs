﻿using DAL.Entities;
using GUIClient.Models;
using Model.Exceptions;

namespace GUIClient.ViewModels;

public class EditMitigationViewModel: ViewModelBase
{
    #region LANGUAGE

    public string StrMitigation { get; }
    public string StrSubmissionDate { get; }
    #endregion

    #region INTERNAL FIELDS

    private readonly OperationType _operationType;
    private Mitigation? _mitigation;

    #endregion

    public EditMitigationViewModel(OperationType operation, Mitigation? mitigation = null)
    {
        _operationType = operation;
        
        if (_operationType == OperationType.Edit && mitigation == null)
        {
            throw new InvalidParameterException("mitigation", "Mitigation cannot be null on edit operations");
        }
        _mitigation = _operationType == OperationType.Create ? new Mitigation() : mitigation;
        
        
        StrMitigation = Localizer["Mitigation"];
        StrSubmissionDate = Localizer["SubmissionDate"] + ":";
    }

    #region PROPERTIES

    

    #endregion
}