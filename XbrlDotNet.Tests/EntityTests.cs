using XbrlDotNet.Dimensions;

namespace XbrlDotNet.Tests;

public static class EntityTests
{
    [Fact]
    public static void AddContextEntity()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new TestContext("12345600")));
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "entity").Which
            .Should().HaveElement(Xbrli + "identifier").Which
            .Should().HaveValue("12345600")
            .And.HaveAttribute("scheme", "http://www.kvk.nl/kvk-id");
    }

    private record TestContext(string KvkId) : IContext
    {
        IEntity IContext.Entity => new Entity(KvkId);
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }
}