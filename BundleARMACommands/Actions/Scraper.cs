using BundleARMACommands.Classes;
using BundleARMACommands.Enums;
using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace BundleARMACommands.Actions;

public static class Scraper
{
    public static async Task<List<string>> GetData(Website? website, CancellationToken cancellationToken)
    {
        if (website == null)
            throw new ArgumentNullException(nameof(website));

        var returnCommands = new List<string>();

        var commands = await GetRawData(website, cancellationToken).ConfigureAwait(true);

        if (website.Prepend)
        {
            Console.WriteLine($"Website type: '{website.SiteType}' has prepend enabled, adding prepend list to existing commands");
            commands.AddRange(Global.Prepend);
        }

        if (website.SiteType == WebsiteType.CBA)
        {
            Console.WriteLine($"Website type: '{website.SiteType}' is a CBA type, adding appending {Global.CBAAppend} to command name");
            foreach (var command in commands)
            {
                var cbaCommand = $"{Global.CBAAppend}{command}";
                returnCommands.Add(cbaCommand);
            }
        }
        else
        {
            returnCommands.AddRange(commands);
        }

        return returnCommands;
    }

    public static List<string> FinaliseCommands(ICollection<string> commands, string path)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        var finalCommands = new List<string>();

        foreach (var command in commands)
            finalCommands.Add($"{Global.KeywordPrepend}{command}{Global.KeywordAppend}");

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

        var commands = nodes.Where(node => !Global.Filter.Contains(node.InnerText))
            .Select(node => node.InnerText.Trim().Replace(' ', '_'))
            .ToList();

        Console.WriteLine($"Found {commands.Count} commands in {website.Uri}");

        return commands is not null ? commands : throw new ArgumentOutOfRangeException($"Error: No commands found for '{website.SiteType}'");
    }
}
