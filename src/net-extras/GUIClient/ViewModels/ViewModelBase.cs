using GUIClient.Services;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using Splat;

namespace GUIClient.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected IStringLocalizer _localizer;
        private IAuthenticationService _authenticationService;
        public IStringLocalizer Localizer
        {
            get => _localizer;
            set => _localizer = value;
        }
        
        public IAuthenticationService AuthenticationService
        {
            get => _authenticationService;
            set => _authenticationService = value;
        }
        public ViewModelBase()
        {
            var localizationService = GetService<ILocalizationService>();
            AuthenticationService = GetService<IAuthenticationService>();
            Localizer = localizationService.GetLocalizer();
        }
        
        protected static T GetService<T>() => Locator.Current.GetService<T>();
    }
    
    
}