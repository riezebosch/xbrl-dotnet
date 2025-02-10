namespace XbrlDotNet.Tests.TestTaxonomy;

internal record Taxonomy(params IEnumerable<IContext> Contexts) : ITaxonomy
{
}