using XbrlDotNet.Converters;

namespace XbrlDotNet;

public static class XbrlConverter
{
    public static XDocument Convert(IReport report)
    {
        var target = new Report();

        var r = new ReportConverter(target);
        r.Convert(report);
        
        var context = new ContextConverter(target);
        foreach (var c in report.Contexts)
        {
            context.Convert(c);
        }

        return target.ToXDocument();
    }

    private static XDocument ToXDocument(this Report report)
    {
        using var reader = new XmlNodeReader(report.ToXmlDocument());
        return XDocument.Load(reader);
    }
}