using System;
using EntitySystem.Server.Fluent.Database;
using EntitySystem.Server.Services;
using EntitySystem.Shared.Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace EntitySystem.Server.Fluent
{
    public static class EntitySystemServerFluentRegistrations
    {
        public static IServiceCollection AddEntitySystemServerFluent<TService, TEntity>(this IServiceCollection serviceCollection, Action<ISession> initialization = null)
            where TService : IEntityService<TEntity>
            where TEntity : class, IEntity
        {
            return serviceCollection.AddEntitySystemServerFluent<TService, TEntity>(SQLiteConfiguration.Standard.UsingFile("Database.db"), initialization);
        }

        public static IServiceCollection AddEntitySystemServerFluent<TService, TEntity>(this IServiceCollection serviceCollection, IPersistenceConfigurer persistenceConfig, Action<ISession> initialization = null, bool standardOutput = false)
            where TService : IEntityService<TEntity>
            where TEntity : class, IEntity
        {
            var databaseConfiguration = Fluently.Configure()
                .Database(persistenceConfig)
                .AddEntitySystemFluentMappings<TEntity>()
                .BuildEntitySystemFluentConfiguration(standardOutput);

            return serviceCollection
                .Scan(s => s.FromAssemblyOf<TService>().AddClasses(c => c.AssignableTo(typeof(EntityService<>))).AsSelf().WithScopedLifetime())
                .AddSessionFactory(databaseConfiguration, initialization)
                .AddEntitySystemServer();
        }

        public static FluentConfiguration AddEntitySystemFluentMappings<TEntity>(this FluentConfiguration fluentConfiguration)
        {
            fluentConfiguration.Mappings(c => c.AutoMappings.Add(AutoMap.AssemblyOf<TEntity>(new AutoMappingConfiguration())
                    .Conventions.Add<IndexedPropertyConvention>()
                    .Conventions.Add<StringColumnLengthConvention>()));

            return fluentConfiguration;
        }

        public static Configuration BuildEntitySystemFluentConfiguration(this FluentConfiguration fluentConfiguration, bool standardOutput = false)
        {
            var configuration = fluentConfiguration.BuildConfiguration();

            var exporter = new SchemaUpdate(configuration);

            exporter.Execute(standardOutput, true);

            return configuration;
        }

        private static IServiceCollection AddSessionFactory(this IServiceCollection serviceCollection, Configuration databaseConfiguration, Action<ISession> initialization = null)
        {
            var sessionFactory = databaseConfiguration.BuildSessionFactory();

            serviceCollection.AddSingleton(sessionFactory);

            if (initialization == null) return serviceCollection;

            using var session = sessionFactory.OpenSession();

            initialization(session);

            return serviceCollection;
        }
    }
}