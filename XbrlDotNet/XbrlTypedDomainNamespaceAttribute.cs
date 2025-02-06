namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlTypedDomainNamespaceAttribute(string prefix, string uri) : Attribute, IReportAttribute
{
    void IReportAttribute.Update(Report report) => 
        report.SetTypedDomainNamespace(prefix, uri);
}