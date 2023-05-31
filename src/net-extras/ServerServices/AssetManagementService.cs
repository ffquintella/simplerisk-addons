﻿using DAL;
using DAL.Entities;
using Serilog;
using Serilog.Core;

namespace ServerServices;

public class AssetManagementService: IAssetManagementService
{
    private DALManager? _dalManager;
    private ILogger _logger;

    public AssetManagementService(DALManager dalManager, ILogger logger)
    {
        _dalManager = dalManager;
        _logger = logger;
    }
    
    public List<Asset> GetAssets()
    {
        
        var dbContext = _dalManager!.GetContext();
        var assets = dbContext?.Assets?.ToList();
        return assets;
        
    }
}