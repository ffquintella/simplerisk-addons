﻿using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Model.Authentication;
using ReactiveUI;
using Splat;

namespace GUIClient.ViewModels;

public class UserInfoViewModel: ViewModelBase
{
    private AuthenticatedUserInfo _userInfo;
    private IStringLocalizer _localizer; 
    
    private string StrUserName { get;  }
    
    private string StrUserAccount { get; }
    private string StrRole { get; }
    public UserInfoViewModel(AuthenticatedUserInfo userInfo)
    {
        UserInfo = userInfo;
        _localizer = GetService<ILocalizationService>().GetLocalizer();
        StrUserName = _localizer["Username"];
        StrUserAccount = _localizer["Account"];
        StrRole = _localizer["Role"];
    }

    public AuthenticatedUserInfo UserInfo
    {
        get => _userInfo;
        set => this.RaiseAndSetIfChanged(ref _userInfo, value);
    }
    
    private static T GetService<T>() => Locator.Current.GetService<T>();
}