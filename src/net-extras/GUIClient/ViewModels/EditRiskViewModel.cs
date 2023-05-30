using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using DAL.Entities;
using DynamicData.Tests;
using GUIClient.Models;
using GUIClient.Services;
using Model.DTO;
using Model.Exceptions;
using ReactiveUI;

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

        var sowner = UserListings.FirstOrDefault(ul => ul.Id == _authenticationService.AuthenticatedUserInfo.UserId);

        if (sowner != null) SelectedOwner = sowner;

        if (RiskSources == null) throw new Exception("Unable to load risk list");
        if (Categories == null) throw new Exception("Unable to load category list");
        if (RiskTypes == null) throw new Exception("Unable to load risk types");
        if (UserListings == null) throw new Exception("Unable to load user listing");
        
        BtSaveClicked = ReactiveCommand.Create(ExecuteSave);
        BtCancelClicked = ReactiveCommand.Create(ExecuteCancel);
        
    }
    
    private void ExecuteSave()
    {

    }
    
    private void ExecuteCancel()
    {

    }
    
    public Risk Risk { get; set; }
}