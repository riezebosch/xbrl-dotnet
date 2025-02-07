namespace XbrlDotNet;

public interface ITaxonomy
{
    IEnumerable<IContext> Contexts { get; }

    public interface WithPeriod : ITaxonomy, IPeriod;
    public interface WithInstant : ITaxonomy, IPeriodInstant;
}