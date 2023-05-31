using DAL.Entities;

namespace ServerServices;

public interface IAssetManagementService
{
    public List<Asset> GetAssets();
}