namespace XbrlDotNet.Tests;

public static class FactTests
{
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    [XbrlUnit("EUR", "iso4217:EUR")]
    private record TestReportWith<T>([XbrlContext]T TestContext);

    private record ContextWithDecimals(
        [XbrlFact(Metric = "nl-cd", Decimals = "INF")]
        string FamilyName
    );

    [Fact]
    public static void AddDecimals()
    {
        var report = XbrlDocument.For(new TestReportWith<ContextWithDecimals>(new ("name")));
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("decimals", "INF");
    }

    private record ContextWithUnitRef(
        [XbrlFact(Metric = "nl-cd", UnitRef = "EUR")]
        string FamilyName
    );

    [Fact]
    public static void AddUnitRef()
    {
        var report = XbrlDocument.For(new TestReportWith<ContextWithUnitRef>(new("name")));
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
    }
}