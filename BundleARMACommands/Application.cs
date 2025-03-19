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

        if (args.Length < 2)
            throw new ArgumentNullException(nameof(args));

        var commands = new List<string>();

        foreach (var website in Universal.WebsitesToScrape)
            commands.AddRange(await Scraper.GetData(website, CancellationToken.None).ConfigureAwait(true));

        Console.WriteLine($"Setting String Comparison to '{StringComparer.OrdinalIgnoreCase}'");

        commands.Sort(StringComparer.OrdinalIgnoreCase);

        Console.WriteLine($"{commands.Count} commands found.");

        var changed = Writer.WriteXML(commands, args.ElementAt(0), FileType.AutoComplete);

        bool? commit = null;
        try
        {
            commit = args.ElementAt(2) != "no-commit";
            if (!commit.Value)
                Console.WriteLine($"Found no-commit in {nameof(args)}, skipping pushing changes to repo.");
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine($"Did not find no-commit in {nameof(args)}, pushing changes to repo.");
        }

        if (changed && (commit.HasValue && commit.Value))
        {
            await Commit.PushToRepo(args.ElementAt(1)).ConfigureAwait(true);
            Console.WriteLine($"Pushed Changes to {args.ElementAt(1)} repo.");
        }

        Console.WriteLine($"{(!changed ? "No changes found, " : "")}Exiting {typeof(Program).Assembly.GetName().Name}.");
    }
}