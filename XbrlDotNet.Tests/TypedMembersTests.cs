using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

public static class TypedMembersTests
{
    private record TestContext(
        [XbrlTypedMember("frc-vt-dim:OwnersAxis", "frc-vt-dm")]
        string OwnersTypedMember) : IContext
    {
        Entity IContext.Entity => new();
    }

    [XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
    [XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
    private record TestReport(TestContext TestContext) : IReport
    {
        public IEnumerable<IContext> Contexts => [TestContext];
    }

    [Fact]
    public static void AddTypedMembers()
    {
        var report = XbrlConverter.Convert(new TestReport(new ("OwnerName")));
        
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
}