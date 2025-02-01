using System;

namespace Xbrl.ToObjects;

public class XbrlEntityAttribute(string scheme) : Attribute
{
    public string Scheme { get; } = scheme;
}