namespace XbrlDotNet;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class XbrlDimensionNamespaceAttribute(string prefix, string uri) : Attribute, IReportAttribute
{
    void IReportAttribute.Update(Report report) => 
        report.SetDimensionNamespace(prefix, uri);
}