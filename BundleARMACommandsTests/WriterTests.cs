using ARMACommands.Actions;
using ARMACommands.Classes;
using ARMACommands.Enums;

namespace BundleARMACommandsTests;

[TestFixture]
public class WriterTests
{
    private const string TestFilePath = "test.xml";

    [SetUp]
    public void Setup()
    {
        // Create a test file
        File.WriteAllLines(TestFilePath, new List<string>
            {
                "<root>",
                $"{Universal.KeywordPrepend}command1{Universal.KeywordAppend}",
                $"{Universal.KeywordPrepend}command2{Universal.KeywordAppend}",
                "</root>"
            });
    }

    [TearDown]
    public void Teardown()
    {
        // Delete the test file after each test
        File.Delete(TestFilePath);
    }

    [TestCase(FileType.AutoComplete)]
    [TestCase(FileType.SyntaxHighlighting)]
    public void ReadXML_NullOrEmptyPath_ThrowsArgumentException(FileType type)
    {
        Assert.Throws<ArgumentException>(() => Writer.ReadXml(string.Empty, type));
    }

    [TestCase(FileType.AutoComplete)]
    [TestCase(FileType.SyntaxHighlighting)]
    public void ReadXML_ValidPath_ReturnsFileLines(FileType type)
    {

        if (type == FileType.AutoComplete)
        {
            var fileLines = Writer.ReadXml(TestFilePath, type);
            Assert.Multiple(() =>
            {
                Assert.That(fileLines.Item2, Is.EqualTo(1));
                Assert.That(fileLines.Item3, Is.EqualTo(2));
                Assert.That(fileLines.Item1, Has.Count.EqualTo(4));
            });
        }
        else
        {
            Assert.Throws<NotImplementedException>(() => Writer.ReadXml(TestFilePath, type));
        }
    }

    [TestCase(FileType.AutoComplete)]
    [TestCase(FileType.SyntaxHighlighting)]
    public void ReadXML_InvalidKeywordLines_ThrowsArgumentException(FileType type)
    {
        // Create a file without keyword lines
        File.WriteAllLines(TestFilePath, new List<string> { "<root>", "</root>" });
        Assert.Throws<ArgumentException>(() => Writer.ReadXml(string.Empty, type));
    }

    [Test]
    public void WriteXML_NullCommands()
    {
        Assert.Throws<ArgumentNullException>(() => Writer.WriteAutoComplete(null, ""));
    }

    [Test]
    public void Write_XML()
    {
        var commands = new List<string>() { "Test" };
        var result = Writer.WriteAutoComplete(commands, TestFilePath);

        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void Write_XML_EmptyCommands()
    {
        var result = Writer.WriteAutoComplete([], TestFilePath);

        Assert.That(result, Is.EqualTo(true));
    }
}