using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

public static class DecimalsTests
{
    private record ContextWithInf(
        [NlCommonData] [Decimals.INF] string FamilyName
    ) : IContext
    {
        IEntity IContext.Entity => TestEntity.Dummy;
    }

    [Fact]
    public static void AddInf()
    {
        var report = XbrlConverter.Convert(new TestReport(new ContextWithInf("name")));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttributeWithValue("decimals", "INF");
    }

    private record ContextWithDecimals(
        [NlCommonData] [Decimals(2)] string FamilyName
    ) : IContext
    {
        IEntity IContext.Entity => TestEntity.Dummy;
    }

    [Fact]
    public static void AddDecimals()
    {
        var report = XbrlConverter.Convert(new TestReport(new ContextWithDecimals("name")));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttributeWithValue("decimals", "2");
    }
}