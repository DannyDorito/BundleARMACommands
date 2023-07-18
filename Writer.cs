using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace BundleARMACommands;

public static class Writer
{

    public static void WriteToXML(ReadOnlyCollection<string> commands, string file)
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
        var readerSettings = new XmlReaderSettings
        {
            IgnoreComments = true
        };
        using (var reader = XmlReader.Create(file, readerSettings))
        {
            var myData = new XmlDocument();
            myData.Load(reader);

            var x = myData.ChildNodes[1]!.ChildNodes[0]!;
            foreach (XmlNode x2 in x.ChildNodes)
            {
                if (x2.Name != "Environment")
                {
                    existingKeyWords.Add(x2.Attributes?.GetNamedItem("name")?.Value!);
                }
            }
        }

        var toWrite = commands.Where(c => !existingKeyWords.Any(kw => c != kw)).ToList();

        using XmlWriter writer = XmlWriter.Create(file, new XmlWriterSettings { });

    }
}
