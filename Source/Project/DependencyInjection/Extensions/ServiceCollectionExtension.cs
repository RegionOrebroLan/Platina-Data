using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;
using RegionOrebroLan.Platina.Data.Sqlite;
using RegionOrebroLan.Platina.Data.SqlServer;

namespace RegionOrebroLan.Platina.Data.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddPlatinaContext<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where T : PlatinaContext
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddPlatinaContextDependencies();
			services.AddDbContext<T>(optionsAction, contextLifetime, optionsLifetime);
			services.Add(new ServiceDescriptor(typeof(IPlatinaContext), serviceProvider => serviceProvider.GetService<PlatinaContext>(), contextLifetime));
			services.Add(new ServiceDescriptor(typeof(PlatinaContext), serviceProvider => serviceProvider.GetService<T>(), contextLifetime));
			services.AddSingleton<IPlatinaContextFactory, PlatinaContextFactory>();

			return services;
		}

		public static IServiceCollection AddPlatinaContextDependencies(this IServiceCollection services)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			services.TryAddSingleton<IGuidFactory, GuidFactory>();
			services.TryAddSingleton<ISystemClock, SystemClock>();

			return services;
		}

		public static IServiceCollection AddSqlitePlatinaContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
		{
			return services.AddPlatinaContext<SqlitePlatinaContext>(optionsAction, contextLifetime, optionsLifetime);
		}

		public static IServiceCollection AddSqlServerPlatinaContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
		{
			return services.AddPlatinaContext<SqlServerPlatinaContext>(optionsAction, contextLifetime, optionsLifetime);
		}

		#endregion
	}
}