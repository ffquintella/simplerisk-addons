using System.Collections.Generic;
using DAL.Entities;

namespace GUIClient.Services;

public interface IRisksService
{
    public List<Risk> GetAllRisks();
}