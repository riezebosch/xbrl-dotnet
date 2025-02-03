using System.Reflection;
using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Converters;

internal class ContextConverter(Report report)
{
    public void Convert(object o)
    {
        report.Contexts.IdFormat = "c{0}d_0" + o.GetType().Name;
        var scenario = new Scenario(report);
        
        foreach (var attribute in o.GetType().GetCustomAttributes<XbrlExplicitMemberAttribute>())
        {
            scenario.AddExplicitMember(attribute.Dimension, attribute.Value);
        }

        var context = report.CreateContext(scenario);
        var provider = new PropertyAttributesProvider();
        foreach (var property in o.GetType().GetProperties())
        {
            var value = property.GetValue(o);
            foreach (var attribute in provider.For(property))
            {
                switch (attribute)
                {
                    case XbrlFactAttribute a:
                        report.AddFact(context, $"{a.Metric}:{property.Name}", a.UnitRef, a.Decimals, value?.ToString());
                        break;
                    case XbrlTypedMemberAttribute a:
                        context.Scenario.AddTypedMember(a.Dimension, a.Domain, value?.ToString());
                        break;
                    case XbrlEntityAttribute a:
                        context.Entity = new Entity(a.Scheme, value?.ToString());
                        break;
                    case XbrlPeriodStartAttribute:
                        context.Period.StartDate = (DateTime)value!;
                        break;
                    case XbrlPeriodEndAttribute:
                        context.Period.EndDate = (DateTime)value!;
                        break;
                }
            }
        }
    }
}