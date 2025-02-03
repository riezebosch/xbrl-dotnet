namespace XbrlDotNet.Tests;

public static class TypedMembersTests
{
    private record TestContext(
        [XbrlTypedMember("frc-vt-dim:OwnersAxis", "frc-vt-dm:OwnersTypedMember")]
        string Owner);
    
    [XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
    [XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
    private record TestReport([XbrlContext] TestContext TestContext);
    [Fact]
    public static void AddTypedMembers()
    {
        var report = XbrlDocument.For(new TestReport(new ("OwnerName")));
        
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