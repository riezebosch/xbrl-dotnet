namespace XbrlDotNet.Tests;

public static class ExplicitMembersTests
{
    [Fact]
    public static void AddExplicitMembers()
    {
        var report = XbrlConverter.Convert(new TestTaxonomy(new TestContext()));
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "explicitMember").Which
            .Should().HaveValue("frc-vt-dm:ClientMember")
            .And.HaveAttribute("dimension", "frc-vt-dim:ClientAxis");
    }

    [XbrlExplicitMember("frc-vt-dim:ClientAxis", "frc-vt-dm:ClientMember")]
    private record TestContext : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
    }

    [XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
    [XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
    private record TestTaxonomy(TestContext TestContext) : ITaxonomy
    {
        public IEnumerable<IContext> Contexts => [TestContext];
    }
}