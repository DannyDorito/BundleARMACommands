using BundleARMACommands.Enums;

namespace BundleARMACommands.Actions;

public static class Writer
{
    public static bool WriteXML(List<string> commands, string path, WriteType writingTo)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        return writingTo switch
        {
            WriteType.AutoComplete => WriteAutoComplete(commands, path),
            WriteType.SyntaxHighlighting => WriteSyntaxHiglighting(commands, path),
            _ => false,
        };
    }

    public static bool WriteAutoComplete(List<string>? commands, string path)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        var finalCommands = Scraper.FinaliseCommands(commands, path);

        Console.WriteLine($"Reading existing .xml file from '{path}'");

        var file = ReadXML(path, out var start, out var end);

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

    public static bool WriteSyntaxHiglighting(List<string>? commands, string path)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        Console.WriteLine($"Reading existing .xml file from '{path}'");
        return true;
    }

    public static List<string> ReadXML(string path, out int start, out int end)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException($"File cannot be null or empty.", nameof(path));

        var file = File.ReadAllLines(path).ToList();

        start = file.FindIndex(line => line.StartsWith(Scraper.KeywordPrepend, StringComparison.Ordinal));
        end = file.FindLastIndex(line => line.StartsWith(Scraper.KeywordPrepend, StringComparison.Ordinal));

        if (start == -1 || end == -1)
            throw new ArgumentException("Error: Could not find start and end of keywords in file.", nameof(path));

        return file;
    }
}
