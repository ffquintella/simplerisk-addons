﻿using DAL;
using DAL.Entities;

namespace ServerServices;

public interface IClientRegistrationService
{
    List<AddonsClientRegistration> GetAll();
    List<AddonsClientRegistration> GetRequested();

    AddonsClientRegistration? GetRegistrationById(int id);
    
    int Delete(AddonsClientRegistration addonsClientRegistration);

    int Update(AddonsClientRegistration addonsClientRegistration);
    
    int Add(AddonsClientRegistration addonsClientRegistration);
    int IsAccepted(string externalId);
}