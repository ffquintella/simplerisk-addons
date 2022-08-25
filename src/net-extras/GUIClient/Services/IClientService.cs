using System.Collections.Generic;
using Model;

namespace GUIClient.Services;

public interface IClientService
{
    List<Client> GetAll();
}