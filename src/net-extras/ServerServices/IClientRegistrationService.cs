using DAL;
using DAL.Entities;

namespace ServerServices;

public interface IClientRegistrationService
{
    List<AddonsClientRegistration> GetAll();
    List<AddonsClientRegistration> GetRequested();
}