using DAL;
using DAL.Entities;

namespace ServerServices;

public interface IClientRegistrationService
{
    List<AddonsClientRegistration> GetAll();
    List<AddonsClientRegistration> GetRequested();

    AddonsClientRegistration? GetRegistrationById(int id);
    
    void Delete(AddonsClientRegistration addonsClientRegistration);

    void Save(AddonsClientRegistration addonsClientRegistration);
}