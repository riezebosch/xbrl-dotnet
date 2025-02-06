using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

internal record TestReport(params IContext[] TestContext) : IReport
{
    public IEnumerable<IContext> Contexts => TestContext;
}