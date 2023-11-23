using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace BundleARMACommands;

public static class Writer
{
    public static void WriteToXML(List<string> commands, string file)
    {
        if (commands is null)
        {
            throw new ArgumentNullException(nameof(commands));
        }

        if (string.IsNullOrEmpty(file))
        {
            throw new ArgumentException($"'{nameof(file)}' cannot be null or empty.", nameof(file));
        }

        EncodingProvider ppp = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);

        var existingKeyWords = new List<string>();

        var toWrite = commands.Where(c => !existingKeyWords.Any(kw => c != kw)).ToList();

        var fileToWrite = File.ReadAllLines(file).ToList();

        var start = fileToWrite.FindIndex(f => f.StartsWith("\t\t<KeyWord name=\"", StringComparison.Ordinal));
        var end = fileToWrite.FindLastIndex(f => f.StartsWith("\t\t<KeyWord name=\"", StringComparison.Ordinal));

        fileToWrite.RemoveRange(start, end - start + 1);

        fileToWrite.InsertRange(start, commands);

        File.WriteAllLines(file, fileToWrite);
    }

    [Obsolete]
    public static void WriteHacky(List<string> commands)
    {
        if (commands is null)
        {
            throw new ArgumentNullException(nameof(commands));
        }
        var hackyCommands = new List<string>();

        foreach (var c in commands)
        {
            hackyCommands.Add($"\t\t<KeyWord name=\"{c}\" />");
        }

        var hackyCommandsStr = string.Join("\r\n", hackyCommands);

        File.WriteAllText(@$"{Environment.CurrentDirectory}\output.xml", hackyCommandsStr);
    }
}
