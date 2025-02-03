using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XbrlDotNet;

public class PropertyAttributesProvider
{
    public IEnumerable<Attribute> For(PropertyInfo property)
    {
        ICustomAttributeProvider?[] providers =
        [
            property,
            property.DeclaringType?.GetConstructors().FirstOrDefault()?.GetParameters().FirstOrDefault(x => x.Name == property.Name)
        ];

        return providers
            .OfType<ICustomAttributeProvider>()
            .SelectMany(x => x.GetCustomAttributes(true).Cast<Attribute>());
    }
}