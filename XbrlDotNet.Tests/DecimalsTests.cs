using XbrlDotNet.Dimensions;
using XbrlDotNet.Facts;

namespace XbrlDotNet.Tests;

public static class DecimalsTests
{
    [Fact]
    public static void AddInf()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithInf("name")));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttributeWithValue("decimals", "INF");
    }

    [Fact]
    public static void AddDecimals()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithDecimals("name")));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttributeWithValue("decimals", "2");
    }

    private record ContextWithInf(
        [NlCommonData] [Inf] string FamilyName
    ) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private record ContextWithDecimals(
        [NlCommonData] [Decimals(2)] string FamilyName
    ) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }
}