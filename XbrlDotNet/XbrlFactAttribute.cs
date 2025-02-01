namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class XbrlFactAttribute : Attribute
{
    public string? Metric { get; set; }
    public string? UnitRef { get; set; }
    public string? Decimals { get; set; }
}