using System;
using System.Collections.Generic;

using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using DAL.Entities;
using DynamicData.Tests;
using GUIClient.Models;
using GUIClient.Services;
using MessageBox.Avalonia.DTO;
using Model.DTO;
using Model.Exceptions;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using MessageBox.Avalonia.Enums;

namespace GUIClient.ViewModels;

public class EditRiskViewModel: ViewModelBase
{
    public string StrRisk { get; }
    public string StrOperation { get; }
    public string StrOperationType { get; }
    public string StrRiskType { get; }
    public string StrSubject { get; }
    public string StrSource { get; }
    public string StrCategory { get; }
    public string StrNotes { get; }
    public string StrOwner { get; }
    public string StrManager { get; }
    public bool ShowEditFields { get; }
    public string StrSave { get; }
    public string StrCancel { get; }
    
    public List<Source>? RiskSources { get; }
    
    public List<UserListing>? UserListings { get; }
    
    private Source? _selectedRiskSource;
    public Source? SelectedRiskSource
    {
        get
        {
            return _selectedRiskSource;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRiskSource, value);
        }
    }
    
    
    public List<Category>? Categories { get; }

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get
        {
            return _selectedCategory;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCategory, value);
        }
    }

    private bool _isCtrlNumVisible = false;
    
    
    public bool IsCtrlNumVisible
    {
        get
        {
            return _isCtrlNumVisible;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _isCtrlNumVisible, value);
        }
    }

    private UserListing? _selectedOwner;
    public UserListing? SelectedOwner
    {
        get
        {
            return _selectedOwner;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedOwner, value);
        }
    }
    
    private UserListing? _selectedManager;
    public UserListing? SelectedManager
    {
        get
        {
            return _selectedManager;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedManager, value);
        }
    }
    
    private string? _notes;
    public String? Notes
    {
        get
        {
            return _notes;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _notes, value);
        }
    }
    
    private List<RiskCatalog> RiskTypes { get; }

    private List<RiskCatalog> _selectedRiskTypes;

    private List<RiskCatalog> SelectedRiskTypes
    {
        get
        {
            return _selectedRiskTypes;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRiskTypes, value);
        }
    }
    

    private OperationType _operationType;
    private IRisksService _risksService;
    private IAuthenticationService _authenticationService;
    private IUsersService _usersService;
    
    public ReactiveCommand<Unit, Unit> BtSaveClicked { get; }
    public ReactiveCommand<Unit, Unit> BtCancelClicked { get; }
    
    public EditRiskViewModel(OperationType operation, Risk? risk = null)
    {
        if (operation == OperationType.Edit && risk == null)
        {
            throw new InvalidParameterException("risk", "Risk cannot be null");
        }
        
        if (operation == OperationType.Edit)
        {
            IsCtrlNumVisible = true;
        }

        
        _operationType = operation;
        StrRisk = Localizer["Risk"];
        StrOperation = Localizer["Operation"] + ": ";
        StrSubject = Localizer["Subject"] + ": ";
        StrSource = Localizer["Source"] + ": ";
        StrCategory = Localizer["Category"]+ ": ";
        StrRiskType = Localizer["RiskType"] ;
        StrOwner = Localizer["Owner"] + ":";
        StrManager = Localizer["Manager"] + ":";
        StrNotes = Localizer["Notes"] + ": ";
        StrSave= Localizer["Save"] ;
        StrCancel= Localizer["Cancel"] ;
        
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
        _authenticationService = GetService<IAuthenticationService>();
        _usersService = GetService<IUsersService>();

        RiskSources = _risksService.GetRiskSources();
        Categories = _risksService.GetRiskCategories();
        RiskTypes = _risksService.GetRiskTypes();
        UserListings = _usersService.ListUsers();

        var sowner = UserListings.FirstOrDefault(ul => ul.Id == _authenticationService!.AuthenticatedUserInfo!.UserId);

        if (sowner != null) SelectedOwner = sowner;

        if (RiskSources == null) throw new Exception("Unable to load risk list");
        if (Categories == null) throw new Exception("Unable to load category list");
        if (RiskTypes == null) throw new Exception("Unable to load risk types");
        if (UserListings == null) throw new Exception("Unable to load user listing");
        
        BtSaveClicked = ReactiveCommand.Create(ExecuteSave);
        BtCancelClicked = ReactiveCommand.Create(ExecuteCancel);
        
        
        this.ValidationRule(
            viewModel => viewModel.RiskSubject, 
            name => !string.IsNullOrWhiteSpace(RiskSubject),
            Localizer["RiskMustHaveASubjectMSG"]);
        
        IObservable<bool> subjectUnique =
            this.WhenAnyValue(
                x => x.RiskSubject,
                (subject) =>
                {
                    return !_risksService.RiskSubjectExists(subject);
                });
        
        this.ValidationRule(
            vm => vm.RiskSubject,
            subjectUnique,
            "Subject already exists.");
        
        
        this.IsValid()
            .Subscribe(x =>
            {
                SaveEnabled = x;
            });
    }
    

    
    private async void ExecuteSave()
    {

        if(SelectedOwner != null)
            Risk.Owner = SelectedOwner.Id;
        if (SelectedManager != null)
            Risk.Manager = SelectedManager.Id;

        if (_operationType == OperationType.Create)
        {
            Risk.Status = "new";
            Risk.SubmissionDate = DateTime.Now;
        }

        Risk.LastUpdate = DateTime.Now;

        if (SelectedCategory != null)
            Risk.Category = SelectedCategory.Value;
        if (SelectedRiskSource != null)
            Risk.Source = SelectedRiskSource.Value;
        if (Notes != null)
            Risk.Notes = Notes;

        foreach (var srt in SelectedRiskTypes)
        {
            Risk.RiskCatalogMapping += srt.Id + ";";
        }

        var resultingRisk = _risksService.CreateRisk(Risk);

        if (resultingRisk == null)
        {
            var msgError = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
            {
                ContentTitle = Localizer["Error"],
                ContentMessage = Localizer["ErrorCreatingRiskMSG"],
                Icon = Icon.Error,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            });
    
            await msgError.Show();
        }




    }
    
    private void ExecuteCancel()
    {

    }

    private bool _saveEnabled = false;
    
    public bool SaveEnabled
    {
        get
        {
            return _saveEnabled;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _saveEnabled, value);
        }
    }
    
    private string _riskSuject = "";
    
    public string RiskSubject
    {
        get
        {
            return _riskSuject;
        }
        set
        {
            Risk.Subject = _riskSuject;
            this.RaiseAndSetIfChanged(ref _riskSuject, value);
        }
    }
    
    private Risk _risk = new Risk();
    //public Risk Risk { get; set; }
    
    public Risk Risk
    {
        get
        {
            return _risk;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _risk, value);
        }
    }
}