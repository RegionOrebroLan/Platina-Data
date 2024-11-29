using System;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace IntegrationTests
{
	[TestClass]
	public class OrganizationContextFactoryTest
	{
		#region Methods

		[TestMethod]
		public async Task Create_Once_SqliteAndIfContextLifetimeIsScope_ShouldWork()
		{
			await this.Create_Sqlite_Test(false, ServiceLifetime.Scoped);
		}

		[TestMethod]
		public async Task Create_Once_SqliteAndIfContextLifetimeIsSingleton_ShouldWork()
		{
			await this.Create_Sqlite_Test(false, ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task Create_Once_SqliteAndIfContextLifetimeIsTransient_ShouldWork()
		{
			await this.Create_Sqlite_Test(false, ServiceLifetime.Transient);
		}

		[TestMethod]
		public async Task Create_Once_SqlServerAndIfContextLifetimeIsScope_ShouldWork()
		{
			await this.Create_SqlServer_Test(false, ServiceLifetime.Scoped);
		}

		[TestMethod]
		public async Task Create_Once_SqlServerAndIfContextLifetimeIsSingleton_ShouldWork()
		{
			await this.Create_SqlServer_Test(false, ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task Create_Once_SqlServerAndIfContextLifetimeIsTransient_ShouldWork()
		{
			await this.Create_SqlServer_Test(false, ServiceLifetime.Transient);
		}

		protected internal virtual async Task Create_Sqlite_Test(bool createTwice, ServiceLifetime serviceLifetime)
		{
			var services = new ServiceCollection();
			services.AddSqlitePlatinaContext(builder => builder.UseSqlite(Global.Configuration.GetConnectionString("Sqlite")), serviceLifetime, serviceLifetime);

			await this.Create_Test(createTwice, services);
		}

		protected internal virtual async Task Create_SqlServer_Test(bool createTwice, ServiceLifetime serviceLifetime)
		{
			var connectionString = Global.Configuration.GetConnectionString("SqlServer");
			connectionString = await SqlServerConnectionStringResolver.ResolveAsync(connectionString);

			var services = new ServiceCollection();
			services.AddSqlServerPlatinaContext(builder => builder.UseSqlServer(connectionString), serviceLifetime, serviceLifetime);

			await this.Create_Test(createTwice, services);
		}

		protected internal virtual async Task Create_Test(bool createTwice, IServiceCollection services)
		{
			using(var serviceProvider = services.BuildServiceProvider())
			{
				var applicationBuilder = new ApplicationBuilder(serviceProvider);

				using(var scope = applicationBuilder.ApplicationServices.CreateScope())
				{
					await scope.ServiceProvider.GetRequiredService<PlatinaContext>().Database.MigrateAsync();
				}

				try
				{
					var platinaContextFactory = serviceProvider.GetRequiredService<IPlatinaContextFactory>();

					using(var platinaContext = platinaContextFactory.Create())
					{
						_ = platinaContext.Documents.Count();
					}

					if(createTwice)
					{
						using(var platinaContext = platinaContextFactory.Create())
						{
							_ = platinaContext.Documents.Count();
						}
					}
				}
				finally
				{
					await DatabaseHelper.DeleteDatabasesAsync();
				}
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectDisposedException))]
		public async Task Create_Twice_SqliteAndIfContextLifetimeIsScope_ShouldThrowAnObjectDisposedException()
		{
			await this.Create_Sqlite_Test(true, ServiceLifetime.Scoped);
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectDisposedException))]
		public async Task Create_Twice_SqliteAndIfContextLifetimeIsSingleton_ShouldThrowAnObjectDisposedException()
		{
			await this.Create_Sqlite_Test(true, ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task Create_Twice_SqliteAndIfContextLifetimeIsTransient_ShouldWork()
		{
			await this.Create_Sqlite_Test(true, ServiceLifetime.Transient);
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectDisposedException))]
		public async Task Create_Twice_SqlServerAndIfContextLifetimeIsScope_ShouldThrowAnObjectDisposedException()
		{
			await this.Create_SqlServer_Test(true, ServiceLifetime.Scoped);
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectDisposedException))]
		public async Task Create_Twice_SqlServerAndIfContextLifetimeIsSingleton_ShouldThrowAnObjectDisposedException()
		{
			await this.Create_SqlServer_Test(true, ServiceLifetime.Singleton);
		}

		[TestMethod]
		public async Task Create_Twice_SqlServerAndIfContextLifetimeIsTransient_ShouldWork()
		{
			await this.Create_SqlServer_Test(true, ServiceLifetime.Transient);
		}

		[ClassInitialize]
		public static async Task InitializeAsync(TestContext _)
		{
			await DatabaseHelper.DeleteDatabasesAsync();
		}

		#endregion
	}
}