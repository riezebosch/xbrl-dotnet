namespace XbrlDotNet.Tests;

public static class UnitsTests
{
    [Fact]
    public static void AddUnitRef()
    {
        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithUnitRef("name")));

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
    }

    private record ContextWithUnitRef(
        [NlCommonData] [Units.Euro] string FamilyName
    ) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
    }
}