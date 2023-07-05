using System.Collections.ObjectModel;
using ClientServices.Interfaces;
using DAL.Entities;
using Model.DTO;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class UsersViewModel: ViewModelBase
{
    #region LANGUAGE
    private string StrUsers { get;  }
    
    #endregion

    #region PROPERTIES

    private ObservableCollection<UserListing> _users;
    public ObservableCollection<UserListing> Users
    {
        get => _users;
        set => this.RaiseAndSetIfChanged(ref _users, value);
    }
    
    private UserListing _selectedUser;
    public UserListing SelectedUser
    {
        get => _selectedUser;
        set => this.RaiseAndSetIfChanged(ref _selectedUser, value);
    }
    
    #endregion

    #region PRIVATE FIELDS
        private readonly IUsersService _usersService = GetService<IUsersService>();
        private readonly IAuthenticationService _authenticationService = GetService<IAuthenticationService>();
        private bool _initialized;

    #endregion
    public UsersViewModel()
    {
        StrUsers = Localizer["Users"];

        _users = new ObservableCollection<UserListing>();
        _usersService.UserAdded += (_, user) => _users.Add(user.User);        
        _authenticationService.AuthenticationSucceeded += (_, _) =>
        {
            Initialize();
            
        };
    }

    private void Initialize()
    {
        if (_initialized) return;
        Users = new ObservableCollection<UserListing>(_usersService.ListUsers());
        _initialized = true;
    }
}