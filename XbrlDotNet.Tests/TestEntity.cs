namespace XbrlDotNet.Tests;

internal class TestEntity(string value) : IEntity
{
    string IEntity.Value { get; } = value;
    string IEntity.Scheme => "http://www.kvk.nl/kvk-id";
    public static TestEntity Dummy => new("");
}