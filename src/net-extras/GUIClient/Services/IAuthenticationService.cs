using System.Collections.Generic;
using Avalonia.Controls;
using GUIClient.Models;
using Model.Authentication;

namespace GUIClient.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; set; }
    
    AuthenticationCredential AuthenticationCredential { get; set; }

    void TryAuthenticate(Window parentWindow);
    
    List<AuthenticationMethod> GetAuthenticationMethods();

    int DoServerAuthentication(string user, string password);

}