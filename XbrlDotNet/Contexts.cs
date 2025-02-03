using System.Collections;
using System.Linq;

namespace XbrlDotNet;

internal class Contexts(Diwen.Xbrl.Xml.Report report)
{
    public static Contexts For(Diwen.Xbrl.Xml.Report report) => new(report);

    public void Add(object data)
    {
        var provider = new PropertyAttributesProvider();
        foreach (var property in data
                     .GetType()
                     .GetProperties()
                     .Where(x => provider.For(x).OfType<XbrlContextAttribute>().Any()))
        {
            var value = property.GetValue(data)!;
            AddContexts(value);
        }
    }

    private void AddContexts(object item)
    {
        var c = Context.For(report);
        if (item is IEnumerable children)
        {
            foreach (var child in children)
            {
                c.Add(child);
            }
        }
        else
        {
            c.Add(item);
        }
    }
}