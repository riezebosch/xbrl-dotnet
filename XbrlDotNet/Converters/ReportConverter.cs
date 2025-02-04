namespace XbrlDotNet.Converters;

internal class ReportConverter(Report report)
{
    public void Convert(object data)
    {
        ConvertAttributes(data);
        ConvertProperties(data);
    }

    private void ConvertAttributes(object data)
    {
        var attributes = data.GetType().GetCustomAttributes();
        foreach (var attribute in attributes)
        {
            ApplyAttribute(attribute);
        }
    }

    private void ApplyAttribute(Attribute attribute)
    {
        switch (attribute)
        {
            case XbrlTypedDomainNamespaceAttribute a:
                report.SetTypedDomainNamespace(a.Prefix, a.Uri);
                break;
            case XbrlDimensionNamespaceAttribute a:
                report.SetDimensionNamespace(a.Prefix, a.Uri);
                break;
            case XbrlUnitAttribute a:
                report.Units.Add(a.Id, a.Value);
                break;
        }
    }

    private void ConvertProperties(object data)
    {
        var properties = data.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(data);
            if (value != null)
            {
                ApplyPropertyAttributes(property, value);
            }
        }
    }

    private void ApplyPropertyAttributes(PropertyInfo property, object value)
    {
        var attributes = new PropertyAttributesProvider().For(property);
        foreach (var attribute in attributes)
        {
            ApplyPropertyAttribute(attribute, value);
        }
    }

    private void ApplyPropertyAttribute(Attribute attribute, object value)
    {
        switch (attribute)
        {
            case XbrlPeriodStartAttribute:
                report.Period.StartDate = (DateTime)value;
                break;
            case XbrlPeriodEndAttribute:
                report.Period.EndDate = (DateTime)value;
                break;
        }
    }
}