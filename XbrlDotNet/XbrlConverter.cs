using XbrlDotNet.Converters;

namespace XbrlDotNet;

public static class XbrlConverter
{
    public static XDocument Convert(object data)
    {
        var report = new Report();

        var r = new ReportConverter(report);
        r.Convert(data);
        
        var c = new ContextsConverter(new ContextConverter(report));
        c.Convert(data);

        return report.ToXDocument();
    }

    private static XDocument ToXDocument(this Report report)
    {
        using var reader = new XmlNodeReader(report.ToXmlDocument());
        return XDocument.Load(reader);
    }
}