using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.IO.CompressionTasks;

class Build : NukeBuild
{

    AbsolutePath SourceDirectory => RootDirectory / "src"  / "net-extras";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath OutputBuildDirectory => RootDirectory / "output"  / "build";
    AbsolutePath OutputPublishDirectory => RootDirectory / "output"  / "publish";
    
    [GitRepository] readonly GitRepository Repository;
    
    
    string Version => Repository?.Tags?.FirstOrDefault() ?? "0.0.0";
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
    [Solution]
    readonly Solution Solution;

    Target Clean => _ => _
        .Before(Prepare)
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
            if(Directory.Exists(OutputBuildDirectory))
                Directory.Delete(OutputBuildDirectory, true);
            
        });
    
    Target Prepare => _ => _
        .Before(Restore)
        .DependsOn(Print)
        .Executes(() =>
        {
            if (!Directory.Exists(OutputDirectory)) Directory.CreateDirectory(OutputDirectory);
            if (!Directory.Exists(OutputBuildDirectory)) Directory.CreateDirectory(OutputBuildDirectory);
            if (!Directory.Exists(OutputPublishDirectory)) Directory.CreateDirectory(OutputPublishDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Print)
        .Executes(() =>
        {
            
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.Normal));
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
            
            Log.Information("Solution path = {Value}", Solution);
            Log.Information("Solution directory = {Value}", Solution.Directory);

            Log.Information("-- PROJECTS --");
            foreach (var project in Solution.Projects)
            {
                Log.Information("=> {Value}", project.Name);
                Log.Information("=> Frameworks:");
                foreach (var framework in project.GetTargetFrameworks())
                {
                    Log.Information("-=> {Value}", framework);    
                }
                
            }
        });
    
    Target Compile => _ => _
        .DependsOn(Print)
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetVersion(Version)
                .SetConfiguration(Configuration)
                .SetOutputDirectory(OutputBuildDirectory)
                .SetVerbosity(DotNetVerbosity.Normal));
            
        });
    
    Target PublishApi => _ => _
        .DependsOn(Clean)
        .DependsOn(Restore)
        //.DependsOn(Compile)
        .Executes(() =>
        {
            var project = Solution.GetProject("API");
            
            DotNetPublish(s => s
                .SetProject(project)
                .SetVersion(Version)
                .SetConfiguration(Configuration.Release)
                .SetRuntime("linux-x64")
                .EnablePublishTrimmed()
                .EnablePublishSingleFile()
                .SetOutput(OutputPublishDirectory / "api")
                .EnablePublishReadyToRun()
                .SetVerbosity(DotNetVerbosity.Normal));
            
            var archive = OutputPublishDirectory / $"SRNET-Server-${Version}.zip";
            
            CompressZip(OutputPublishDirectory / "api", 
                archive);

            var checksum = GetFileHash(archive);
            var checksumFile = OutputPublishDirectory / $"SRNET-Server-${Version}.sha256";  
            File.WriteAllText(checksumFile, checksum);

        });

    Target PublishConsoleClient => _ => _
        .DependsOn(Clean)
        .DependsOn(Restore)
        .Executes(() =>
        {
            var project = Solution.GetProject("ConsoleClient");
            
            DotNetPublish(s => s
                .SetProject(project)
                .SetVersion(Version)
                .SetConfiguration(Configuration.Release)
                .SetRuntime("linux-x64")
                .EnablePublishTrimmed()
                .EnablePublishSingleFile()
                .SetOutput(OutputPublishDirectory / "consoleClient")
                .EnablePublishReadyToRun()
                .SetVerbosity(DotNetVerbosity.Normal));
            
            var archive = OutputPublishDirectory / $"SRNET-ConsoleClient-${Version}.zip";
            
            CompressZip(OutputPublishDirectory / "consoleClient", 
                archive);

            var checksum = GetFileHash(archive);
            var checksumFile = OutputPublishDirectory / $"SRNET-ConsoleClient-${Version}.sha256";  
            File.WriteAllText(checksumFile, checksum);

        });
    
}
