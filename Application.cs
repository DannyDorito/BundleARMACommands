using BundleARMACommands;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Starting ARMA Command Bundler");

        var commands = new List<string>();

        foreach (var uri in Scraper.UrisToScrape)
        {
            commands = await Scraper.GetRawData(uri, CancellationToken.None).ConfigureAwait(true);
            commands.ForEach(Console.WriteLine);
        }

        await Writer.WriteToXML(commands, args[0]).ConfigureAwait(true);
    }
}