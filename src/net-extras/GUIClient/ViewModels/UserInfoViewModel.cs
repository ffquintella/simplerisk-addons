using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Model.Authentication;
using ReactiveUI;
using Splat;

namespace GUIClient.ViewModels;

public class UserInfoViewModel: ViewModelBase
{
    private AuthenticatedUserInfo _userInfo;
    
    private string StrUserName { get;  }
    
    private string StrUserAccount { get; }
    private string StrRole { get; }
    public UserInfoViewModel(AuthenticatedUserInfo userInfo)
    {
        _userInfo = userInfo;
        StrUserName = Localizer["Username"];
        StrUserAccount = Localizer["Account"];
        StrRole = Localizer["Role"];
    }

    public AuthenticatedUserInfo UserInfo
    {
        get => _userInfo;
        set => this.RaiseAndSetIfChanged(ref _userInfo, value);
    }
    
    
}