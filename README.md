# xbrl-dotnet

Setup an object model:

```csharp
[XbrlUnit(id, value)]
[XbrlDimensionNamespace(prefix, uri)]
[XbrlTypedDomainNamespace(prefix, uri)]
record SomeReport(
    [XbrlPeriodStart] DateTime Start,
    [XbrlPeriodEnd] DateTime End,
    [XbrlContext] SomeContext Context,
    ...
    );

[XbrlExplicitMember(dimension, value)]
record SomeContext(
    [XbrlEntity] string Id,
    [XbrlFact(Metric = ..., Decimals = ..., UnitRef = ...)] string Value,
    [XbrlTypedMember(dimension, domain)] string Something
    );
```

Generate the report:

```csharp
var xbrl = XbrlConverter.Convert(new SomeReport(DateTime.Now, DateTime.Now, new SomeContext(...), ...);
```