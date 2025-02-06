using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

public static class UnitsTests
{
    private record ContextWithUnitRef(
        [NlCommonData] [Units.Euro] string FamilyName
    ) : IContext
    {
        Entity IContext.Entity => new();
        Period? IContext.Period => null;
    }
    
    [Fact]
    public static void AddUnitRef()
    {
        var report = XbrlConverter.Convert(new TestReport(new ContextWithUnitRef("name")));

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
    }
}