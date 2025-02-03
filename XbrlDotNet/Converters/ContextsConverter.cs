using System.Collections;
using System.Linq;

namespace XbrlDotNet.Converters;

internal class ContextsConverter(ContextConverter converter)
{
    public void Convert(object data)
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

    private void AddContexts(object value)
    {
        if (value is IEnumerable items)
        {
            foreach (var item in items)
            {
                AddContexts(item);
            }
        }
        else
        {
            converter.Convert(value);
        }
    }
}