using ARMACommands.Actions;
using ARMACommands.Classes;
using ARMACommands.Enums;

namespace BundleARMACommandsTests;

[TestFixture]
public class ScraperTests
{
    private Website _websiteCommands;
    private Website _websiteFunctions;
    private Website _websiteCBA;

    [SetUp]
    public void Setup()
    {
        _websiteCommands = new Website(new Uri("https://community.bistudio.com/wiki/Category:Scripting_Commands"), WebsiteType.Commands);
        _websiteFunctions = new Website(new Uri("https://community.bistudio.com/wiki/Category:Functions"), WebsiteType.Functions);
        _websiteCBA = new Website(new Uri("https://cbateam.github.io/CBA_A3/docs/index/Functions.html"), WebsiteType.CBA);
    }

    [Test]
    public async Task GetData_WithCommandsWebsite_ReturnsData()
    {
        var result = await Scraper.GetData(_websiteCommands, CancellationToken.None);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        });
    }

    [Test]
    public void GetData_WebsiteNull_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await Scraper.GetData(null, CancellationToken.None));
    }

    [Test]
    public async Task GetData_WithFunctionsWebsite_ReturnsData()
    {
        var result = await Scraper.GetData(_websiteFunctions, CancellationToken.None);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        });
    }

    [Test]
    public async Task GetData_WithCBAWebsite_ReturnsData()
    {
        var result = await Scraper.GetData(_websiteCBA, CancellationToken.None);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
        });
    }
}