namespace XbrlDotNet.Facts;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class DecimalsAttribute(string value) : Attribute
{
    public DecimalsAttribute(int value) : this(value.ToString()) { }

    public void Update(Fact fact) => fact.Decimals = value;
}