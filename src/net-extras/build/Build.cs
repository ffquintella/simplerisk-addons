using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.MinVer;
using Nuke.Common.Tools.NerdbankGitVersioning;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

class Build : NukeBuild
{

    AbsolutePath SourceDirectory => RootDirectory / "src"  / "net-extras";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    [GitRepository] readonly GitRepository Repository;
    
    
    string Version => Repository?.Tags?.FirstOrDefault() ?? "0.dev";
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

    Target Print => _ => _
        .Executes(() =>
        {
            Log.Information("STARTING BUILD");
            Log.Information("SOURCE DIR: {0}", SourceDirectory);
            Log.Information("OUTPUT DIR: {0}", OutputDirectory);
            
            
            Log.Information("Commit = {Value}", Repository.Commit);
            Log.Information("Branch = {Value}", Repository.Branch);
            Log.Information("Tags = {Value}", Repository.Tags);

            Log.Information("main branch = {Value}", Repository.IsOnMainBranch());
            Log.Information("main/master branch = {Value}", Repository.IsOnMainOrMasterBranch());
            
            Log.Information("VersionInfo = {Value}", Version);
        });
    
    Target Compile => _ => _
        .DependsOn(Print)
        .DependsOn(Restore)
        .Executes(() =>
        {

            
        });

}
