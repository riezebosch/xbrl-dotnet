using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

public static class EntityTests
{
    private record KvkEntity(string Id)
    {
        public static implicit operator Entity(KvkEntity instance) => new("http://www.kvk.nl/kvk-id", instance.Id);
    }

    private record TestContext(string KvkId) : IContext
    {
        Entity IContext.Entity => new KvkEntity(KvkId);
    }

    [Fact]
    public static void AddContextEntity()
    {
        var report = XbrlConverter.Convert(new TestReport(new TestContext("12345600")));
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "entity").Which
            .Should().HaveElement(Xbrli + "identifier").Which
            .Should().HaveValue("12345600")
            .And.HaveAttribute("scheme", "http://www.kvk.nl/kvk-id");
    }
}