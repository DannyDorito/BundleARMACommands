using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleARMACommands;

public static class Scraper
{
    private static readonly string CommandsUrl = "https://community.bistudio.com/wiki/Category:Scripting_Commands";
    private static readonly char[] Filter = { '►', ' ', '+', '-' };

    public static async Task<List<string>> GetRawData(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var response = await client.GetStringAsync(CommandsUrl, cancellationToken);

        var rawHtml = new HtmlDocument();
        rawHtml.LoadHtml(response);

        var nodes = rawHtml.DocumentNode.SelectNodes("//li[not(contains(@class, 'bohemia-header-nav-item'))]")
		    .ToList();

        var commands = nodes.Where(node => node.InnerHtml.Trim() != string.Empty
                && !node.InnerText.Any(n => Filter.Contains(n))
                && !char.IsUpper(node.InnerText.FirstOrDefault()))
            .Select(node => node.InnerText.Trim())
            .ToList();

        return commands;
    }
}
