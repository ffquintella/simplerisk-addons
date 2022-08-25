using System;
using System.Collections.Generic;
using System.Linq;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Model;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class DeviceViewModel: ViewModelBase
{

    private bool _initialized = false;
    private List<Client> _clients;
    public List<Client> Clients
    {
        get => _clients;
        set => this.RaiseAndSetIfChanged(ref _clients, value);
    }

    private IClientService _clientService;
    private IStringLocalizer _localizer;
    
    private string StrName { get;  }
    private string StrComputer { get;  }
    private string StrLoggedAccount { get;  }
    private string StrActions { get;  }
    
    public DeviceViewModel(
        ILocalizationService localizationService,
        IClientService clientService)
    {
        _localizer = localizationService.GetLocalizer();
        _clientService = clientService;

        StrName = _localizer["Name"];
        StrComputer = _localizer["Computer"];
        StrLoggedAccount= _localizer["LoggedAccount"];
        StrActions= _localizer["Actions"];
    }

    public void Initialize()
    {
        if (!_initialized)
        {
            Clients = _clientService.GetAll();
            _initialized = true;
        }
    }
    
}