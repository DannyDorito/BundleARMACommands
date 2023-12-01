namespace BundleARMACommands;

public static class Writer
{
    private const string KeywordPrepend = "\t\t<KeyWord name=\"";
    private const string KeywordAppend = "\" />";
    public static void WriteToXML(List<string> commands, string file)
    {
        if (commands is null)
            throw new ArgumentNullException(nameof(commands));

        if (string.IsNullOrEmpty(file))
            throw new ArgumentException($"File cannot be null or empty.", nameof(file));

        var fileToWrite = File.ReadAllLines(file).ToList();

        var start = fileToWrite.FindIndex(f => f.StartsWith(KeywordPrepend, StringComparison.Ordinal));
        var end = fileToWrite.FindLastIndex(f => f.StartsWith(KeywordPrepend, StringComparison.Ordinal));

        if (start == -1 || end == -1)
            throw new ArgumentException("Error: Could not find start and end of keywords in file", nameof(file));

        var finalCommands = new List<string>();
        foreach (var command in commands)
            finalCommands.Add($"{KeywordPrepend}{command}{KeywordAppend}");

        fileToWrite.RemoveRange(start, end - start + 1);
        fileToWrite.InsertRange(start, finalCommands);

        File.WriteAllLines(file, fileToWrite);
    }
}
