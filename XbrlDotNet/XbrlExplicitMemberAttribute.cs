namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlExplicitMemberAttribute(string dimension, string value) : Attribute
{
    public string Dimension { get; } = dimension;
    public string Value { get; } = value;
}