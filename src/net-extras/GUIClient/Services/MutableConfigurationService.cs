﻿using System.IO;
using GUIClient.Configuration;
using LiteDB;

namespace GUIClient.Services;

public class MutableConfigurationService: IMutableConfigurationService
{
    private IEnvironmentService _environmentService;

    private string _configurationFilePath;
    private string _configurationConnectionString;
    public MutableConfigurationService(IEnvironmentService environmentService )
    {
        _environmentService = environmentService;
        _configurationFilePath = _environmentService.ApplicationDataFolder + @"\configuration.db";
        _configurationConnectionString = "Filename=" + _configurationFilePath + ";Upgrade=true;Password="+_environmentService.DeviceID;
    }

    public bool IsInitialized
    {
        get
        {
            if (_environmentService == null) return false;
            return File.Exists(_configurationFilePath);
        }
    }

    public void Initialize()
    {
        if (!IsInitialized)
        {
            if(!Directory.Exists(_environmentService.ApplicationDataFolder)) Directory.CreateDirectory(_environmentService.ApplicationDataFolder);
            
            using (var db = new LiteDatabase(_configurationConnectionString))
            {
                var col = db.GetCollection<MutableConfiguration>("configuration");

                col.Insert(new MutableConfiguration
                {
                    ID = 1,
                    Name = "DeviceID",
                    Value = _environmentService.DeviceID
                });
                col.EnsureIndex(x => x.Name);
            }
        }
    }
}