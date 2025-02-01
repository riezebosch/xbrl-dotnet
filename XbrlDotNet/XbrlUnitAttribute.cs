namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class)]
public class XbrlUnitAttribute(string id, string value) : Attribute
{
    public string Id { get; } = id;
    public string Value { get; } = value;
}