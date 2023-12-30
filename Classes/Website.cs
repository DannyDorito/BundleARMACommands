namespace BundleARMACommands.Classes;

public class Website(Uri uri, WebsiteType siteType)
{
    public Uri Uri { get; set; } = uri;
    public WebsiteType SiteType { get; set; } = siteType;

    public bool Prepend => SiteType == WebsiteType.Commands;

    public string XPath => SiteType switch
    {
        WebsiteType.Commands => "//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]",
        WebsiteType.Functions => "//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]",
        WebsiteType.CBA => "//td[@class='IEntry']",
        _ => string.Empty,
    };
}

public enum WebsiteType
{
    Commands,
    Functions,
    CBA
}
