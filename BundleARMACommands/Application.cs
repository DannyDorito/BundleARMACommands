using BundleARMACommands.Actions;
using BundleARMACommands.Classes;
using BundleARMACommands.Enums;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine($"Starting {typeof(Program).Assembly.GetName().Name}.");

        if (args.Length != 2)
            throw new ArgumentNullException(nameof(args));

        var commands = new List<string>();

        foreach (var website in Universal.WebsitesToScrape)
            commands.AddRange(await Scraper.GetData(website, CancellationToken.None).ConfigureAwait(true));

        Console.WriteLine($"Setting String Comparison to '{StringComparer.OrdinalIgnoreCase}'");

        commands.Sort(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine($"{commands.Count} commands found.");

        var changed = Writer.WriteXML(commands, args.ElementAt(0), FileType.AutoComplete);

        if (changed)
        {
            await Commit.PushToRepo(args.ElementAt(1), CancellationToken.None).ConfigureAwait(true);
            Console.WriteLine($"Pushed Changes to {args.ElementAt(1)} repo.");
        }

        Console.WriteLine($"{(!changed ? "No changes found, " : "")}Exiting {typeof(Program).Assembly.GetName().Name}.");
    }
}