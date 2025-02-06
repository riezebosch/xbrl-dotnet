namespace XbrlDotNet;

public interface IReport
{
    Period? Period => null;
    IEnumerable<IContext> Contexts { get; }
}