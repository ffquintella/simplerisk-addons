using System.Collections.Generic;
using Avalonia.Controls;
using GUIClient.Models;
using Model.Authentication;

namespace GUIClient.Services;

/// <summary>
/// A service for managing the authentication of the user.
/// </summary>
public interface IAuthenticationService
{
    bool IsAuthenticated { get; set; }
    
    AuthenticationCredential AuthenticationCredential { get; set; }

    void TryAuthenticate(Window parentWindow);
    
    /// <summary>
    /// Gets the list of authentication methods avaliable at the server.
    /// </summary>
    /// <returns> A list of authentication methods</returns>
    List<AuthenticationMethod> GetAuthenticationMethods();
    
    /// <summary>
    /// Authenticates the user with the given credentials.
    /// </summary>
    /// <param name="user"> Username</param>
    /// <param name="password"> User password</param>
    /// <returns>0 if success; -1 if unkown error; 1 if authentication error</returns>
    int DoServerAuthentication(string user, string password);

    /// <summary>
    ///  Gets the information about the authenticated user from the server.
    /// </summary>
    /// <returns>0 if success; -1 if internal error; 1 if communication error;</returns>
    int GetAuthenticatedUserInfo();

}