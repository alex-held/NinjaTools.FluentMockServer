#load "nuget:Dotnet.Build, 0.7.1"
#load "nuget:dotnet-steps, 0.0.2"


using static FileUtils;
using System.Xml.Linq;
using System.Text.RegularExpressions;

public string rootPath = Directory.GetParent(FileUtils.GetScriptFolder()).FullName;
string BuildId = Args.FirstOrDefault();

foreach (var arg in Args)
{
    Console.WriteLine(arg);
}

[StepDescription("Creates all NuGet packages and the release zip for GitHub releases")]
Step pack = () =>
{
    build();
    CreateNuGetPackage("NinjaTools.FluentMockServer");
};

[StepDescription("Builds the projects..")]
[DefaultStep]
Step build = () =>
{
     var version = GetVersion("NinjaTools.FluentMockServer");
     var projectPath = GetProjectPath("NinjaTools.FluentMockServer");
     Command.Execute($"dotnet", $"build {projectPath} -c Release -p:Version={version}");
};

await ExecuteSteps(Args);

public string GetProjectPath(string project) => Directory.GetFiles(rootPath, $"{project}.csproj", SearchOption.AllDirectories).Single();

public string GetVersion(string project)
{
    var projectPath = GetProjectPath(project);
    var projectFile = XDocument.Load(projectPath);
    var versionElement = projectFile.Descendants("Version").Single();
    var version = versionElement.Value;
    var match = Regex.Match(version, @"^(?<major>\d*)\.(?<minor>\d*)\.(?<patch>\d*)");
    var major = int.Parse(match.Groups["major"].Value);
    var minor = int.Parse(match.Groups["minor"].Value);
    var patch = int.Parse(match.Groups["patch"].Value);

    var head = File.ReadAllText(Path.Combine(rootPath, ".git", "HEAD"));
    if(head.Contains("refs/heads/master") || Args.Count() < 2 )
    {
        Console.WriteLine($"[{project}] Using stable version: {version}");
        return version;
    }
    else
    {
        version= $"{++major}.{minor}.{patch}-pre{Args[1]}";
        Console.WriteLine($"[{project}] Using prerelease version: {version}");

        return version;
    }
}




private void CreateNuGetPackage(string project)
{
    var version = GetVersion(project);
    var projectPath = GetProjectPath(project);

    if(Args.Count() == 3)
    {
        var output = Args[2];
        Command.Execute("dotnet", $"pack {projectPath} --no-build -p:Version={version} -c Release --output {output} -v d /nologo");
    }
    else
        Command.Execute("dotnet", $"pack {projectPath} --no-build -p:Version={version} -c Release -v d /nologo");
}
