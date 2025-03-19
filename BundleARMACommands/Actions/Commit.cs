using System.Management.Automation;

namespace BundleARMACommands.Actions;

public static class Commit
{
    /// <summary>
    /// Commit changes to Npp-sqf to git
    /// </summary>
    /// <param name="repoLocation">Local location of repo</param>
    /// <param name="cancellationToken">Async Cancellation Token</param>
    public static async Task PushToRepo(string repoLocation)
    {
        if (string.IsNullOrWhiteSpace(repoLocation))
            throw new ArgumentNullException(nameof(repoLocation));

        if (!Directory.Exists(repoLocation))
            throw new DirectoryNotFoundException(repoLocation);

        using PowerShell ps = PowerShell.Create();
        ps.AddScript("cd {repoLocation}");
        ps.AddScript("git add *");
        ps.AddScript(@$"git commit -m 'Update autocompletion\SQF.xml as of {DateTime.Now:dd:MM:yyyy} with automated tool BundleARMACommands.'");
        ps.AddScript("git push");
        await ps.InvokeAsync().ConfigureAwait(true);
    }
}
