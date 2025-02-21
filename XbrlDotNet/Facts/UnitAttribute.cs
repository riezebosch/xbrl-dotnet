namespace XbrlDotNet.Facts;

/// <summary>
/// https://www.xbrl.org/utr/2017-07-12/utr.html
/// </summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class UnitAttribute(string unitId, string nsUnit) : Attribute
{
    public void Update(Fact fact) => fact.Unit = new Unit(unitId, nsUnit);
}