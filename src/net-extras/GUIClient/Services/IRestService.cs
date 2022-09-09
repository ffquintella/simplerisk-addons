using RestSharp;

namespace GUIClient.Services;

public interface IRestService
{
    RestClient GetClient(bool ignoreTimeVerification = false);
}