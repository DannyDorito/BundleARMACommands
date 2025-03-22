using ARMACommands.Classes;
using ARMACommands.Enums;

namespace ARMACommands.Actions;

public static class Writer
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<Pending>")]
    public static bool WriteXML(List<string> commands, string path, FileType type)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        return type switch
        {
            FileType.AutoComplete => WriteAutoComplete(commands, path),
            FileType.SyntaxHighlighting => WriteSyntaxHiglighting(commands, path),
            _ => false,
        };
    }

    public static Tuple<List<string>, int, int> ReadXml(string path, FileType type)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException($"File cannot be null or empty.", nameof(path));

        return type switch
        {
            FileType.AutoComplete => ReadXmlAutoComplete(path),
            FileType.SyntaxHighlighting => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<Pending>")]
    public static bool WriteAutoComplete(List<string>? commands, string path)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        var finalCommands = Scraper.FinaliseCommands(commands);

        Console.WriteLine($"Reading existing .xml file from '{path}'");

        var fileTuple = ReadXml(path, FileType.AutoComplete);
        var file = fileTuple.Item1;
        var start = fileTuple.Item2;
        var end = fileTuple.Item3;

        Console.WriteLine($"Writing {finalCommands.Count} to '{path}'");

        var hasChanged = end - start + 1 != finalCommands.Count;

        if (hasChanged)
        {
            file.RemoveRange(start, end - start + 1);
            file.InsertRange(start, finalCommands);

            File.WriteAllLines(path, file);
        }
        Console.WriteLine($"Changed {finalCommands.Count - (end - start + 1)} lines");
        return hasChanged;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "<Pending>")]
    public static bool WriteSyntaxHiglighting(List<string>? commands, string path)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        Console.WriteLine($"Reading existing .xml file from '{path}'");
        return true;
    }

    public static Tuple<List<string>, int, int> ReadXmlAutoComplete(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException($"File cannot be null or empty.", path);

        var file = File.ReadAllLines(path).ToList();

        var start = file.FindIndex(line => line.StartsWith(Universal.KeywordPrepend, StringComparison.Ordinal));
        var end = file.FindLastIndex(line => line.StartsWith(Universal.KeywordPrepend, StringComparison.Ordinal));

        if (start == -1 || end == -1)
            throw new ArgumentException("Error: Could not find start and end of keywords in file.", path);

        return Tuple.Create(file, start, end);
    }
}
