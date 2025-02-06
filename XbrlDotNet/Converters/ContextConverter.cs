namespace XbrlDotNet.Converters;

internal class ContextConverter(Report report)
{
    public void Convert(IContext data)
    {
        var scenario = CreateScenario(data);
        report.Contexts.IdFormat = "c{0}d_0" + data.GetType().Name;

        var context = report.CreateContext(scenario);
        context.Entity = data.Entity;

        context.Period = data switch
        {
            IPeriod period => new Period(period.PeriodStart, period.PeriodEnd),
            IPeriodInstant instant => new Period(instant.Period),
            _ => context.Period
        };

        ConvertProperties(context, data);
    }

    private Scenario CreateScenario(object data)
    {
        var scenario = new Scenario(report);
        foreach (var member in data.GetType().GetCustomAttributes<XbrlExplicitMemberAttribute>())
        {
            member.Update(scenario);
        }

        return scenario;
    }

    private void ConvertProperties(Context context, object data)
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

    private void ConvertProperty(Context context, PropertyInfo property, object value)
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