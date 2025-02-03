namespace XbrlDotNet.Tests;

public static class ContextTests
{
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestReportWith<T>([XbrlContext]T TestContext);
    
    private record ContextWithConstructorParameters([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);

    [Fact]
    public static void AddContextWithConstructorParameters()
    {
        var client = new TestReportWith<ContextWithConstructorParameters>(new ContextWithConstructorParameters("12345600"));
        
        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }

    [Fact]
    public static void AddMultipleContexts()
    {
        var client = new TestReportWith<IEnumerable<ContextWithConstructorParameters>>([
            new ContextWithConstructorParameters("12345600"),
            new ContextWithConstructorParameters("xxxxxxxx")
        ]);
        
        var report = XbrlConverter.Convert(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
        
        root.Elements(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Should()
            .Contain(x => x.Value == "xxxxxxxx");
    }

    private class ContextWithProperties
    {
        [XbrlFact(Metric = "nl-cd")]
        public string? ChamberOfCommerceRegistrationNumber { get; set; }
    }

    [Fact]
    public static void AddContextWithProperties()
    {
        var client = new TestReportWith<ContextWithProperties>(new() { ChamberOfCommerceRegistrationNumber = "12345600" });
        
        var report = XbrlConverter.Convert(client);

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }

    private class ContextNoAttributes
    {
        public string? ChamberOfCommerceRegistrationNumber { get; set; }
    }

    [Fact]
    public static void AddContextNoAttributes()
    {
        var client = new TestReportWith<ContextNoAttributes>(new() { ChamberOfCommerceRegistrationNumber = "12345600" });
        
        var report = XbrlConverter.Convert(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Descendants().Should().NotContain(x => x.Value == "12345600");
    }
}
