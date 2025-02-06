using Diwen.Xbrl.Xml;
using Xunit.Abstractions;

namespace XbrlDotNet.Tests;

public class ContextTests(ITestOutputHelper output)
{
    private record ContextWithConstructorParameters([NlCommonData] string ChamberOfCommerceRegistrationNumber): IContext
    {
        Entity IContext.Entity => new();
        Period? IContext.Period => null;
    };

    [Fact]
    public void AddContextWithConstructorParameters()
    {
        var client = new TestReport(new ContextWithConstructorParameters("12345600"));
        
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
        var client = new TestReport(
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

    private class ContextWithProperties : IContext
    {
        [NlCommonData]
        public string? ChamberOfCommerceRegistrationNumber { get; set; }

        Entity IContext.Entity => new();
        Period? IContext.Period => null;
    }

    [Fact]
    public static void AddContextWithProperties()
    {
        var client = new TestReport(new ContextWithProperties { ChamberOfCommerceRegistrationNumber = "12345600" });
        
        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());
        
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(NlCd + "ChamberOfCommerceRegistrationNumber")
            .Which
            .Should()
            .HaveValue("12345600");
    }
    
    record ContextName : IContext
    {
        Entity IContext.Entity => new();
        Period? IContext.Period => null;
    }

    [Fact]
    public static void ContextNameTest()
    {
        var client = new TestReport(new ContextName());
        
        var report = XbrlConverter.Convert(client);
        using var scope = new AssertionScope(report.ToString());
        var root = report
            .Element(Xbrli + "xbrl")!;
        root.Should().HaveElement(Xbrli + "context")
            .Which
            .Should()
            .HaveAttributeWithValue("id", "c0d_0ContextName");
    }
}