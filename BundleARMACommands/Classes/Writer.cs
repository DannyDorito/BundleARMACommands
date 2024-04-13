namespace BundleARMACommands;

public static class Writer
{
    private const string KeywordPrepend = "\t\t<KeyWord name=\"";
    private const string KeywordAppend = "\" />";
#pragma warning disable CA1002 // Do not expose generic lists
    public static bool WriteToXML(List<string>? commands, string path)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        Console.WriteLine($"Reading existing .xml file from '{path}'");

        var finalCommands = new List<string>();
        var file = ReadXML(path, out var start, out var end);

        Console.WriteLine($"Writing {finalCommands.Count} to '{path}'");

        foreach (var command in commands)
            finalCommands.Add($"{KeywordPrepend}{command}{KeywordAppend}");

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

#pragma warning disable CA1002 // Do not expose generic lists
    public static List<string> ReadXML(string path, out int start, out int end)
#pragma warning restore CA1002 // Do not expose generic lists
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException($"File cannot be null or empty.", nameof(path));

        var file = File.ReadAllLines(path).ToList();

        start = file.FindIndex(line => line.StartsWith(KeywordPrepend, StringComparison.Ordinal));
        end = file.FindLastIndex(line => line.StartsWith(KeywordPrepend, StringComparison.Ordinal));

        if (start == -1 || end == -1)
            throw new ArgumentException("Error: Could not find start and end of keywords in file.", nameof(path));

        return file;
    }
}
