using EntitySystem.Shared.Fluent.Attributes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace EntitySystem.Server.Fluent.Database;

public class IndexedPropertyConvention : AttributePropertyConvention<IndexedAttribute>
{
    protected override void Apply(IndexedAttribute attribute, IPropertyInstance instance)
    {
        instance.Index("idx__" + instance.Property.Name);
    }
}