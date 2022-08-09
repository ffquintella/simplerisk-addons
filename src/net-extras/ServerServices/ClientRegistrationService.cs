using DAL;
using DAL.Entities;

namespace ServerServices;

public class ClientRegistrationService: IClientRegistrationService
{

    private DALManager _dalManager;
    public ClientRegistrationService(DALManager dalManager)
    {
        _dalManager = dalManager;
    }
    
    public List<AddonsClientRegistration> GetAll()
    {
      var result = new List<AddonsClientRegistration>();
      
      var context = _dalManager.GetContext();
      var registrations = context.AddonsClientRegistrations.ToList();
      if (registrations != null) result = registrations;
      
      return result;
    }
    
    public List<AddonsClientRegistration> GetRequested()
    {
        var result = new List<AddonsClientRegistration>();
      
        var context = _dalManager.GetContext();
        var registrations = context.AddonsClientRegistrations.Where(ad => ad.Status == "requested").ToList();
        if (registrations != null) result = registrations;
      
        return result;
    }
}