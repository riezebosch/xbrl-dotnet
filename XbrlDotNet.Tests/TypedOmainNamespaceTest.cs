namespace XbrlDotNet.Tests;

public static class TypedOmainNamespaceTest
{
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestContext([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestReport([XbrlContext] TestContext TestContext);

    [Fact]
    public static void AddTypedDomainNamespace()
    {
        var report = XbrlDocument.For(new TestReport(new("12345600")));
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }
}