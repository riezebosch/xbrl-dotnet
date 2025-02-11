namespace XbrlDotNet;

public interface IContext
{
    IEntity Entity { get; }
    public interface PeriodDuration : IContext, IPeriodDuration;
    public interface PeriodInstant : IContext, IPeriodInstant;
}