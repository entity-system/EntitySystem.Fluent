using EntitySystem.Client.Services;
using EntitySystem.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace EntitySystem.Client.Fluent;

public static class EntitySystemClientFluentRegistrations
{
    public static IServiceCollection AddEntitySystemClientFluent<TService, TEntity>(this IServiceCollection serviceCollection)
        where TService : IEntityService<TEntity>
        where TEntity : class, IEntity
    {
        return serviceCollection
            .Scan(s => s.FromAssemblyOf<TService>().AddClasses(c => c.AssignableTo(typeof(EntityService<>))).AsSelf().WithSingletonLifetime())
            .AddEntitySystemClient();
    }
}