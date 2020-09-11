using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Platina.Data.Builder
{
	public static class ApplicationBuilderExtension
	{
		#region Methods

		public static IApplicationBuilder UseDatabaseContext(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseDatabaseContextInternal<DatabaseContext>();
		}

		private static IApplicationBuilder UseDatabaseContextInternal<T>(this IApplicationBuilder applicationBuilder) where T : DatabaseContextBase
		{
			if(applicationBuilder == null)
				throw new ArgumentNullException(nameof(applicationBuilder));

			// ReSharper disable ConvertToUsingDeclaration
			using(var scope = applicationBuilder.ApplicationServices.CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<T>().Database.Migrate();
			}
			// ReSharper restore ConvertToUsingDeclaration

			return applicationBuilder;
		}

		public static IApplicationBuilder UseSqliteDatabaseContext(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseDatabaseContextInternal<SqliteDatabaseContext>();
		}

		public static IApplicationBuilder UseSqlServerDatabaseContext(this IApplicationBuilder applicationBuilder)
		{
			return applicationBuilder.UseDatabaseContextInternal<SqlServerDatabaseContext>();
		}

		#endregion
	}
}