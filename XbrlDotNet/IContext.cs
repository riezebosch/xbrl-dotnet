using XbrlDotNet.Period;
using ExplicitMember = XbrlDotNet.Dimensions.ExplicitMember;
using TypedMember = XbrlDotNet.Dimensions.TypedMember;

namespace XbrlDotNet;

public interface IContext
{
    IEntity Entity { get; }
    ExplicitMember[] ExplicitMembers { get; }
    TypedMember[] TypedMembers { get; }

    public interface PeriodDuration : IContext, IPeriodDuration;
    public interface PeriodInstant : IContext, IPeriodInstant;
}