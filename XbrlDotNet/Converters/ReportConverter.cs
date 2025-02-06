namespace XbrlDotNet.Converters;

internal class ReportConverter(Report report)
{
    public void Convert(IReport data)
    {
        report.Period = data.Period;
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