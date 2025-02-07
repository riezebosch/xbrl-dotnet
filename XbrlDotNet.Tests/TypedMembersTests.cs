namespace XbrlDotNet.Tests;

public static class TypedMembersTests
{
    [Fact]
    public static void AddTypedMembers()
    {
        var report = XbrlConverter.Convert(new TestTaxonomy(new TestContext("OwnerName")));

        using var scope = new AssertionScope(report.ToString);
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "typedMember").Which
            .Should().HaveAttributeWithValue("dimension", "frc-vt-dim:OwnersAxis").And
            .HaveElement(FrcVtDm + "OwnersTypedMember").Which
            .Should().HaveValue("OwnerName");
    }

    private record TestContext(
        [XbrlTypedMember("frc-vt-dim:OwnersAxis", "frc-vt-dm")]
        string OwnersTypedMember) : IContext
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