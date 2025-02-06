namespace XbrlDotNet;

public interface IReport
{
    IEnumerable<IContext> Contexts { get; }

    public interface WithPeriod : IReport, IPeriod;
    public interface WithInstant : IReport, IPeriodInstant;
}