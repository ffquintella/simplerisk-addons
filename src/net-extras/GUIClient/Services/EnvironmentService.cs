using System.IO;
using DeviceId;
using GUIClient.Tools;
using SysEnv = System.Environment;

namespace GUIClient.Services;

public class EnvironmentService: IEnvironmentService
{
    public string NewLine => SysEnv.NewLine;

    public bool Is64BitProcess => SysEnv.Is64BitProcess;
    
    public string ApplicationData => SysEnv.GetFolderPath(SysEnv.SpecialFolder.ApplicationData);

    public string ApplicationDataFolder => ApplicationData + @"\SRGUIClient";


    public string DeviceToken
    {
        get
        {
            Directory.CreateDirectory(ApplicationDataFolder);
            if(!File.Exists(ApplicationDataFolder + @"\device_token.txt"))
            {
                var token = RandomGenerator.RandomString(20);
                File.WriteAllText(ApplicationDataFolder + @"\device_token.txt", token  );
            }
            
            return File.ReadAllText(ApplicationDataFolder + @"\device_token.txt");
        }
    }
    
    public string DeviceID
    {
        get
        {
            Directory.CreateDirectory(ApplicationDataFolder);
            
            if(!File.Exists(ApplicationDataFolder + @"\device_id.txt"))
            {
                string deviceId = new DeviceIdBuilder()
                    .AddMachineName()
                    .AddOsVersion()
                    .AddMacAddress()
                    .ToString();
                
                File.WriteAllText(ApplicationDataFolder + @"\device_id.txt", deviceId  );
            }

            return File.ReadAllText(ApplicationDataFolder + @"\device_id.txt");
            
            
        }
    }

    public string? GetEnvironmentVariable(string variableName) =>
        SysEnv.GetEnvironmentVariable(variableName);
}