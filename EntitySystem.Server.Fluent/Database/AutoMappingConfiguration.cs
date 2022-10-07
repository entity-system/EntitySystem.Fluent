using System;
using System.Linq;
using EntitySystem.Shared.Fluent.Attributes;
using EntitySystem.Shared.Fluent.Domain;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace EntitySystem.Server.Fluent.Database;

public class AutoMappingConfiguration : DefaultAutomappingConfiguration
{
    public override bool ShouldMap(Type type)
    {
        return type.IsSubclassOf(typeof(Entity));
    }

    public override bool ShouldMap(Member member)
    {
        return !member.MemberInfo.GetCustomAttributes(false).OfType<IgnoredAttribute>().Any() && base.ShouldMap(member);
    }
}