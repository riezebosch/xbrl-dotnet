using XbrlDotNet.Converters;
using Context = XbrlDotNet.Converters.Context;

namespace XbrlDotNet;

public static class XbrlConverter
{
    public static XDocument Convert(ITaxonomy taxonomy)
    {
        var target = new Report();

        var r = new Taxonomy(target);
        r.Convert(taxonomy);
        
        var context = new Context(target);
        foreach (var c in taxonomy.Contexts)
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