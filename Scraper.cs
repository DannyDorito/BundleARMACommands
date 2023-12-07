using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace BundleARMACommands;

public static class Scraper
{
    public static readonly Collection<Website> WebsitesToScrape =
    [
        new Website(new("https://community.bistudio.com/wiki/Category:Scripting_Commands"), WebsiteType.Commands),
        new Website(new("https://community.bistudio.com/wiki/Category:Functions"), WebsiteType.Functions),
        new Website(new("https://cbateam.github.io/CBA_A3/docs/index/Functions.html"), WebsiteType.CBA)
    ];

    private static readonly Collection<string> Filter = ["a != b", "! a", "a != b", "a % b", "a && b", "a &amp;&amp; b", "a * b", "a / b", "a : b", "a = b", "a == b", "a greater b", "a greater= b", "a hash b", "a less b", "a less= b", "a or b", "a ^ b", "+", "-"];  
    private static readonly Collection<string> Prepend = ["_exception", "_forEachIndex", "_this", "_thisArgs", "_thisEvent", "_thisEventHandler", "_thisFSM", "_thisScript", "_thisScriptedEventHandler", "_time", "_x", "_y"];

    private const string CBAAppend = "CBA_fnc_";

    public static async Task<List<string>> GetData(Website website, CancellationToken cancellationToken)
    {
        var returnCommands = new List<string>();

        var commands = await GetRawData(website, cancellationToken).ConfigureAwait(true);

        if (website.Prepend)
        {
            commands.AddRange(Prepend);
        }

        switch (website.SiteType)
        {
            case WebsiteType.CBA:
                {
                    foreach (var command in commands)
                    {
                        var cbaCommand = $"{CBAAppend}{command}";
                        returnCommands.Add(cbaCommand);
                    }

                    break;
                }

            default:
                returnCommands.AddRange(commands);
                break;
        }

        return returnCommands;
    }

    private static async Task<List<string>> GetRawData(Website website, CancellationToken cancellationToken)
    {
        if (website is null)
            throw new ArgumentNullException(nameof(website));

        using var client = new HttpClient();
        var response = await client.GetStringAsync(website.Uri, cancellationToken).ConfigureAwait(true);
        var rawHtml = new HtmlDocument();
        rawHtml.LoadHtml(response);

        var nodes = rawHtml.DocumentNode.SelectNodes(website.XPath());

        var commands = nodes.Where(node => !string.IsNullOrEmpty(node.InnerHtml.Trim())
            && !Filter.Contains(node.InnerText))
        .Select(node => node.InnerText.Trim().Replace(' ', '_'))
        .ToList();

        return commands;
    }
}
