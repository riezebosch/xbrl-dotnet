using System.Xml;
using System.Xml.Linq;

namespace XbrlDotNet;

public static class XbrlDocument
{
    public static XDocument For(object data)
    {
        var report = new Diwen.Xbrl.Xml.Report();

        var r = Report.For(report);
        r.Add(data);
        
        var c = Contexts.For(report);
        c.Add(data);

        return report.ToXDocument();
    }

    private static XDocument ToXDocument(this Diwen.Xbrl.Xml.Report report)
    {
        using var nodeReader = new XmlNodeReader(report.ToXmlDocument());
        var doc = XDocument.Load(nodeReader);

        doc.Root!.Add(new XAttribute(XNamespace.Xml + "lang", "nl"));

        // xdoc.Descendants()
        //     .Attributes()
        //     .Where(a => a.IsNamespaceDeclaration && a.Value == xsiNamespace)
        //     .Remove();

        return doc;
    }
}