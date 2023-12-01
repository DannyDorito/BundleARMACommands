using BundleARMACommands;


Console.WriteLine("Starting ARMA Command Bundler");

var commands = new List<string>();

foreach (var website in Scraper.WebsitesToScrape)
    commands.AddRange(await Scraper.GetData(website, CancellationToken.None).ConfigureAwait(true));

commands.Sort(StringComparer.OrdinalIgnoreCase);


if (args.Length != 1)
    throw new ArgumentNullException(nameof(args));

Writer.WriteToXML(commands, args.ElementAt(0));
