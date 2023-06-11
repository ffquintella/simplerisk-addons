using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using ClientServices.Interfaces;
using DAL.Entities;
using GUIClient.Models;
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
        get => _selectedRiskSource;
        set => this.RaiseAndSetIfChanged(ref _selectedRiskSource, value);
    }
    
    
    public List<Category>? Categories { get; }

    private Category? _selectedCategory;
    public Category? SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    private bool _isCtrlNumVisible = false;
    
    
    public bool IsCtrlNumVisible
    {
        get => _isCtrlNumVisible;
        set => this.RaiseAndSetIfChanged(ref _isCtrlNumVisible, value);
    }

    private UserListing? _selectedOwner;
    public UserListing? SelectedOwner
    {
        get => _selectedOwner;
        set => this.RaiseAndSetIfChanged(ref _selectedOwner, value);
    }
    
    private UserListing? _selectedManager;
    public UserListing? SelectedManager
    {
        get => _selectedManager;
        set => this.RaiseAndSetIfChanged(ref _selectedManager, value);
    }
    
    private string? _notes;
    public string? Notes
    {
        get => _notes;
        set => this.RaiseAndSetIfChanged(ref _notes, value);
    }
    
    private List<RiskCatalog> RiskTypes { get; }

    private List<RiskCatalog> _selectedRiskTypes;

    private List<RiskCatalog> SelectedRiskTypes
    {
        get => _selectedRiskTypes;
        set => this.RaiseAndSetIfChanged(ref _selectedRiskTypes, value);
    }
    

    private readonly OperationType _operationType;
    private readonly IRisksService _risksService;
    private readonly IAuthenticationService _authenticationService;
    private IUsersService _usersService;
    
    public ReactiveCommand<Window, Unit> BtSaveClicked { get; }
    public ReactiveCommand<Window, Unit> BtCancelClicked { get; }
    
    public EditRiskViewModel(OperationType operation, Risk? risk = null)
    {
        if (operation == OperationType.Edit && risk == null)
        {
            throw new InvalidParameterException("risk", "Risk cannot be null");
        }

        _selectedRiskTypes = new List<RiskCatalog>();
        
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

        SelectedRiskTypes = new List<RiskCatalog>();

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
        
        BtSaveClicked = ReactiveCommand.Create<Window>(ExecuteSave);
        BtCancelClicked = ReactiveCommand.Create<Window>(ExecuteCancel);
        
        
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
    

    
    private async void ExecuteSave(Window baseWindow)
    {

        if(SelectedOwner != null)
            Risk.Owner = SelectedOwner.Id;
        if (SelectedManager != null)
            Risk.Manager = SelectedManager.Id;

        if (_operationType == OperationType.Create)
        {
            Risk.Status = "New";
            Risk.SubmissionDate = DateTime.Now;
            if(_authenticationService.AuthenticatedUserInfo!.UserId.HasValue)
                Risk.SubmittedBy = _authenticationService.AuthenticatedUserInfo.UserId.Value;
        }

        Risk.LastUpdate = DateTime.Now;

        if (SelectedCategory != null)
            Risk.Category = SelectedCategory.Value;
        if (SelectedRiskSource != null)
            Risk.Source = SelectedRiskSource.Value;
        if (Notes != null)
            Risk.Notes = Notes;

        Risk.Assessment = "";
        Risk.RiskCatalogMapping = "";
        Risk.ThreatCatalogMapping = "";
        Risk.ReferenceId = "";


        foreach (var srt in SelectedRiskTypes)
        {
            Risk.RiskCatalogMapping += srt.Id + ",";
        }

        Risk.RiskCatalogMapping = Risk.RiskCatalogMapping.TrimEnd(',');

        try
        {
            var resultingRisk = _risksService.CreateRisk(Risk);

            var msgOk = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = Localizer["Save"],
                    ContentMessage = Localizer["SaveOkMSG"] + " ID: " + resultingRisk!.Id + ".",
                    Icon = Icon.Success,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });

            await msgOk.Show();
            
            baseWindow.Close();
            
        }
        catch (ErrorSavingException ex)
        {

            var errors = "";

            foreach (var error in ex.Result.Errors)
            {
                errors += error + "\n";
            }


            var msgError = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = Localizer["Error"],
                    ContentMessage = Localizer["ErrorCreatingRiskMSG"] + "cd: " + ex.Result.Status + "\nerr: " + errors + "." ,
                    Icon = Icon.Error,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });

            await msgError.Show();
            
        }

    }
    
    private void ExecuteCancel(Window baseWindow)
    {
        baseWindow.Close();
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
            Risk.Subject = value;
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