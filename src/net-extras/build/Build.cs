using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{

    AbsolutePath SourceDirectory => RootDirectory / "src"  / "net-extras";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            // Collect and delete all /obj and /bin directories in all sub-directories
            var deletableDirectories = SourceDirectory.GlobDirectories("**/obj", "**/bin");
            foreach (var deletableDirectory in deletableDirectories)
            {
                if(!deletableDirectory.ToString().Contains("build"))
                {
                    Logger.Info($"Deleting {deletableDirectory}");
                    Directory.Delete(deletableDirectory, true);
                }
                
            }
            //deletableDirectories.ForEach(FileSystemTasks.DeleteDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Log.Information("STARTING BUILD");
            Log.Information("SOURCE DIR: {0}", SourceDirectory);
            Log.Information("OUTPUT DIR: {0}", OutputDirectory);
            
        });

}
