using BundleARMACommands;

Console.WriteLine($"Starting {typeof(Program).Assembly.GetName().Name}.");

var commands = new List<string>();

foreach (var website in Scraper.WebsitesToScrape)
    commands.AddRange(await Scraper.GetData(website, CancellationToken.None).ConfigureAwait(true));

commands.Sort(StringComparer.OrdinalIgnoreCase);

Console.WriteLine($"{commands.Count} commands found.");

if (args.Length != 1)
    throw new ArgumentNullException(nameof(args));

Writer.WriteToXML(commands, args.ElementAt(0));
