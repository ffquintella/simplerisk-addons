using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using GUIClient.Services;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
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
    
    public ReactiveCommand<int, Unit> BtApproveClicked { get; }
    public ReactiveCommand<int, Unit> BtRejectClicked { get; }
    public ReactiveCommand<int, Unit> BtDeleteClicked { get; }
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
        
        BtApproveClicked = ReactiveCommand.Create<int>(ExecuteAproveOrder);
        BtRejectClicked = ReactiveCommand.Create<int>(ExecuteRejectOrder);
        BtDeleteClicked = ReactiveCommand.Create<int>(ExecuteDeleteOrder);

        
    }

    private void ExecuteAproveOrder(int id)
    {
        var result = _clientService.Approve(id);
        if (result != 0)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = _localizer["Warning"],
                    ContentMessage = _localizer["ClientApproveErrorMSG"]  ,
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                });
                        
            messageBoxStandardWindow.Show(); 
        }
        else
        {
            Clients = _clientService.GetAll();
        }
    }
    
    private void ExecuteRejectOrder(int id)
    {
        var result = _clientService.Reject(id);
        if (result != 0)
        {
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = _localizer["Warning"],
                    ContentMessage = _localizer["ClientRejectErrorMSG"]  ,
                    Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                });
                        
            messageBoxStandardWindow.Show(); 
        }
        else
        {
            Clients = _clientService.GetAll();
        }
    }

    private async void  ExecuteDeleteOrder(int id)
    {
        
        var messageBoxConfirm = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
            {
                ContentTitle = _localizer["Warning"],
                ContentMessage = _localizer["ClientDeleteConfirmationMSG"]  ,
                ButtonDefinitions = ButtonEnum.OkAbort,
                Icon = Icon.Question,
            });
                        
        var confirmation = await messageBoxConfirm.Show();

        if (confirmation == ButtonResult.Ok)
        {
            
            var result = _clientService.Delete(id);
            if (result != 0)
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                    {
                        ContentTitle = _localizer["Warning"],
                        ContentMessage = _localizer["ClientRejectErrorMSG"]  ,
                        Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                    });
                        
                messageBoxStandardWindow.Show(); 
            }
            else
            {
                Clients = _clientService.GetAll();
            }
        }
         
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