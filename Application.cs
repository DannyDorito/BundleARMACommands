namespace BundleARMACommands;

internal class Application
{
    static async Task Main()
    {
        Console.WriteLine("Starting ARMA Command Bundler");
        var x = await Scraper.GetRawData(CancellationToken.None);
        x.ForEach(Console.WriteLine);
    }
}