namespace XbrlDotNet;

public interface IContext
{
    Entity Entity { get; }
    public interface WithPeriod : IContext, IPeriod;
    public interface WithInstant : IContext, IPeriodInstant;
}