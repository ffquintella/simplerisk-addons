using Tools;
using SysEnv = System.Environment;

namespace ServerServices;

public class EnvironmentService: IEnvironmentService
{
    private string ApplicationData => SysEnv.GetFolderPath(SysEnv.SpecialFolder.ApplicationData);

    public string ApplicationDataFolder => ApplicationData + @"\SRServer";
    
    public string ServerSecretToken
    {
        get
        {
            Directory.CreateDirectory(ApplicationDataFolder);
            if(!File.Exists(ApplicationDataFolder + @"\secret_token.txt"))
            {
                var token = RandomGenerator.RandomString(37);
                File.WriteAllText(ApplicationDataFolder + @"\secret_token.txt", token  );
            }
            
            return File.ReadAllText(ApplicationDataFolder + @"\secret_token.txt");
        }
    }
    
}