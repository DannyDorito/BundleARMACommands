using BundleARMACommands;

Console.WriteLine(value: "Starting ARMA Command Bundler");

var commands = new List<string>();

for (int uriIndex = 0; uriIndex < Scraper.UrisToScrape.Length; uriIndex++)
{
    commands.AddRange(await Scraper.GetRawData(Scraper.UrisToScrape[uriIndex], CancellationToken.None, uriIndex == 0).ConfigureAwait(true));
}

commands.Sort(StringComparer.OrdinalIgnoreCase);

var hackyCommands = new List<string>();

foreach (var c in commands)
{
    hackyCommands.Add($"\t\t<KeyWord name=\"{c}\" />");
}

var hackyCommandsStr = string.Join("\r\n", hackyCommands);

File.WriteAllText(@$"{Environment.CurrentDirectory}\output.txt", hackyCommandsStr);
