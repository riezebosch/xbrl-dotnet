namespace XbrlDotNet.Tests.TestTaxonomy;

internal class Entity(string value) : IEntity
{
    public static Entity Dummy => new("");
    string IEntity.Value { get; } = value;
    string IEntity.Scheme => "http://www.kvk.nl/kvk-id";
}