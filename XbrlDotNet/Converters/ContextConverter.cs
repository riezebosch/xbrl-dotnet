namespace XbrlDotNet.Converters;

internal class ContextConverter(Report report)
{
    public void Convert(object data)
    {
        var scenario = CreateScenario(data);
        report.Contexts.IdFormat = "c{0}d_0" + data.GetType().Name;

        var context = report.CreateContext(scenario);
        ApplyPropertyAttributes(context, data);
    }

    private Scenario CreateScenario(object data)
    {
        var scenario = new Scenario(report);
        var explicitMembers = data.GetType().GetCustomAttributes<XbrlExplicitMemberAttribute>();
        foreach (var member in explicitMembers)
        {
            scenario.AddExplicitMember(member.Dimension, member.Value);
        }
        return scenario;
    }

    private void ApplyPropertyAttributes(Context context, object data)
    {
        var properties = data.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(data);
            if (value != null)
            {
                ApplyPropertyAttributes(context, property, value);
            }
        }
    }

    private void ApplyPropertyAttributes(Context context, PropertyInfo property, object value)
    {
        var attributes = new PropertyAttributesProvider().For(property);
        foreach (var attribute in attributes)
        {
            ApplyPropertyAttribute(context, property, attribute, value);
        }
    }

    private void ApplyPropertyAttribute(Context context, PropertyInfo property, Attribute attribute, object value)
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
            case XbrlPeriodInstantAttribute:
                context.Period.Instant = (DateTime)value;
                break;
        }
    }
}