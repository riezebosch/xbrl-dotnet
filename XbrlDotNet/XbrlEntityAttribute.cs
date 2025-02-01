namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Property|AttributeTargets.Parameter)]
public class XbrlEntityAttribute(string scheme) : Attribute
{
    public string Scheme { get; } = scheme;
}