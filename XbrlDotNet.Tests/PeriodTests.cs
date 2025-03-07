using XbrlDotNet.Dimensions;
using XbrlDotNet.Facts;

namespace XbrlDotNet.Tests;

public static class PeriodTests
{
    [Fact]
    public static void AddContextWithPeriod()
    {
        var date = new DateTime(2020, 01, 01);

        var report = XbrlConverter.Convert(new Taxonomy(new ContextPeriodDuration(date, date)));

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

        var report = XbrlConverter.Convert(new Taxonomy(new ContextPeriodInstant(date)));

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
        var report = XbrlConverter.Convert(new TestReportPeriodDuration(
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
        var report = XbrlConverter.Convert(new TestReportPeriodPeriodInstant(
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

    private record ContextPeriodDuration(DateTime Start, DateTime End) : IContext.PeriodDuration
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private record ContextPeriodInstant(DateTime Instant) : IContext.PeriodInstant
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private record ContextWithNoPeriod([Concept("x", "x")] string Something) : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private record TestReportPeriodDuration(DateTime Start, DateTime End, IContext TestContext)
        : ITaxonomy.PeriodDuration
    {
        NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
        NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
    }

    private record TestReportPeriodPeriodInstant(DateTime Instant, IContext TestContext) : ITaxonomy.PeriodInstant
    {
        NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
        NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
    }
}