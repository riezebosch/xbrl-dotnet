namespace XbrlDotNet.Converters;

internal class ContextsConverter(ContextConverter context)
{
    public void Convert(object data)
    {
        var properties = GetContextProperties(data);
        foreach (var property in properties)
        {
            var value = property.GetValue(data);
            if (value != null)
            {
                AddContexts(value);
            }
        }
    }

    private IEnumerable<PropertyInfo> GetContextProperties(object data)
    {
        var provider = new PropertyAttributesProvider();
        return data.GetType()
                   .GetProperties()
                   .Where(p => provider.For(p).OfType<XbrlContextAttribute>().Any());
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
            context.Convert(value);
        }
    }
}