using BundleARMACommands;

namespace BundleARMACommandsTests;

[TestFixture]
public class WriterTests
{
    private const string TestFilePath = "test.xml";
    private const string KeywordPrepend = "\t\t<KeyWord name=\"";
    private const string KeywordAppend = "\" />";

    [SetUp]
    public void Setup()
    {
        // Create a test file
        File.WriteAllLines(TestFilePath, new List<string>
            {
                "<root>",
                $"{KeywordPrepend}command1{KeywordAppend}",
                $"{KeywordPrepend}command2{KeywordAppend}",
                "</root>"
            });
    }

    [TearDown]
    public void Teardown()
    {
        // Delete the test file after each test
        File.Delete(TestFilePath);
    }

    [Test]
    public void ReadXML_NullOrEmptyPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Writer.ReadXML(string.Empty, out _, out _));
    }

    [Test]
    public void ReadXML_ValidPath_ReturnsFileLines()
    {
        var fileLines = Writer.ReadXML(TestFilePath, out var start, out var end);
      
        Assert.Multiple(() =>
        {
            Assert.That(start, Is.EqualTo(1));
            Assert.That(end, Is.EqualTo(2));
            Assert.That(fileLines, Has.Count.EqualTo(4));
        });
    }

    [Test]
    public void ReadXML_InvalidKeywordLines_ThrowsArgumentException()
    {
        // Create a file without keyword lines
        File.WriteAllLines(TestFilePath, new List<string> { "<root>", "</root>" });

        Assert.Throws<ArgumentException>(() => Writer.ReadXML(TestFilePath, out _, out _));
    }

    [Test]
    public void WriteXML_NullCommands()
    {
        Assert.Throws<ArgumentNullException>(() => Writer.WriteToXML(null, ""));
    }

    [Test]
    public void Write_XML()
    {
        var commands = new List<string>() { "Test" };
        var result = Writer.WriteToXML(commands, TestFilePath);

        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void Write_XML_EmptyCommands()
    {
        var result = Writer.WriteToXML([], TestFilePath);

        Assert.That(result, Is.EqualTo(true));
    }
}