namespace XbrlDotNet.Tests;

public static class ExplicitMembersTests
{
    [XbrlExplicitMember("frc-vt-dim:ClientAxis", "frc-vt-dm:ClientMember")]
    private record TestContext;
    
    [XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
    [XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
    private record TestReport([XbrlContext] TestContext TestContext);

    [Fact]
    public static void AddExplicitMembers()
    {
        var report = XbrlDocument.For(new TestReport(new ()));
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "explicitMember").Which
            .Should().HaveValue("frc-vt-dm:ClientMember")
            .And.HaveAttribute("dimension", "frc-vt-dim:ClientAxis");
    }
}