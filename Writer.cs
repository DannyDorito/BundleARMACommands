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
        System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);

        var keyWords = new List<string>();

        var readerSettings = new XmlReaderSettings
        {
            IgnoreComments = true
        };
        using (XmlReader reader = XmlReader.Create(file, readerSettings))
        {
            var myData = new XmlDocument();
            myData.Load(reader);

            var x = myData.ChildNodes;
            foreach ( var x2 in x) {
                var xx = x2.ToString();
            }
            // etc...
        }

        //foreach (XElement level1Element in XElement.Load(file, readerSettings).Elements("NotepadPlus"))
        //{
        //    keyWords.Add(level1Element.Attribute("name").Value);
        //}

        var toWrite = commands.Where(c => !keyWords.Any(kw => c != kw)).ToList();

    }
}
