using Diwen.Xbrl.Xml;

namespace XbrlDotNet.Tests;

public static class PeriodTests
{
    private record ContextWithPeriod(DateTime Start, DateTime End) : IContext
    {
        Entity IContext.Entity => new();
        Period IContext.Period => new (Start, End);
    }

    [Fact]
    public static void AddContextWithPeriod()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new TestReport(new ContextWithPeriod(date, date)));
        
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
    
    private record ContextWithPeriodInstant(DateTime Period) : IContext
    {
        Entity IContext.Entity => new();
        Period IContext.Period => new(Period);
    }

    [Fact]
    public static void AddContextWithPeriodInstant()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new TestReport(new ContextWithPeriodInstant(date)));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }

    private record ContextWithNoPeriod([Concept("x", "x")] string Something) : IContext
    {
        Entity IContext.Entity => new();
        Period? IContext.Period => null;
    }

    private record TestReportWithPeriod(DateTime Start, DateTime End, ContextWithNoPeriod TestContext) : IReport
    {
        public IEnumerable<IContext> Contexts => [TestContext];
        Period IReport.Period => new(Start, End);
    }

    [Fact]
    public static void AddReportPeriod()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriod(
            new DateTime(2020, 01, 01),
            new DateTime(2020, 01, 02),
            new("x")));
        
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
    
    private record TestReportWithPeriodInstant(DateTime Period, ContextWithNoPeriod TestContext) : IReport
    {
        public IEnumerable<IContext> Contexts => [TestContext];
        Period IReport.Period => new(Period);
    }

    [Fact]
    public static void AddReportPeriodInstant()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriodInstant(
            new DateTime(2020, 01, 01),
            new("x")));
        
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }
}