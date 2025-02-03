namespace XbrlDotNet.Tests;

public static class EntityTests
{
    private record TestContext([XbrlEntity("http://www.kvk.nl/kvk-id")]string KvkId);

    private record TestReport([XbrlContext] TestContext TestContext);

    [Fact]
    public static void AddContextEntity()
    {
        var report = XbrlDocument.For(new TestReport(new ("12345600")));
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