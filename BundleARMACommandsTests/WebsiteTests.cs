using BundleARMACommands.Classes;

namespace BundleARMACommandsTests;

[TestFixture]
public class WebsiteTests
{
    private Website _website;

    [SetUp]
    public void Setup()
    {
        _website = new Website(new Uri("http://example.com"), WebsiteType.Commands);
    }

    [Test]
    public void Constructor_SetsUriAndSiteType()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_website.Uri, Is.EqualTo(new Uri("http://example.com")));
            Assert.That(_website.SiteType, Is.EqualTo(WebsiteType.Commands));
        });
    }

    [Test]
    public void Prepend_ReturnsTrueForCommands()
    {
        Assert.That(_website.Prepend, Is.True);
    }

    [Test]
    public void Prepend_ReturnsFalseForFunctionsAndCBA()
    {
        _website.SiteType = WebsiteType.Functions;
        Assert.That(_website.Prepend, Is.False);

        _website.SiteType = WebsiteType.CBA;
        Assert.That(_website.Prepend, Is.False);
    }

    [Test]
    public void XPath_ReturnsCorrectValueForCommandsAndFunctions()
    {
        string expectedXPath = "//div[contains(@id, 'mw-pages')]//li[not(contains(@class, 'bohemia-header-nav-item'))]";
        Assert.That(_website.XPath, Is.EqualTo(expectedXPath));

        _website.SiteType = WebsiteType.Functions;
        Assert.That(_website.XPath, Is.EqualTo(expectedXPath));
    }

    [Test]
    public void XPath_ReturnsCorrectValueForCBA()
    {
        _website.SiteType = WebsiteType.CBA;
        string expectedXPath = "//td[@class='IEntry']";
        Assert.That(_website.XPath, Is.EqualTo(expectedXPath));
    }

    [Test]
    public void XPath_ReturnsEmptyStringForUnknownWebsiteType()
    {
        _website.SiteType = (WebsiteType)999; // Unknown WebsiteType
        Assert.That(_website.XPath, Is.EqualTo(string.Empty));
    }
}