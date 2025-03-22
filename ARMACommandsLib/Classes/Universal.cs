using ARMACommands;
using ARMACommands.Enums;
using System.Collections.ObjectModel;

namespace ARMACommands.Classes;

public static class Universal
{
    public static readonly Collection<Website> WebsitesToScrape =
    [
        new Website(new(Resources.ScriptingCommandsUrl), WebsiteType.Commands),
            new Website(new(Resources.FunctionsUrl), WebsiteType.Functions),
            new Website(new(Resources.CBAUrl), WebsiteType.CBA)
    ];

    public static ReadOnlyCollection<string> Filter => new(InternalFilter);

    private static List<string> InternalFilter => ["a != b", "! a", "a != b", "a % b", "a && b", "a &amp;&amp; b", "a * b", "a / b", "a : b", "a = b", "a == b", "a greater b", "a greater= b", "a hash b", "a less b", "a less= b", "a or b", "a ^ b", "+", "-", "User:OverlordZorn/Sandbox"];

    public static string[] Prepend() => Resources.Prepend.Split(',');

    public static string CBAAppend => Resources.CBAAppend;

    public const string KeywordPrepend = "\t\t<KeyWord name=\"";

    public const string KeywordAppend = "\" />";
}
