using XbrlDotNet.Dimensions;
using XbrlDotNet.Facts;

namespace XbrlDotNet.Tests;

public static class UnitTests
{
    [Fact]
    public static void AddUnitRef()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithUnitRef("name", "123")));

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
        root.Should().HaveElement(NlCd + "ShareRenewableEnergy")
            .Which
            .Should()
            .HaveValue("123")
            .And.HaveAttribute("unitRef", "pure");
    }

    private record ContextWithUnitRef(
        [NlCommonData] [Euro] string FamilyName,
        [NlCommonData] [Pure] string ShareRenewableEnergy
    ) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }
}