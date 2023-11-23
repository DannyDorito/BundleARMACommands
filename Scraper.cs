using HtmlAgilityPack;

namespace BundleARMACommands;

public static class Scraper
{
    public static readonly Uri[] UrisToScrape = { new("https://community.bistudio.com/wiki/Category:Scripting_Commands"), new("https://community.bistudio.com/wiki/Category:Functions") };
    private static readonly string[] Filter = { "a != b", "! a", "a != b", "a % b", "a && b", "a * b", "a / b", "a : b", "a = b", "a == b", "a greater b", "a greater= b", "a hash b", "a less b", "a less= b", "a or b", "a ^ b", "+", "-" };
    private static readonly List<string> Prepend = new() { "\t\t<KeyWord name=\"_exception\" />", "\t\t<KeyWord name=\"_forEachIndex\" />", "\t\t<KeyWord name=\"_this\" />", "\t\t<KeyWord name=\"_thisArgs\" />", "\t\t<KeyWord name=\"_thisEvent\" />", "\t\t<KeyWord name=\"_thisEventHandler\" />", "\t\t<KeyWord name=\"_thisFSM\" />", "\t\t<KeyWord name=\"_thisScript\" />", "\t\t<KeyWord name=\"_thisScriptedEventHandler\" />", "\t\t<KeyWord name=\"_time\" />", "\t\t<KeyWord name=\"_x\" />", "\t\t<KeyWord name=\"_y\" />" };
    public static async Task<List<string>> GetRawData(Uri uri, CancellationToken cancellationToken, bool prepend = false)
    {
        var returnCommands = new List<string>();

        using var client = new HttpClient();
        var response = await client.GetStringAsync(uri, cancellationToken).ConfigureAwait(true);
        var rawHtml = new HtmlDocument();
        rawHtml.LoadHtml(response);

        var nodes = rawHtml.DocumentNode.SelectNodes("//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]");

        var commands = nodes.Where(node => !string.IsNullOrEmpty(node.InnerHtml.Trim())
            && !Filter.Contains(node.InnerText))
        .Select(node => node.InnerText.Trim().Replace(' ', '_'))
        .ToList();

        if (prepend)
        {
            returnCommands.AddRange(Prepend);
        }
        foreach (var command in commands)
        {
            returnCommands.Add($"\t\t<KeyWord name=\"{command}\" />");
        }

        return returnCommands;
    }
}
