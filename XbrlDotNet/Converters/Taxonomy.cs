namespace XbrlDotNet.Converters;

internal class Taxonomy(Report report)
{
    public void Convert(ITaxonomy instance)
    {
        report.Period = instance switch
        {
            IPeriod period => new Period(period.PeriodStart, period.PeriodEnd),
            IPeriodInstant instant => new Period(instant.Period),
            _ => report.Period
        };

        ApplyAttributes(instance);
    }

    private void ApplyAttributes(object data)
    {
        foreach (var attribute in data.GetType().GetCustomAttributes().OfType<IReportAttribute>())
        {
            attribute.Update(report);
        }
    }
}