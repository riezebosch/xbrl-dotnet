namespace XbrlDotNet;

public interface IContext
{
    Entity Entity { get; }
    Period? Period { get; }
}