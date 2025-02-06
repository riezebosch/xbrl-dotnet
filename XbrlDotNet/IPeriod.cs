namespace XbrlDotNet;

public interface IPeriod
{
    DateTime PeriodStart { get; }
    DateTime PeriodEnd { get; }
}