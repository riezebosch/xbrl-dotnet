namespace XbrlDotNet;

public interface IContext
{
    IEntity Entity { get; }
    public interface WithPeriod : IContext, IPeriod;
    public interface WithInstant : IContext, IPeriodInstant;
}

public interface IEntity
{
    string Value { get; }
    string Scheme { get; }
}