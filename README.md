# xbrl-dotnet

## Report

A report is literally nothing more than a collection of contexts and some metadata.

```csharp
[XbrlDimensionNamespace(prefix, uri)]
[XbrlTypedDomainNamespace(prefix, uri)]
record SomeReport(
    DateTime Start,
    DateTime End,
    public IEnumerable<IContext> Contexts =>
    [
        new SomeContext(...)
    ];
    ...
) : IReport.WithPeriod;
```

## Context

A context is a set of defined concepts, an identity and period (optional).

```csharp
[XbrlExplicitMember(dimension, value)]
record SomeContext(
    [NlCommonDataAttribute] string Something
    ) : IContext
{
    IEntity IContext.Entity => new YourEntity(...);
}

record SomeContext(...) : IContext.WithPeriod
{
    IEntity IContext.Entity => new YourEntity(...);
}


record SomeContext(...) : IContext.WithInstant
{
    IEntity IContext.Entity => new YourEntity(...);
}
```

## Concepts

Define the concepts for your taxonomy:

```csharp
class NlCommonDataAttribute() : 
    ConceptAttribute("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data");
```

## Entity

```csharp
class YourEntity(string value) : IEntity
{
    string IEntity.Value { get; } = value;
    string IEntity.Scheme => "http://www.kvk.nl/kvk-id";
}
```

Generate the report:

```csharp
var xbrl = XbrlConverter.Convert(new SomeReport(DateTime.Now, DateTime.Now, new SomeContext(...), ...);
```