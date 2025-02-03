using System.Reflection;
using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Converters;

internal class ReportConverter(Report report)
{
    public void Convert(object data)
    {
        FromAttributes(data);
        FromProperties(data);
    }

    private void FromAttributes(object data)
    {
        foreach (var attribute in data.GetType().GetCustomAttributes())
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
    }

    private void FromProperties(object data)
    {
        var provider = new PropertyAttributesProvider();
        foreach (var property in data.GetType().GetProperties())
        {
            var value = property.GetValue(data)!;
            foreach (var attr in provider.For(property))
            {
                switch (attr)
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
    }
}