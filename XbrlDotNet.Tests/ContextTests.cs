using XbrlDotNet.Dimensions;
using Xunit.Abstractions;

namespace XbrlDotNet.Tests;

public class ContextTests(ITestOutputHelper output)
{
    [Fact]
    public void AddContextWithConstructorParameters()
    {
        var client = new Taxonomy(new ContextWithConstructorParameters("12345600"));

        var report = XbrlConverter.Convert(client);
        output.WriteLine(report.ToString());

        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }

    private record Taxonomy2(IContext Context1, IContext Context2) : ITaxonomy
    {
        NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
        NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
    }
    
    [Fact]
    public static void AddMultipleContexts()
    {
        var client = new Taxonomy2(
            new ContextWithConstructorParameters("12345600"),
            new ContextWithConstructorParameters("xxxxxxxx")
        );

        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());

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
    
    private record TaxonomyList(params IContext[] Contexts) : ITaxonomy
    {
        NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
        NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
    }
    
    [Fact]
    public static void ListOfContexts()
    {
        var client = new TaxonomyList(
            new ContextWithConstructorParameters("12345600"),
            new ContextWithConstructorParameters("xxxxxxxx")
        );

        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());

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

    [Fact]
    public static void AddContextWithProperties()
    {
        var client = new Taxonomy(new ContextWithProperties { ChamberOfCommerceRegistrationNumber = "12345600" });

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
    public static void ContextNameTest()
    {
        var client = new Taxonomy(new ContextName());

        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context")
            .Which
            .Should()
            .HaveAttributeWithValue("id", "c0d_0ContextName");
    }

    [Fact]
    public static void Null()
    {
        var client = new Taxonomy(null);

        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());
        report
            .Should()
            .HaveRoot(Xbrli + "xbrl");
        report.Should().NotHaveElement(Xbrli + "context");
    }
    
    private record ContextWithConstructorParameters([NlCommonData] string ChamberOfCommerceRegistrationNumber)
        : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private class ContextWithProperties : IContext
    {
        [NlCommonData] public string? ChamberOfCommerceRegistrationNumber { get; set; }
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }

    private record ContextName : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
        ExplicitMember[] IContext.ExplicitMembers => [];
        TypedMember[] IContext.TypedMembers => [];
    }
}