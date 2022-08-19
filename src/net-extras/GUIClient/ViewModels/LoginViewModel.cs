using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using GUIClient.Models;
using GUIClient.Services;
using MessageBox.Avalonia.DTO;
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
    
    public AuthenticationMethod? AuthenticationMethod { get; set; }

    public bool ProgressBarVisibility
    {
        get => _progressBarVisibility;
        set
        {
            this.RaiseAndSetIfChanged(ref _progressBarVisibility, value);
        }
    }

    public int ProgressBarValue
    {
        get => _progressBarValue;
        set
        {
            this.RaiseAndSetIfChanged(ref _progressBarValue, value);
        }
    }

    public int ProgressBarMaxValue { get; set; } = 100;

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
    private bool _progressBarVisibility = false;
    private int _progressBarValue = 0;

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

    public async void OnLoginClickCommand(Window? loginWindow)
    {
        ProgressBarValue = 0;
        if (AuthenticationMethod == null)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = _localizer["Warning"],
                    ContentMessage = _localizer["SelectAuthenticationMSG"]  ,
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                });
                        
            messageBoxStandardWindow.Show(); 
        }
        else
        {
            if ( AuthenticationMethod.Type == "SAML")
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                    {
                        ContentTitle = _localizer["Warning"],
                        ContentMessage = _localizer["NotImplementedMSG"]  ,
                        Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                    });
                            
                messageBoxStandardWindow.Show(); 
            }
            else
            {
                ProgressBarVisibility = true;

                var task = Task.Run(() => _authenticationService.DoServerAuthentication(Username, Password));

                int i = 1;
                while(!task.IsCompleted && i < 100)
                {
                    ProgressBarValue = i;
                    i++;
                    this.RaisePropertyChanged("Progress");
                    await Task.Delay(TimeSpan.FromMilliseconds(20));
                }

                ProgressBarValue = 100;
                ProgressBarVisibility = false;
                
                var result =  task.Result;
                
                //var result = _authenticationService.DoServerAuthentication(Username, Password);

                if (result != 0)
                {
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                        {
                            ContentTitle = _localizer["Warning"],
                            ContentMessage = _localizer["LoginError"]  ,
                            Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                        });
                            
                    messageBoxStandardWindow.Show(); 
                }
                else
                {
                    if (loginWindow != null)
                    {
                        loginWindow.Close();
                    } 
                }
            }
            
        }
    }
    
    public void OnExitClickCommand()
    {
        Environment.Exit(0);
    }
    
}