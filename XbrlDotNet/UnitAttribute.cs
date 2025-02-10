namespace XbrlDotNet;

public abstract class UnitAttribute(string id, string value) : Attribute
{
    public void Update(Fact fact) => fact.Unit = new Unit(id, value);
}