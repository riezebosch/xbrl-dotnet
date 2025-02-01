using System;

namespace Xbrl.ToObjects;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlTypedDomainNamespaceAttribute(string prefix, string uri) : Attribute
{
    public string Prefix { get; } = prefix;
    public string Uri { get; } = uri;
}