using ARMACommands.Classes;
using ARMACommands.Enums;
using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace ARMACommands.Actions;

public static class Scraper
{
    private static ReadOnlyCollection<string> Filter => Universal.Filter;
    public static async Task<List<string>> GetData(Website? website, CancellationToken cancellationToken)
    {
        if (website == null)
            throw new ArgumentNullException(nameof(website));

        var returnCommands = new List<string>();

        var commands = await GetRawData(website, cancellationToken).ConfigureAwait(true);

        if (website.Prepend)
        {
            Console.WriteLine($"Website type: '{website.SiteType}' has prepend enabled, adding prepend list to existing commands");
            commands.AddRange(Universal.Prepend());
        }

        if (website.SiteType == WebsiteType.CBA)
        {
            Console.WriteLine($"Website type: '{website.SiteType}' is a CBA type, adding appending {Universal.CBAAppend} to command name");
            foreach (var command in commands)
            {
                var cbaCommand = $"{Universal.CBAAppend}{command}";
                returnCommands.Add(cbaCommand);
            }
        }
        else
        {
            returnCommands.AddRange(commands);
        }

        return returnCommands;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<Pending>")]
    public static List<string> FinaliseCommands(ICollection<string> commands)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        var finalCommands = new List<string>();

        foreach (var command in commands)
            finalCommands.Add($"{Universal.KeywordPrepend}{command}{Universal.KeywordAppend}");

        return finalCommands;
    }

    private static async Task<List<string>> GetRawData(Website website, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Getting raw data for: {website.Uri}");

        using var client = new HttpClient();
        var response = await client.GetStringAsync(website.Uri, cancellationToken).ConfigureAwait(true);
        var rawHtml = new HtmlDocument();
        rawHtml.LoadHtml(response);

        var nodes = rawHtml.DocumentNode.SelectNodes(website.XPath) ??
            throw new ArgumentOutOfRangeException($"Error: No nodes found for '{website.SiteType}'");

        var commands = nodes.Where(node => !Filter.Contains(node.InnerText))
            .Select(node => node.InnerText.Trim().Replace(' ', '_'))
            .ToList();

        Console.WriteLine($"Found {commands.Count} commands in {website.Uri}");

        return commands is not null ? commands : throw new ArgumentOutOfRangeException($"Error: No commands found for '{website.SiteType}'");
    }
}
