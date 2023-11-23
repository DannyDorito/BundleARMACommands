using BundleARMACommands;

Console.WriteLine("Starting ARMA Command Bundler");

var commands = new List<string>();

for (int uriIndex = 0; uriIndex < Scraper.UrisToScrape.Length; uriIndex++)
{
    commands.AddRange(await Scraper.GetRawData(Scraper.UrisToScrape[uriIndex], CancellationToken.None, uriIndex == 0).ConfigureAwait(true));
}

commands.Sort(StringComparer.OrdinalIgnoreCase);

//Writer.WriteToXML(commands, @"F:\GitHub\BundleARMACommands\SQF.xml");
Writer.WriteHacky(commands);