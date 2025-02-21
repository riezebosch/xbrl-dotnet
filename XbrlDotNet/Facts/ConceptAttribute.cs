namespace XbrlDotNet.Facts;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class ConceptAttribute(string prefix, string ns) : Attribute
{
    public void Update(Fact fact, Report report, string name)
    {
        report.Facts.Add(fact);    
        report.Namespaces.AddNamespace(prefix, ns);
        fact.Metric = new XmlQualifiedName($"{prefix}:{name}", ns);
    }
}