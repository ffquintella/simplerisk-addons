using RestSharp;
using RestSharp.Authenticators;

namespace GUIClient.Services;

public interface IRestService
{
    RestClient GetClient(IAuthenticator? autenticator = null,  bool ignoreTimeVerification = false);
}