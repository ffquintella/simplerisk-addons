using DAL.Entities;

namespace ClientServices.Interfaces;

public interface IMitigationService
{
    /// <summary>
    /// Gets mitigation by Risk Id
    /// </summary>
    /// <param name="id">Risk Id</param>
    /// <returns></returns>
    public Mitigation? GetByRiskId(int id);
    
    /// <summary>
    /// Gets mitigation by id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns></returns>
    public Mitigation? GetById(int id);
}