using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Diwen.Xbrl.Xml;

namespace XbrlDotNet;

public static class XbrlDocument
{
    public static XDocument For(object data)
    {
        var report = new Report();

        Report(report, data);
        Contexts(report, data);

        return report.ToXDocument();
    }

    private static void Report(Report report, object data)
    {
        foreach (var attribute in data.GetType().GetCustomAttributes())
        {
            switch (attribute)
            {
                case XbrlTypedDomainNamespaceAttribute a:
                    report.SetTypedDomainNamespace(a.Prefix, a.Uri);
                    break;
                case XbrlDimensionNamespaceAttribute a:
                    report.SetDimensionNamespace(a.Prefix, a.Uri);
                    break;
                case XbrlUnitAttribute a:
                    report.Units.Add(a.Id, a.Value);
                    break;
            }
        }
        
        var attributes = CustomAttributes(data);
        foreach (var property in data.GetType().GetProperties())
        {
            var value = property.GetValue(data)!;
            foreach (var attr in attributes[property.Name])
            {
                switch (attr)
                {
                    case XbrlPeriodStartAttribute:
                        report.Period.StartDate = (DateTime)value;
                        break;
                    case XbrlPeriodEndAttribute:
                        report.Period.EndDate = (DateTime)value;
                        break;
                }
            }
        }
    }

    private static void Contexts(Report report, object data)
    {
        var attributes = CustomAttributes(data);
        foreach (var property in data
                     .GetType()
                     .GetProperties()
                     .Where(x => attributes[x.Name].OfType<XbrlContextAttribute>().Any()))
        {
            var value = property.GetValue(data)!;
            AddContexts(report, value);
        }
    }

    private static void AddContexts(Report report, object item)
    {
        if (item is IEnumerable children)
        {
            foreach (var child in children)
            {
                AddContext(report, child);
            }
        }
        else
        {
            AddContext(report, item);
        }
    }

    private static void AddContext(Report report, object o)
    {
        report.Contexts.IdFormat = "c{0}d_0" + o.GetType().Name;
        var scenario = new Scenario(report);
        
        foreach (var attribute in o.GetType().GetCustomAttributes<XbrlExplicitMemberAttribute>())
        {
            scenario.AddExplicitMember(attribute.Dimension, attribute.Value);
        }

        var context = report.CreateContext(scenario);
        var attributes = CustomAttributes(o);
        foreach (var property in o.GetType().GetProperties())
        {
            var value = property.GetValue(o);
            foreach (var attr in attributes[property.Name])
            {
                switch (attr)
                {
                    case XbrlFactAttribute a:
                        report.AddFact(context, $"{a.Metric}:{property.Name}", a.UnitRef, a.Decimals, value?.ToString());
                        break;
                    case XbrlTypedMemberAttribute a:
                        context.Scenario.AddTypedMember(a.Dimension, a.Domain, value?.ToString());
                        break;
                    case XbrlEntityAttribute a:
                        context.Entity = new Entity(a.Scheme, value?.ToString());
                        break;
                    case XbrlPeriodStartAttribute:
                        context.Period.StartDate = (DateTime)value!;
                        break;
                    case XbrlPeriodEndAttribute:
                        context.Period.EndDate = (DateTime)value!;
                        break;
                }
            }
        }
    }

    private static Dictionary<string, List<Attribute>> CustomAttributes(object o)
    {
        var parameters = o.GetType()
            .GetConstructors()
            .First()
            .GetParameters().ToDictionary(x => x.Name!);

        var attributes = o
            .GetType()
            .GetProperties()
            .ToDictionary(x => x.Name, x => x
                .GetCustomAttributes()
                .Concat(parameters.GetValueOrDefault(x.Name)?.GetCustomAttributes() ?? []).ToList());
        return attributes;
    }

    private static XDocument ToXDocument(this Report report)
    {
        using var nodeReader = new XmlNodeReader(report.ToXmlDocument());
        var xdoc = XDocument.Load(nodeReader);

        xdoc.Root!.Add(new XAttribute(XNamespace.Xml + "lang", "nl"));

        // xdoc.Descendants()
        //     .Attributes()
        //     .Where(a => a.IsNamespaceDeclaration && a.Value == xsiNamespace)
        //     .Remove();

        return xdoc;
    }
}