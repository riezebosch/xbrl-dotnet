using XbrlDotNet.Converters;
using Context = XbrlDotNet.Converters.Context;

namespace XbrlDotNet;

public static class XbrlConverter
{
    public static XDocument Convert(ITaxonomy taxonomy)
    {
        var report = new Report();

        ConvertTaxonomy(taxonomy, report);
        ConvertContexts(taxonomy, report);

        return report.ToXDocument();
    }

    private static void ConvertTaxonomy(ITaxonomy taxonomy, Report target)
    {
        var converter = new Taxonomy(target);
        converter.Convert(taxonomy);
    }

    private static void ConvertContexts(ITaxonomy taxonomy, Report target)
    {
        var converter = new Context(target);
        var contexts = taxonomy
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => x.GetValue(taxonomy))
            .ToList();
            
        foreach (var c in contexts.OfType<IContext>())
        {
            converter.Convert(c);
        }
        
        foreach (var c in contexts.OfType<IEnumerable<IContext>>().SelectMany(x => x))
        {
            converter.Convert(c);
        }
    }

    private static XDocument ToXDocument(this Report report)
    {
        using var reader = new XmlNodeReader(report.ToXmlDocument());
        return XDocument.Load(reader);
    }
}