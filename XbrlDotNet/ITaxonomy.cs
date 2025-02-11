namespace XbrlDotNet;

public interface ITaxonomy
{
    IEnumerable<IContext> Contexts { get; }

    public interface PeriodDuration : ITaxonomy, IPeriodDuration;
    public interface PeriodInstant : ITaxonomy, IPeriodInstant;
}