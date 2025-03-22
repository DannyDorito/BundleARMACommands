using ARMACommands;
using ARMACommands.Enums;

namespace ARMACommands.Classes;

public class Website(Uri uri, WebsiteType siteType)
{
    public Uri Uri { get; set; } = uri;
    public WebsiteType SiteType { get; set; } = siteType;

    public bool Prepend => SiteType == WebsiteType.Commands;

    public string XPath => SiteType switch
    {
        WebsiteType.Commands => Resources.ScriptingCommandsRegex,
        WebsiteType.Functions => Resources.FunctionsRegex,
        WebsiteType.CBA => Resources.CBARegex,
        _ => string.Empty,
    };
}

