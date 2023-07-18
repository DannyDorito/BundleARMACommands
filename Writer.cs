using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BundleARMACommands;

public static class Writer
{

    public static async Task WriteToXML(List<string> commands, string file)
    {
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
            foreach ( XmlNode x2 in x.ChildNodes) {
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
