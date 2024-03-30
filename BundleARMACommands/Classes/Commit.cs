using System.Management.Automation;

namespace BundleARMACommands;

public static class Commit
{
    /// <summary>
    /// Commit changes to Npp-sqf to git
    /// </summary>
    /// <param name="repoLocation">Local location of repo</param>
    /// <param name="driveLetter">Drive letter of repo location, default is 'C:'</param>
    public static void PushToNppRepo(string repoLocation, string driveLetter = "C:")
    {
        using PowerShell powershell = PowerShell.Create();
        powershell.AddScript(driveLetter);
        powershell.AddScript($"cd {repoLocation}");
        powershell.AddScript("git add *");
        powershell.AddScript($"git commit -m 'Update autocompletion\\SQF.xml as of {DateTime.Now:d}'");
        powershell.AddScript(@"git push");
        powershell.Invoke();
    }
}
