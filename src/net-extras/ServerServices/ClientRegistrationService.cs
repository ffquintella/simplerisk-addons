using DAL.Entities;

namespace ServerServices;

public class ClientRegistrationService: IClientRegistrationService
{
    public List<AddonsClientRegistration> GetAll()
    {
      var result = new List<AddonsClientRegistration>();
      
      return result;
    }
}