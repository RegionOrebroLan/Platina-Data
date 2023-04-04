using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.Builder.Extensions;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace IntegrationTests.Builder.Extensions
{
	[TestClass]
	public class ApplicationBuilderExtensionTest
	{
		#region Methods

		[ClassInitialize]
		public static async Task InitializeAsync(TestContext _)
		{
			await DatabaseHelper.DeleteDatabasesAsync();
		}

		protected internal virtual async Task UsePlatinaContext_Sqlite_Test(ServiceLifetime serviceLifetime)
		{
			var services = new ServiceCollection();
			services.AddSqlitePlatinaContext(builder => builder.UseSqlite(Global.Configuration.GetConnectionString("Sqlite")), serviceLifetime, serviceLifetime);

			using(var serviceProvider = services.BuildServiceProvider())
			{
				var applicationBuilder = new ApplicationBuilder(serviceProvider);
				applicationBuilder.UsePlatinaContext();

				using(var scope = applicationBuilder.ApplicationServices.CreateScope())
				{
					var sqlitePlatinaContext = scope.ServiceProvider.GetRequiredService<PlatinaContext>();

					Assert.IsFalse(sqlitePlatinaContext.Documents.Any());

					await sqlitePlatinaContext.Database.EnsureDeletedAsync();
				}
			}
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqliteAndScopedLifetime_Test()
		{
			await this.UsePlatinaContext_Sqlite_Test(ServiceLifetime.Scoped);
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqliteAndSingletonLifetime_Test()
		{
			await this.UsePlatinaContext_Sqlite_Test(ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqliteAndTransientLifetime_Test()
		{
			await this.UsePlatinaContext_Sqlite_Test(ServiceLifetime.Transient);
		}

		protected internal virtual async Task UsePlatinaContext_SqlServer_Test(ServiceLifetime serviceLifetime)
		{
			var connectionString = Global.Configuration.GetConnectionString("SqlServer");
			connectionString = await SqlServerConnectionStringResolver.ResolveAsync(connectionString);

			var services = new ServiceCollection();
			services.AddSqlServerPlatinaContext(builder => builder.UseSqlServer(connectionString), serviceLifetime, serviceLifetime);

			using(var serviceProvider = services.BuildServiceProvider())
			{
				var applicationBuilder = new ApplicationBuilder(serviceProvider);
				applicationBuilder.UsePlatinaContext();

				using(var scope = applicationBuilder.ApplicationServices.CreateScope())
				{
					var sqlServerPlatinaContext = scope.ServiceProvider.GetRequiredService<PlatinaContext>();

					Assert.IsFalse(sqlServerPlatinaContext.Documents.Any());

					await sqlServerPlatinaContext.Database.EnsureDeletedAsync();
				}
			}
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqlServerAndScopedLifetime_Test()
		{
			await this.UsePlatinaContext_SqlServer_Test(ServiceLifetime.Scoped);
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqlServerAndSingletonLifetime_Test()
		{
			await this.UsePlatinaContext_SqlServer_Test(ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task UsePlatinaContext_SqlServerAndTransientLifetime_Test()
		{
			await this.UsePlatinaContext_SqlServer_Test(ServiceLifetime.Transient);
		}

		#endregion
	}
}