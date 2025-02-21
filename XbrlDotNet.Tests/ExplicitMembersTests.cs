using XbrlDotNet.Dimensions;

namespace XbrlDotNet.Tests;

public static class ExplicitMembersTests
{
    [Fact]
    public static void AddExplicitMembers()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new TestContext()));
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "explicitMember").Which
            .Should().HaveValue("frc-vt-dm:ClientMember")
            .And.HaveAttribute("dimension", "frc-vt-dim:ClientAxis");
    }

    private record TestContext : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => 
        [
            new(FrcVtDm + "ClientMember", FrcVtDim + "ClientAxis")
        ];
        TypedMember[] IContext.TypedMembers => [];
    }
}