namespace XbrlDotNet.Converters;

internal class ReportConverter(Report report)
{
    public void Convert(IReport data)
    {
        report.Period = data switch
        {
            IPeriod period => new Period(period.PeriodStart, period.PeriodEnd),
            IPeriodInstant instant => new Period(instant.Period),
            _ => report.Period
        };

        ApplyAttributes(data);
    }

    private void ApplyAttributes(object data)
    {
        foreach (var attribute in data.GetType().GetCustomAttributes().OfType<IReportAttribute>())
        {
            attribute.Update(report);
        }
    }
}