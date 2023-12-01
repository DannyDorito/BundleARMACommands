namespace BundleARMACommands;

public class Website
{
    public Uri Uri { get; set; }
    public WebsiteType SiteType { get; set; }

    public bool Prepend => SiteType == WebsiteType.Commands;

    public Website(Uri uri, WebsiteType siteType)
    {
        Uri = uri;
        SiteType = siteType;
    }

    public string XPath()
    {
        return SiteType switch
        {
            WebsiteType.Commands => "//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]",
            WebsiteType.Functions => "//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]",
            WebsiteType.CBA => "//td[@class='IEntry']",
            _ => string.Empty,
        };
    }
}

public enum WebsiteType
{
    Commands,
    Functions,
    CBA
}
