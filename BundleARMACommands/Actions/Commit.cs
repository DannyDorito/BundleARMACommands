using System.Diagnostics;

namespace BundleARMACommands.Actions;

public static class Commit
{
    /// <summary>
    /// Commit changes to Npp-sqf to git
    /// </summary>
    /// <param name="repoLocation">Local location of repo</param>
    /// <param name="cancellationToken">Async Cancellation Token</param>
    public static async Task PushToRepo(string repoLocation, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(repoLocation))
            throw new ArgumentNullException(nameof(repoLocation));

        if (!Directory.Exists(repoLocation))
            throw new DirectoryNotFoundException(repoLocation);

        var driveLetter = Path.GetPathRoot(repoLocation);

        if (string.IsNullOrWhiteSpace(driveLetter))
            throw new ArgumentException(nameof(driveLetter));

        var process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.ArgumentList.Add(driveLetter);
        process.StartInfo.ArgumentList.Add($"cd {repoLocation}");
        process.StartInfo.ArgumentList.Add("git add *");
        process.StartInfo.ArgumentList.Add($"git commit -m 'Update autocompletion\\SQF.xml as of {DateTime.Now:d}'");
        process.StartInfo.ArgumentList.Add(@"git push");

        process.Start();
        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(true);

        process.Dispose();
    }
}
