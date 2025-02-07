# xbrl-dotnet

## Taxonomy

A taxonomy is a collection of contexts and some metadata.

```csharp
[XbrlDimensionNamespace(prefix, uri)]
[XbrlTypedDomainNamespace(prefix, uri)]
record SomeTaxonomy(
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

A context is a set of defined concepts, an identity and a period (optional).

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

Define concepts for your taxonomy:

```csharp
class NlCommonDataAttribute() : 
    ConceptAttribute("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data");
```

## Entity

Define entities to be used on contexts:

```csharp
class YourEntity(string value) : IEntity
{
    string IEntity.Value { get; } = value;
    string IEntity.Scheme => "http://www.kvk.nl/kvk-id";
}
```

## Instance

Generate the report for an instance:

```csharp
var intance = new SomeTaxonomy(DateTime.Now, DateTime.Now, new SomeContext(...), ...)
var xbrl = XbrlConverter.Convert();
```