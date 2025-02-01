namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class XbrlTypedMemberAttribute(string dimension, string domain) : Attribute
{
    public string Dimension { get; } = dimension;
    public string Domain { get; } = domain;
}