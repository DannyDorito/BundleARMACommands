using BundleARMACommands;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine($"Starting {typeof(Program).Assembly.GetName().Name}.");

        if (args.Length != 3)
            throw new ArgumentNullException(nameof(args));

        var commands = new List<string>();

        foreach (var website in Scraper.WebsitesToScrape)
            commands.AddRange(await Scraper.GetData(website, CancellationToken.None).ConfigureAwait(true));

        Console.WriteLine($"Setting String Comparison to '{StringComparer.OrdinalIgnoreCase}'");

        commands.Sort(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine($"{commands.Count} commands found.");

        var changed = Writer.WriteToXML(commands, args.ElementAt(0));

        if (changed)
        {
            await Commit.PushToNppRepo(args.ElementAt(2), args.ElementAt(1)).ConfigureAwait(true);
            Console.WriteLine($"Pushed Changes to {args.ElementAt(2)} repo.");
        }

        Console.WriteLine($"{(!changed ? "No changes found, " : "")}Exiting {typeof(Program).Assembly.GetName().Name}.");
    }
}