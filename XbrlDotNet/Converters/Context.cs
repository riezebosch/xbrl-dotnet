using XbrlDotNet.Facts;
using XbrlDotNet.Period;

namespace XbrlDotNet.Converters;

internal class Context(Report report)
{
    public void Convert(IContext data)
    {
        var scenario = new Scenario(report);
        report.Contexts.IdFormat = "c{0}d_0" + data.GetType().Name;
        ConvertDimensions(scenario, data);

        var context = report.CreateContext(scenario);
        context.Entity = new Entity(data.Entity.Scheme, data.Entity.Value);
        
        SetContextPeriod(context, data);
        ConvertProperties(context, data);
    }

    private void ConvertDimensions(Scenario scenario, IContext data)
    {
        foreach (var member in data.TypedMembers)
        {
            scenario.AddTypedMember(ToXmlNameWithPrefix(member.Dimension), ToXmlNameWithPrefix(member.Domain), member.Value);
        }

        foreach (var member in data.ExplicitMembers)
        {
            scenario.AddExplicitMember(ToXmlNameWithPrefix(member.Dimension), ToXmlNameWithPrefix(member.Domain));
        }
    }

    private static void SetContextPeriod(Diwen.Xbrl.Xml.Context context, IContext data) =>
        context.Period = data switch
        {
            IPeriodDuration period => new Diwen.Xbrl.Xml.Period(period.Start, period.End),
            IPeriodInstant instant => new Diwen.Xbrl.Xml.Period(instant.Instant),
            _ => context.Period
        };

    private void ConvertProperties(Diwen.Xbrl.Xml.Context context, object data)
    {
        var properties = data.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(data);
            if (value != null)
            {
                ConvertProperty(context, property, value);
            }
        }
    }

    private void ConvertProperty(Diwen.Xbrl.Xml.Context context, PropertyInfo property, object value)
    {
        var f = new Fact
        {
            Context = context,
            Value = value.ToString()
        };

        var attributes = new PropertyAttributesProvider().For(property);
        foreach (var attribute in attributes)
        {
            switch (attribute)
            {
                case ConceptAttribute c:
                    c.Update(f, report, property.Name);
                    break;
                case DecimalsAttribute d:
                    d.Update(f);
                    break;
                case UnitAttribute u:
                    u.Update(f);
                    break;
            }
        }
    }

    private string ToXmlNameWithPrefix(XName name) => 
        report.Namespaces.LookupPrefix(name.NamespaceName) + ":"+ name.LocalName;
}