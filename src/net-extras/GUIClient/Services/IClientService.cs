using System.Collections.Generic;
using Model;

namespace GUIClient.Services;

public interface IClientService
{
    List<Client> GetAll();
    
    /// <summary>
    /// Approves the client with Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>-1 if error; 0 if ok;</returns>
    int Approve(int id);
}