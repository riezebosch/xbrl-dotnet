using System.Xml.Linq;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Xbrl.ToObjects.Tests;

public static class XbrlDocumentTests
{
    private static readonly XNamespace Xbrli = "http://www.xbrl.org/2003/instance";
    private static readonly XNamespace NlCd = "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data";
    private static readonly XNamespace Xbrldi = "http://xbrl.org/2006/xbrldi";
    private static readonly XNamespace FrcVtDm = "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains";
    private record ClientConstructorParameters([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);

    [Fact]
    public static void AddClientConstructorParameters()
    {
        var client = new TestReport2<ClientConstructorParameters>(new ClientConstructorParameters("12345600"));
        
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
    public static void AddMultiple()
    {
        var client = new TestReport2<IEnumerable<ClientConstructorParameters>>([
            new ClientConstructorParameters("12345600"),
            new ClientConstructorParameters("xxxxxxxx")
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

    private class ClientProperties
    {
        [XbrlFact(Metric = "nl-cd")]
        public string? ChamberOfCommerceRegistrationNumber { get; set; }
    }

    [Fact]
    public static void AddClientProperties()
    {
        var client = new TestReport2<ClientProperties>(new ClientProperties  { ChamberOfCommerceRegistrationNumber = "12345600" });
        
        var report = XbrlDocument.For(client);

        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }

    private record ClientDecimals(
        [XbrlFact(Metric = "nl-cd", Decimals = "INF")]
        string FamilyName
    );

    [Fact]
    public static void AddDecimals()
    {
        var client = new TestReport2<ClientDecimals>(new ClientDecimals("name"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("decimals", "INF");
    }

    private record ClientUnitRef(
        [XbrlFact(Metric = "nl-cd", UnitRef = "EUR")]
        string FamilyName
    );

    [Fact]
    public static void AddUnitRef()
    {
        var client = new TestReport2<ClientUnitRef>(new ClientUnitRef("name"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "FamilyName")
            .Which
            .Should()
            .HaveValue("name")
            .And.HaveAttribute("unitRef", "EUR");
    }

    private class ClientNoAttributes
    {
        public string? KvkId { get; set; }
    }

    [Fact]
    public static void AddClientNoAttributes()
    {
        var client = new TestReport2<ClientNoAttributes>(new ClientNoAttributes { KvkId = "12345600" });
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Descendants().Should().NotContain(x => x.Value == "12345600");
    }

    private record ClientTypedMember(
        [XbrlTypedMember("frc-vt-dim:OwnersAxis", "frc-vt-dm:OwnersTypedMember")]
        string Owner);

    [Fact]
    public static void AddTypedMembers()
    {
        var client = new TestReport2<ClientTypedMember>(new ClientTypedMember("OwnerName"));
        
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
    private record ClientExplicitMember;

    [Fact]
    public static void AddExplicitMembers()
    {
        var client = new TestReport2<ClientExplicitMember>(new ClientExplicitMember());
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        // root.AssertContext("c0d_0ClientExplicitMember")
        //     .HasExplicitMember("frc-vt-dim:ClientAxis", "frc-vt-dm:ClientMember");
    }

    [XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
    private record ClientTypedDomainNamespace([XbrlFact(Metric = "nl-cd")] string ChamberOfCommerceRegistrationNumber);

    [Fact]
    public static void AddTypedDomainNamespace()
    {
        var client = new TestReport2<ClientTypedDomainNamespace>(new ClientTypedDomainNamespace("12345600"));
        
        var report = XbrlDocument.For(client);
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }
    
    private record ClientEntity([XbrlEntity("http://www.kvk.nl/kvk-id")]string KvkId);

    [Fact]
    public static void AddClientEntity()
    {
        var client = new TestReport2<ClientEntity>(new ("12345600"));
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        // root.AssertContext("c0d_0ClientEntity").HasIdentifier("12345600");
    }
    
    private record ClientPeriod(
        [XbrlStartPeriod] DateTime Start,
        [XbrlEndPeriod] DateTime End
        );

    [Fact]
    public static void AddClientPeriod()
    {
        var date = new DateTime(2020, 01, 01);
        var client = new TestReport2<ClientPeriod>(new (date, date));
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        // root.AssertContext("c0d_0ClientPeriod")
        //     .HasPeriod(date);
    }
    
    [Fact]
    public static void AddReportPeriod()
    {
        var date = new DateTime(2020, 01, 01);
        var client = new TestReportPeriod<ClientConstructorParameters>(date, date, new("1234"));
        
        var report = XbrlDocument.For(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        // root.AssertContext("c0d_0ClientConstructorParameters")
        //     .HasPeriod(date);
        
        root.Descendants(Xbrli + "context").Should().NotContain(x => (string?)x.Attribute("id") == "c1d_0DateTime");
    }
}

[XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
public record TestReportPeriod<T>([XbrlStartPeriod] DateTime Start, [XbrlEndPeriod] DateTime End, [XbrlContext] T Context);

[XbrlTypedDomainNamespace("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data")]
[XbrlTypedDomainNamespace("frc-vt-dm", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-domains")]
[XbrlDimensionNamespace("frc-vt-dim", "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes")]
[XbrlUnit("EUR", "iso4217:EUR")]
internal record TestReport2<T>([XbrlContext]T TestContext);