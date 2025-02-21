using XbrlDotNet.Period;

namespace XbrlDotNet;

public interface ITaxonomy
{
    NamespacePrefix Domain { get; }
    NamespacePrefix Dimension { get; }

    public interface PeriodDuration : ITaxonomy, IPeriodDuration;
    public interface PeriodInstant : ITaxonomy, IPeriodInstant;
}