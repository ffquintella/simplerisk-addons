using DAL;
using DAL.Entities;
using Serilog;
using Serilog.Core;

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
    /// <summary>
    /// Updates a client registration data
    /// </summary>
    /// <param name="addonsClientRegistration"></param>
    /// <returns>0 if success; -1 if failure</returns>
    public int Update(AddonsClientRegistration addonsClientRegistration)
    {
        var result = 0;
        try
        {
            var context = _dalManager.GetContext();
            context.AddonsClientRegistrations.Update(addonsClientRegistration);
            context.SaveChanges();
        }catch (Exception ex)
        {
            Log.Error("Error updating a registration ex: {0}", ex.Message);
            result = -1;
        }
        return result;
    }

    public AddonsClientRegistration? GetRegistrationById(int id)
    {
        var context = _dalManager.GetContext();
        var request = context.AddonsClientRegistrations.Where(cr => cr.Id == id).FirstOrDefault();
        return request;
    }

    /// <summary>
    /// Deletes a client registration from db
    /// </summary>
    /// <param name="addonsClientRegistration"></param>
    /// <returns>0 if success; -1 if failure</returns>
    public int Delete(AddonsClientRegistration addonsClientRegistration)
    {
        var result = 0;
        try
        {
            var context = _dalManager.GetContext();
            context.AddonsClientRegistrations.Remove(addonsClientRegistration);
            context.SaveChanges();
        }catch (Exception ex)
        {
            Log.Error("Error deleting a registration ex: {0}", ex.Message);
            result = -1;
        }
        return result;
    }

    /// <summary>
    /// Adds a new Client Registration
    /// </summary>
    /// <param name="addonsClientRegistration"></param>
    /// <returns>0 if success; 1 if client already exists; -1 if failure</returns>
    public int Add(AddonsClientRegistration addonsClientRegistration)
    {
        var result = 0;
        try
        {
            var context = _dalManager.GetContext();

            var nfound = context.AddonsClientRegistrations
                .Count(cr => cr.ExternalId == addonsClientRegistration.ExternalId 
                             || cr.Name == addonsClientRegistration.Name);

            if (nfound > 0)
            {
                Log.Warning("Trying to add an already existing client");
                return 1;
            }
            
            context.AddonsClientRegistrations.Add(addonsClientRegistration);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            Log.Error("Error adding new client registration ex: {0}", ex.Message);
            result = -1;
        }
        return result;
    }
}