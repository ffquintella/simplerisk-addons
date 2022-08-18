using System;
using System.Collections.Generic;
using System.Dynamic;using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Model.Authentication;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private ILocalizationService _localizationService;
    private IAuthenticationService _authenticationService;
    public IStringLocalizer _localizer;

    public string StrNotAccepted { get; }
    public string StrLogin { get; }
    public string StrUsername { get; }
    public string StrPassword { get; }
    public string StrExit { get; }
    public string StrEnvironment { get; }

    public List<AuthenticationMethod> AuthenticationMethods => _authenticationService.GetAuthenticationMethods();

    public LoginViewModel(ILocalizationService localizationService, IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        _localizationService = localizationService;
        _localizer = _localizationService.GetLocalizer();
        StrNotAccepted = _localizer["NotAccepted"];
        StrLogin = _localizer["Login"];
        StrPassword = _localizer["Password"];
        StrUsername = _localizer["Username"];
        StrExit = _localizer["Exit"];
        StrEnvironment = _localizer["Environment"];
    }

    private bool _isAccepted;

    public bool IsAccepted
    {
        get
        {
            return _isAccepted;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _isAccepted, value);
        }
    }


    public string? Username { get; set;}
    public string? Password { get; set; }

    public void OnLoginClickCommand()
    {
        // do something
    }
    
    public void OnExitClickCommand()
    {
        Environment.Exit(0);
    }
    
}