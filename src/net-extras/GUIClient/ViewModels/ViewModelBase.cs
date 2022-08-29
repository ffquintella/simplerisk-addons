using GUIClient.Exceptions;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using Splat;
using ILogger = Serilog.ILogger;

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
        
        private ILogger _logger;
        public ILogger Logger
        {
            get => _logger;
            set => _logger = value;
        }
        
        public IAuthenticationService AuthenticationService
        {
            get => _authenticationService;
            set => _authenticationService = value;
        }
        public ViewModelBase()
        {
            var localizationService = GetService<ILocalizationService>();
            _authenticationService = GetService<IAuthenticationService>();
            _logger = GetService<ILogger>();
            var localizer = localizationService.GetLocalizer();
            if (localizer == null)
            {
                Logger.Error("Error getting localizer service");
                throw new DIException("Error getting localizer service");
            }

            _localizer = localizer;

        }
        
        protected static T GetService<T>() => Locator.Current.GetService<T>();
    }
    
    
}