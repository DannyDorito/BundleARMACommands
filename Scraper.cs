using HtmlAgilityPack;

namespace BundleARMACommands;

public static class Scraper
{
    public static readonly List<Website> WebsitesToScrape = new()
    {
        new Website(new("https://community.bistudio.com/wiki/Category:Scripting_Commands"), WebsiteType.Commands),
        new Website(new("https://community.bistudio.com/wiki/Category:Functions"), WebsiteType.Functions),
        new Website(new("https://cbateam.github.io/CBA_A3/docs/index/Functions.html"), WebsiteType.CBA)
    };

    private static readonly string[] Filter = { "a != b", "! a", "a != b", "a % b", "a && b", "a &amp;&amp; b", "a * b", "a / b", "a : b", "a = b", "a == b", "a greater b", "a greater= b", "a hash b", "a less b", "a less= b", "a or b", "a ^ b", "+", "-" };  
    private static readonly List<string> Prepend = new() { "_exception", "_forEachIndex", "_this", "_thisArgs", "_thisEvent", "_thisEventHandler", "_thisFSM", "_thisScript", "_thisScriptedEventHandler", "_time", "_x", "_y" };

    private const string CBAAppend = "CBA_fnc_";

    public static async Task<List<string>> GetData(Website website, CancellationToken cancellationToken)
    {
        var returnCommands = new List<string>();

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
}
