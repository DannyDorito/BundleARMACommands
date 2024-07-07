using BundleARMACommands.Enums;
using System.Collections.ObjectModel;

namespace BundleARMACommands.Classes
{
    public static class Common
    {
        public static readonly Collection<Website> WebsitesToScrape =
        [
            new Website(new(Resources.ScriptingCommandsUrl), WebsiteType.Commands),
                new Website(new(Resources.FunctionsUrl), WebsiteType.Functions),
                new Website(new(Resources.CBAUrl), WebsiteType.CBA)
        ];

        private static ReadOnlyCollection<string> InternalFilter()
        {
            var filterArray = Resources.Filter.Split(',');
            var filterList = new List<string>();
            foreach (var filter in filterArray)
            {
                filterList.Add(filter.Replace("amp;", "").Replace("_", " "));
            }
            return filterList.AsReadOnly();
        }

        public static ReadOnlyCollection<string> Filter => InternalFilter();
        public static string[] Prepend => Resources.Prepend.Split(',');

        public static string CBAAppend => Resources.CBAAppend;

        public const string KeywordPrepend = "\t\t<KeyWord name=\"";

        public const string KeywordAppend = "\" />";
    }
}
