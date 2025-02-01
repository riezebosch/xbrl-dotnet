using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Diwen.Xbrl.Xml;

namespace Xbrl.ToObjects;

public static class XbrlDocument
{
    public static XDocument For(object data)
    {
        var type = data.GetType();
        var report = new Report();
        
        foreach (var attribute in type.GetCustomAttributes<XbrlTypedDomainNamespaceAttribute>())
        {
            report.SetTypedDomainNamespace(attribute.Prefix, attribute.Uri);
        }
        
        foreach (var attribute in type.GetCustomAttributes<XbrlDimensionNamespaceAttribute>())
        {
            report.SetDimensionNamespace(attribute.Prefix, attribute.Uri);
        }
        
        foreach (var attribute in type.GetCustomAttributes<XbrlUnitAttribute>())
        {
            report.Units.Add(attribute.Id, attribute.Value);
        }

        var attributes = CustomAttributes(data);
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(data)!;
            SetPeriod(value, report.Period, attributes[property.Name]);

            if (attributes[property.Name].OfType<XbrlContextAttribute>().Any())
            {
                AddContexts(report, value);
            }
        }

        return report.ToXDocument();
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
            var attr = attributes[property.Name];
            var value = property.GetValue(o);

            if (attr.OfType<XbrlFactAttribute>().FirstOrDefault() is { } fact)
            {
                report.AddFact(context, $"{fact.Metric}:{property.Name}", fact.UnitRef, fact.Decimals, value?.ToString());
            }

            if (attr.OfType<XbrlTypedMemberAttribute>().FirstOrDefault() is { } member)
            {
                scenario.AddTypedMember(member.Dimension, member.Domain, value?.ToString());
            }

            if (attr.OfType<XbrlEntityAttribute>().FirstOrDefault() is { } entity)
            {
                context.Entity = new Entity(entity.Scheme, value?.ToString());
            }
            
            SetPeriod(value, context.Period, attr);
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

    private static void SetPeriod(object? value, Period period, IList<Attribute> attr)
    {
        if (attr.OfType<XbrlStartPeriodAttribute>().Any())
        {
            period.StartDate = (DateTime)value!;
        }

        if (attr.OfType<XbrlEndPeriodAttribute>().Any())
        {
            period.EndDate = (DateTime)value!;
        }
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