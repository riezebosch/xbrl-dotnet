using System.Xml.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using XbrlDotNet;

namespace Xbrl.ToObjects.Tests;

public static class XbrlDocumentTests
{
    private static readonly XNamespace Xbrli = "http://www.xbrl.org/2003/instance";
    private static readonly XNamespace NlCd = "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data";
    private static readonly XNamespace Xbrldi = "http://xbrl.org/2006/xbrldi";
    private static readonly XNamespace FrcVtDm = "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains";
    
    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    [XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
    [XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
    [XbrlUnit("EUR", "iso4217:EUR")]
    private record TestReportWith<T>([XbrlContext]T TestContext);
    
    private record ContextWithConstructorParameters([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);

    [Fact]
    public static void AddCContextWithConstructorParameters()
    {
        var client = new TestReportWith<ContextWithConstructorParameters>(new ContextWithConstructorParameters("12345600"));
        
        var report = XbrlDocument.For(client);
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
        
        var report = XbrlDocument.For(client);
        
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
        
        var report = XbrlDocument.For(client);

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }

    private record ContextWithDecimals(
        [XbrlFact(Metric = "nl-cd", Decimals = "INF")]
        string FamilyName
    );

    [Fact]
    public static void AddDecimals()
    {
        var client = new TestReportWith<ContextWithDecimals>(new ("name"));
        
        var report = XbrlDocument.For(client);
        
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
        var client = new TestReportWith<ContextWithUnitRef>(new("name"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
    }

    private class ContextNoAttributes
    {
        public string? KvkId { get; set; }
    }

    [Fact]
    public static void AddContextNoAttributes()
    {
        var client = new TestReportWith<ContextNoAttributes>(new() { KvkId = "12345600" });
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Descendants().Should().NotContain(x => x.Value == "12345600");
    }

    private record ContextTypedMember(
        [XbrlTypedMember("frc-vt-dim:OwnersAxis", "frc-vt-dm:OwnersTypedMember")]
        string Owner);

    [Fact]
    public static void AddTypedMembers()
    {
        var client = new TestReportWith<ContextTypedMember>(new ("OwnerName"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "typedMember").Which
            .Should().HaveAttributeWithValue("dimension", "frc-vt-dim:OwnersAxis").And
            .HaveElement(FrcVtDm + "OwnersTypedMember").Which
            .Should().HaveValue("OwnerName");
    }

    [XbrlExplicitMember("frc-vt-dim:ClientAxis", "frc-vt-dm:ClientMember")]
    private record ContextExplicitMember;

    [Fact]
    public static void AddExplicitMembers()
    {
        var client = new TestReportWith<ContextExplicitMember>(new ());
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "scenario").Which
            .Should().HaveElement(Xbrldi + "explicitMember").Which
            .Should().HaveValue("frc-vt-dm:ClientMember")
            .And.HaveAttribute("dimension", "frc-vt-dim:ClientAxis");
    }

    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record ContextTypedDomainNamespace([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);

    [Fact]
    public static void AddTypedDomainNamespace()
    {
        var client = new TestReportWith<ContextTypedDomainNamespace>(new("12345600"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }
    
    private record ContextEntity([XbrlEntity("http://www.kvk.nl/kvk-id")]string KvkId);

    [Fact]
    public static void AddContextEntity()
    {
        var client = new TestReportWith<ContextEntity>(new ("12345600"));
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context").Which
            .Should().HaveElement(Xbrli + "entity").Which
            .Should().HaveElement(Xbrli + "identifier").Which
            .Should().HaveValue("12345600")
            .And.HaveAttribute("scheme", "http://www.kvk.nl/kvk-id");
    }
    
    private record ContextWithPeriod(
        [XbrlPeriodStart] DateTime Start,
        [XbrlPeriodEnd] DateTime End
        );

    [Fact]
    public static void AddCContextWithPeriod()
    {
        var date = new DateTime(2020, 01, 01);
        var client = new TestReportWith<ContextWithPeriod>(new (date, date));
        
        var report = XbrlDocument.For(client);
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

    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record TestReportPeriod<T>([XbrlPeriodStart] DateTime Start, [XbrlPeriodEnd] DateTime End, [XbrlContext] T Context);

    [Fact]
    public static void AddReportPeriod()
    {
        var date = new DateTime(2020, 01, 01);
        var client = new TestReportPeriod<ContextWithConstructorParameters>(
            date,
            date,
            new("1234"));
        
        var report = XbrlDocument.For(client);
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
}
