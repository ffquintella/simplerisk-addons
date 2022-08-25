using System;
using System.Collections.Generic;
using System.Linq;
using GUIClient.Services;
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
    
    public DeviceViewModel(IClientService clientService)
    {
        _clientService = clientService;
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