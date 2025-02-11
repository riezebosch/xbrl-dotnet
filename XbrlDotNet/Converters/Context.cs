namespace XbrlDotNet.Converters;

internal class Context(Report report)
{
    public void Convert(IContext data)
    {
        var scenario = CreateScenario(data);
        report.Contexts.IdFormat = "c{0}d_0" + data.GetType().Name;

        var context = report.CreateContext(scenario);
        context.Entity = new Entity(data.Entity.Scheme, data.Entity.Value);

        SetContextPeriod(context, data);
        ConvertProperties(context, data);
    }

    private static void SetContextPeriod(Diwen.Xbrl.Xml.Context context, IContext data) =>
        context.Period = data switch
        {
            IPeriodDuration period => new Period(period.Start, period.End),
            IPeriodInstant instant => new Period(instant.Instant),
            _ => context.Period
        };

    private Scenario CreateScenario(object data)
    {
        var scenario = new Scenario(report);
        foreach (var member in data.GetType().GetCustomAttributes<XbrlExplicitMemberAttribute>())
        {
            member.Update(scenario);
        }

        return scenario;
    }

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
                case XbrlTypedMemberAttribute t:
                    t.Update(context, property.Name, value);
                    break;
            }
        }
    }
}