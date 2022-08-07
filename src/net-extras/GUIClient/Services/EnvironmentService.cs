using SysEnv = System.Environment;

namespace GUIClient.Services;

public class EnvironmentService: IEnvironmentService
{
    public string NewLine => SysEnv.NewLine;

    public bool Is64BitProcess => SysEnv.Is64BitProcess;
    
    public string ApplicationData => SysEnv.GetFolderPath(SysEnv.SpecialFolder.ApplicationData);

    public string ApplicationDataFolder
    {
        get { return ApplicationData + @"\SRGUIClient"; }
    }

    public string? GetEnvironmentVariable(string variableName) =>
        SysEnv.GetEnvironmentVariable(variableName);
}