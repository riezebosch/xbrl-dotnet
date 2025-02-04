namespace XbrlDotNet.Tests;

public static class PeriodTests
{
    private record ContextWithPeriod([XbrlPeriodStart] DateTime Start, [XbrlPeriodEnd] DateTime End);
    private record TestReportWith<T>([XbrlContext]T TestContext);

    [Fact]
    public static void AddContextWithPeriod()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new TestReportWith<ContextWithPeriod>(new (date, date)));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "startDate").Which
            .Should().HaveValue("2020-01-01");
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "endDate").Which
            .Should().HaveValue("2020-01-01");
    }
    
    private record ContextWithPeriodInstant([XbrlPeriodInstant] DateTime Instant);
    
    [Fact]
    public static void AddContextWithPeriodInstant()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new TestReportWith<ContextWithPeriodInstant>(new (date)));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }

    private record ContextWithNoPeriod;
    
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestReportWithPeriod([XbrlPeriodStart] DateTime Start, [XbrlPeriodEnd] DateTime End, [XbrlContext] ContextWithNoPeriod TestContext);

    [Fact]
    public static void AddReportPeriod()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriod(
            new DateTime(2020, 01, 01),
            new DateTime(2020, 01, 02),
            new()));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "startDate").Which
            .Should().HaveValue("2020-01-01");
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "endDate").Which
            .Should().HaveValue("2020-01-02");
    }
    
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestReportWithPeriodInstant([XbrlPeriodInstant] DateTime Period, [XbrlContext]ContextWithNoPeriod TestContext);

    [Fact]
    public static void AddReportPeriodInstant()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriodInstant(
            new DateTime(2020, 01, 01),
            new()));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }
}