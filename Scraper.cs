using HtmlAgilityPack;

namespace BundleARMACommands;

public static class Scraper
{
    public static readonly Uri[] UrisToScrape = { new("https://community.bistudio.com/wiki/Category:Scripting_Commands"), new("https://community.bistudio.com/wiki/Category:Functions") };
    private static readonly string[] Filter = { "a != b", "! a", "a != b", "a % b", "a && b", "a * b", "a / b", "a : b", "a = b", "a == b", "a greater b", "a greater= b", "a hash b", "a less b", "a less= b", "a or b", "a ^ b" };

    public static async Task<List<string>> GetRawData(Uri uri, CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        var response = await client.GetStringAsync(uri, cancellationToken).ConfigureAwait(true);
        var rawHtml = new HtmlDocument();
        rawHtml.LoadHtml(response);

        var nodes = rawHtml.DocumentNode.SelectNodes("//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]");

        var commands = nodes.Where(node => !string.IsNullOrEmpty(node.InnerHtml.Trim())
            && !Filter.Contains(node.InnerText))
        .Select(node => node.InnerText.Trim().Replace(' ', '_'))
        .ToList();

        return commands;
    }
}
