using System.Management.Automation;

namespace BundleARMACommands.Actions;

public static class Commit
{
    /// <summary>
    /// Commit changes to Npp-sqf to git
    /// </summary>
    /// <param name="repoLocation">Local location of repo</param>
    /// <param name="driveLetter">Drive letter of repo location, default is 'C:'</param>
    public static async Task PushToNppRepo(string repoLocation, string driveLetter = "C:")
    {
        if (string.IsNullOrWhiteSpace(repoLocation))
            throw new ArgumentNullException(nameof(repoLocation));

        if (string.IsNullOrWhiteSpace(driveLetter))
            throw new ArgumentNullException(nameof(driveLetter));

        using PowerShell powershell = PowerShell.Create();
        powershell.AddScript(driveLetter);
        powershell.AddScript($"cd {repoLocation}");
        powershell.AddScript("git add *");
        powershell.AddScript($"git commit -m 'Update autocompletion\\SQF.xml as of {DateTime.Now:d}'");
        powershell.AddScript(@"git push");
        await powershell.InvokeAsync().ConfigureAwait(true);
    }
}
