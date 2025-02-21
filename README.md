# xbrl-dotnet

## Namespaces

Everyting in XBRL is contained in a namespace, define the used namespaces first:

```csharp
public static readonly XNamespace FrcVtDim = "https://www.sbrnexus.nl/vt17/frc/20240131/dictionary/frc-vt-axes";
...
```

## Taxonomy

A taxonomy is a collection of contexts and metadata.

```csharp
record SomeTaxonomy(
    DateTime Start,
    DateTime End,
    IContext Context1,
    IContext Context2,
    ...
) : ITaxonomy.PeriodDuration
{
    NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
    NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
}
```

## Context

A context is a set of defined concepts, an identity and a period (optional).

```csharp
record SomeContext([NlCommonDataAttribute] string Something) : IContext
{
    IEntity IContext.Entity => new YourEntity(...);
    ExplicitMember[] IContext.ExplicitMembers => [];
    TypedMember[] IContext.TypedMembers => [];
}

record SomeContext(DateTime Start, DateTime End, ...) : IContext.PeriodDuration
{
    ...
}


record SomeContext(DateTime Instant, ...) : IContext.PeriodInstant
{
    ...
}
```

## Concepts

Define concepts for the taxonomy:

```csharp
class NlCommonDataAttribute() : 
    ConceptAttribute("nl-cd", "http://www.nltaxonomie.nl/nt17/sbr/20220301/dictionary/nl-common-data");
```

## Entity

Define the entity/entities to be used on contexts:

```csharp
class YourEntity(string value) : IEntity
{
    string IEntity.Value { get; } = value;
    string IEntity.Scheme => "http://www.kvk.nl/kvk-id";
}
```

## Dimensions

Define typed members:

```csharp
TypedMember[] IContext.TypedMembers =>
[
    new (FrcVtDm + "OwnersTypedMember", FrcVtDim + "OwnersAxis", SomeValue)
];
```

Define explicit members:

```csharp
ExplicitMember[] IContext.ExplicitMembers => 
[
    new(FrcVtDm + "ClientMember", FrcVtDim + "ClientAxis")
];
```

# Instance

Generate an instance report:

```csharp
var intance = new SomeTaxonomy(DateTime.Now, DateTime.Now, new SomeContext(...), ...)
var xbrl = XbrlConverter.Convert(instance);
```