namespace XbrlDotNet.Converters;

internal class Taxonomy(Report report)
{
    public void Convert(ITaxonomy instance)
    {
        report.Period = instance switch
        {
            IPeriodDuration period => new Period(period.Start, period.End),
            IPeriodInstant instant => new Period(instant.Instant),
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