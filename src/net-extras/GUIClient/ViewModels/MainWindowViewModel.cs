using System;
using GUIClient.Configuration;
using GUIClient.Models;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using ReactiveUI;

namespace GUIClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ILocalizationService _localizationService;
        public IStringLocalizer _localizer;
        
        private bool _viewDashboardIsVisible = true;
        private bool _viewDeviceIsVisible = false;

        public string StrApplicationMN { get; }
        public string StrExitMN { get; }

        public bool ViewDashboardIsVisible
        {
            get => _viewDashboardIsVisible;
            set => this.RaiseAndSetIfChanged(ref _viewDashboardIsVisible, value);
        }
        
        public bool ViewDeviceIsVisible
        {
            get => _viewDeviceIsVisible;
            set {
                this.RaiseAndSetIfChanged(ref _viewDeviceIsVisible, value);
                
            }
    }

        public MainWindowViewModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _localizer = _localizationService.GetLocalizer();

            StrApplicationMN = _localizer["ApplicationMN"];
            StrExitMN = _localizer["ExitMN"];
        }

        public void OnMenuExitCommand()
        {
            Environment.Exit(0);
        }

        public void NavigateTo(AvaliableViews view)
        {
            HideAllViews();
            switch (view)
            {
                case AvaliableViews.Dashboard:
                    ViewDashboardIsVisible = true;
                    break;
                case AvaliableViews.Devices:
                    ViewDeviceIsVisible = true;
                    break;
            }
        }

        private void HideAllViews()
        {
            ViewDashboardIsVisible = false;
            ViewDeviceIsVisible = false;
        }
        
    }
}