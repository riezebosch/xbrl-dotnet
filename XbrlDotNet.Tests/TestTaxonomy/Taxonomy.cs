namespace XbrlDotNet.Tests.TestTaxonomy;

internal record Taxonomy(IContext? Context) : ITaxonomy
{
    NamespacePrefix ITaxonomy.Domain => new("frc-vt-dm", FrcVtDm);
    NamespacePrefix ITaxonomy.Dimension => new("frc-vt-dim", FrcVtDim);
}