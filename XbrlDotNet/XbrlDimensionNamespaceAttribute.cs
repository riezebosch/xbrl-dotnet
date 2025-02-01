namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlDimensionNamespaceAttribute(string prefix, string uri) : Attribute
{
    public string Prefix { get; } = prefix;
    public string Uri { get; } = uri;
}