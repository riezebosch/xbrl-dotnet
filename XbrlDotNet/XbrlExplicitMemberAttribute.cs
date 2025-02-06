namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlExplicitMemberAttribute(string dimension, string value) : Attribute
{
    public void Update(Scenario scenario) => 
        scenario.AddExplicitMember(dimension, value);
}