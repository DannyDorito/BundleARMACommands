using BundleARMACommands;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Starting ARMA Command Bundler");

        var commands = new List<string>();

        foreach (var uri in Scraper.UrisToScrape)
        {
            commands.AddRange( await Scraper.GetRawData(uri, CancellationToken.None).ConfigureAwait(true));
        }

        commands.Sort(StringComparer.OrdinalIgnoreCase);

        var hackyCommands = new List<string>();

        foreach (var c in commands)
        {
            hackyCommands.Add($"<KeyWord name=\"{c}\" />");
        }

        var hackyCommandsStr = string.Join("\r\n", hackyCommands);

        File.WriteAllText(@$"{Environment.CurrentDirectory}\output.txt", hackyCommandsStr);

        //await Writer.WriteToXML(commands, args[0]).ConfigureAwait(true);
    }
}