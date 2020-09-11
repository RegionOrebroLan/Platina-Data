using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddDatabaseContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, bool forceDependencies = false, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
		{
			return services.AddDatabaseContextInternal<DatabaseContext>(optionsAction, forceDependencies, contextLifetime, optionsLifetime);
		}

		public static IServiceCollection AddDatabaseContextDependencies(this IServiceCollection services, bool force = false)
		{
			services.AddSingletonInternal<IGuidFactory, GuidFactory>(force);
			return services.AddSingletonInternal<ISystemClock, SystemClock>(force);
		}

		private static IServiceCollection AddDatabaseContextInternal<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, bool forceDependencies = false, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where T : DatabaseContextBase
		{
			services.AddDatabaseContextDependencies(forceDependencies);
			return services.AddDbContext<T>(optionsAction, contextLifetime, optionsLifetime);
		}

		private static IServiceCollection AddSingletonInternal<TService, TImplementation>(this IServiceCollection services, bool force) where TService : class where TImplementation : class, TService
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(force)
				services.AddSingleton<TService, TImplementation>();
			else
				services.TryAddSingleton<TService, TImplementation>();

			return services;
		}

		public static IServiceCollection AddSqliteDatabaseContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, bool forceDependencies = false, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
		{
			return services.AddDatabaseContextInternal<SqliteDatabaseContext>(optionsAction, forceDependencies, contextLifetime, optionsLifetime);
		}

		public static IServiceCollection AddSqlServerDatabaseContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, bool forceDependencies = false, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
		{
			return services.AddDatabaseContextInternal<SqlServerDatabaseContext>(optionsAction, forceDependencies, contextLifetime, optionsLifetime);
		}

		#endregion
	}
}