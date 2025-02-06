namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class XbrlTypedMemberAttribute(string dimension, string domain) : Attribute
{
    public void Update(Context context, string name, object value) =>
        context.Scenario.AddTypedMember(dimension, $"{domain}:{name}", value.ToString());
}