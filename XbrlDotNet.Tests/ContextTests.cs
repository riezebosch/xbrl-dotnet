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

    [Fact]
    public static void AddMultipleContexts()
    {
        var client = new Taxonomy(
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

    private record ContextWithConstructorParameters([NlCommonData] string ChamberOfCommerceRegistrationNumber)
        : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
    }

    private class ContextWithProperties : IContext
    {
        [NlCommonData] public string? ChamberOfCommerceRegistrationNumber { get; set; }

        IEntity IContext.Entity => Entity.Dummy;
    }

    private record ContextName : IContext
    {
        IEntity IContext.Entity => Entity.Dummy;
    }
}