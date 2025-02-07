namespace XbrlDotNet.Tests;

public static class PeriodTests
{
    [Fact]
    public static void AddContextWithPeriod()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithPeriod(date, date)));

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

    [Fact]
    public static void AddContextWithPeriodInstant()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new Taxonomy(new ContextWithPeriodInstant(date)));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }

    [Fact]
    public static void AddReportPeriod()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriod(
            new DateTime(2020, 01, 01),
            new DateTime(2020, 01, 02),
            new ContextWithNoPeriod("x")));

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

    [Fact]
    public static void AddReportPeriodInstant()
    {
        var report = XbrlConverter.Convert(new TestReportWithPeriodInstant(
            new DateTime(2020, 01, 01),
            new ContextWithNoPeriod("x")));

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "period").Which
            .Should().HaveElement(Xbrli + "instant").Which
            .Should().HaveValue("2020-01-01");
    }

    private record ContextWithPeriod(DateTime PeriodStart, DateTime PeriodEnd) : IContext.WithPeriod
    {
        IEntity IContext.Entity => Entity.Dummy;
    }

    private record ContextWithPeriodInstant(DateTime Period) : IContext.WithInstant
    {
        IEntity IContext.Entity => Entity.Dummy;
    }

    private record ContextWithNoPeriod([Concept("x", "x")] string Something) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
    }

    private record TestReportWithPeriod(DateTime PeriodStart, DateTime PeriodEnd, ContextWithNoPeriod TestContext)
        : ITaxonomy.WithPeriod
    {
        public IEnumerable<IContext> Contexts => [TestContext];
    }

    private record TestReportWithPeriodInstant(DateTime Period, ContextWithNoPeriod TestContext) : ITaxonomy.WithInstant
    {
        public IEnumerable<IContext> Contexts => [TestContext];
    }
}