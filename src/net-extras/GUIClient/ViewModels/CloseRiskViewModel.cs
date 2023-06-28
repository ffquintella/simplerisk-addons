using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using ClientServices.Interfaces;
using ClientServices.Services;
using DAL.Entities;
using GUIClient.Models;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class CloseRiskViewModel: ViewModelBase
{
    #region LANGUAGE

        public string StrCloseRisk { get; }
        public string StrReason { get; }
        public string StrNotes { get; }
        public string StrSave { get; }
        public string StrCancel { get; }
    #endregion
    
    #region PROPERTIES

        private List<CloseReason> _closeReasons;
        public List<CloseReason> CloseReasons
        {
            get => _closeReasons;
            set => this.RaiseAndSetIfChanged(ref _closeReasons, value);
        }
        
        private CloseReason _selectedCloseReason;
        public CloseReason SelectedCloseReason
        {
            get => _selectedCloseReason;
            set => this.RaiseAndSetIfChanged(ref _selectedCloseReason, value);
        }

        private bool _saveEnabled;
        public bool SaveEnabled
        {
            get => _saveEnabled;
            set => this.RaiseAndSetIfChanged(ref _saveEnabled, value);
        }
        
        public ReactiveCommand<Window, Unit> BtSaveClicked { get; }
        public ReactiveCommand<Window, Unit> BtCancelClicked { get; }
    #endregion

    #region INTERNAL FIELDS
        private Risk _risk;
        private readonly IRisksService _risksService;

    #endregion

    public CloseRiskViewModel(Risk risk)
    {
        StrCloseRisk = Localizer["CloseRisk"];
        StrReason = Localizer["Reason"];
        StrNotes = Localizer["Notes"];
        StrSave = Localizer["Save"];
        StrCancel = Localizer["Cancel"];
        
        _risk = risk;
        _risksService = GetService<IRisksService>();
        
        CloseReasons = _risksService.GetRiskCloseReasons();
        
        BtSaveClicked = ReactiveCommand.Create<Window>(ExecuteSave);
        BtCancelClicked = ReactiveCommand.Create<Window>(ExecuteCancel);
    }
    
    #region METHODS

    private async void ExecuteSave(Window baseWindow)
    {
    }
    
    private async void ExecuteCancel(Window baseWindow)
    {
    }

    #endregion
    
    
}