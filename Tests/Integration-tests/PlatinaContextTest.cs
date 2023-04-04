using System;
using System.Linq;
using System.Threading.Tasks;
using IntegrationTests.Fakes;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.Builder.Extensions;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;
using RegionOrebroLan.Platina.Data.Entities;

namespace IntegrationTests
{
	[TestClass]
	public class PlatinaContextTest
	{
		#region Methods

		protected internal virtual async Task<ServiceProvider> CreateServiceProviderAsync(DatabaseProvider databaseProvider, bool useFakes)
		{
			var services = await this.CreateServicesAsync(databaseProvider, useFakes);

			var serviceProvider = services.BuildServiceProvider();

			var applicationBuilder = new ApplicationBuilder(serviceProvider);

			applicationBuilder.UsePlatinaContext();

			return serviceProvider;
		}

		protected internal virtual async Task<IServiceCollection> CreateServicesAsync(DatabaseProvider databaseProvider, bool useFakes)
		{
			var services = Global.CreateServices();

			if(useFakes)
			{
				services.AddSingleton<IGuidFactory, FakedGuidFactory>();
				services.AddSingleton<ISystemClock, FakedSystemClock>();
			}

			switch(databaseProvider)
			{
				case DatabaseProvider.Sqlite:
				{
					services.AddSqlitePlatinaContext(builder => builder.UseSqlite(Global.Configuration.GetConnectionString("Sqlite")));
					break;
				}
				case DatabaseProvider.SqlServer:
				{
					var connectionString = Global.Configuration.GetConnectionString("SqlServer");
					connectionString = await SqlServerConnectionStringResolver.ResolveAsync(connectionString);

					services.AddSqlServerPlatinaContext(builder => builder.UseSqlServer(connectionString));
					break;
				}
				default:
					throw new InvalidOperationException($"{databaseProvider} is not a valid database-provider.");
			}

			return services;
		}

		[ClassInitialize]
		public static async Task InitializeAsync(TestContext _)
		{
			await DatabaseHelper.DeleteDatabasesAsync();
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_ShouldResolveCreatedAndGuidAndSaved()
		{
			await Task.CompletedTask;

			this.SaveChangesShouldResolveCreatedAndGuidAndSaved(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_WithAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			this.SaveChangesWithAutoDetectChangesShouldWorkProperly(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			this.SaveChangesWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_ShouldResolveCreatedAndGuidAndSaved()
		{
			await Task.CompletedTask;

			this.SaveChangesShouldResolveCreatedAndGuidAndSaved(DatabaseProvider.SqlServer);
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_WithAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			this.SaveChangesWithAutoDetectChangesShouldWorkProperly(DatabaseProvider.SqlServer);
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			this.SaveChangesWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider.SqlServer);
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_ShouldResolveCreatedAndGuidAndSaved()
		{
			await this.SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_WithAutoDetectChangesShouldWorkProperly()
		{
			await this.SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await this.SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_ShouldResolveCreatedAndGuidAndSaved()
		{
			await this.SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(DatabaseProvider.SqlServer);
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_WithAutoDetectChangesShouldWorkProperly()
		{
			await this.SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(DatabaseProvider.SqlServer);
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await this.SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider.SqlServer);
		}

		protected internal virtual async Task SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = await this.CreateServiceProviderAsync(databaseProvider, true))
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					var created = DateTime.UtcNow;
					((FakedSystemClock)platinaContext.SystemClock).UtcNow = created;
					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(((FakedGuidFactory)platinaContext.GuidFactory).Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(created, document.Saved);

					var saved = created.AddHours(2);
					((FakedSystemClock)platinaContext.SystemClock).UtcNow = saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(((FakedGuidFactory)platinaContext.GuidFactory).Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(saved, document.Saved);
				}
			}

			await DatabaseHelper.DeleteDatabasesAsync();
		}

		protected internal virtual async Task SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = await this.CreateServiceProviderAsync(databaseProvider, false))
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					Assert.IsTrue(platinaContext.ChangeTracker.AutoDetectChangesEnabled);

					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var firstSaved = document.Saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var secondSaved = document.Saved;

					Assert.IsTrue(secondSaved > firstSaved);
				}
			}

			await DatabaseHelper.DeleteDatabasesAsync();
		}

		protected internal virtual async Task SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = await this.CreateServiceProviderAsync(databaseProvider, false))
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					var autoDetectChangesEnabled = platinaContext.ChangeTracker.AutoDetectChangesEnabled;
					Assert.IsTrue(autoDetectChangesEnabled);

					platinaContext.ChangeTracker.AutoDetectChangesEnabled = false;

					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					platinaContext.ChangeTracker.DetectChanges();
					var numberOfChanges = platinaContext.ChangeTracker.Entries().Count(entry => entry.State is EntityState.Added or EntityState.Modified);
					Assert.AreEqual(1, numberOfChanges);
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var firstSaved = document.Saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					platinaContext.ChangeTracker.DetectChanges();
					numberOfChanges = platinaContext.ChangeTracker.Entries().Count(entry => entry.State is EntityState.Added or EntityState.Modified);
					Assert.AreEqual(1, numberOfChanges);
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var secondSaved = document.Saved;

					Assert.IsTrue(secondSaved > firstSaved);

					platinaContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
				}
			}

			await DatabaseHelper.DeleteDatabasesAsync();
		}

		protected internal virtual void SaveChangesShouldResolveCreatedAndGuidAndSaved(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = this.CreateServiceProviderAsync(databaseProvider, true).Result)
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					var created = DateTime.UtcNow;
					((FakedSystemClock)platinaContext.SystemClock).UtcNow = created;
					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					Assert.AreEqual(1, platinaContext.SaveChanges());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(((FakedGuidFactory)platinaContext.GuidFactory).Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(created, document.Saved);

					var saved = created.AddHours(2);
					((FakedSystemClock)platinaContext.SystemClock).UtcNow = saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					Assert.AreEqual(1, platinaContext.SaveChanges());
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(((FakedGuidFactory)platinaContext.GuidFactory).Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(saved, document.Saved);
				}
			}

			DatabaseHelper.DeleteDatabasesAsync().Wait();
		}

		protected internal virtual void SaveChangesWithAutoDetectChangesShouldWorkProperly(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = this.CreateServiceProviderAsync(databaseProvider, false).Result)
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					Assert.IsTrue(platinaContext.ChangeTracker.AutoDetectChangesEnabled);

					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					Assert.AreEqual(1, platinaContext.SaveChanges());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var firstSaved = document.Saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					Assert.AreEqual(1, platinaContext.SaveChanges());
					document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var secondSaved = document.Saved;

					Assert.IsTrue(secondSaved > firstSaved);
				}
			}

			DatabaseHelper.DeleteDatabasesAsync().Wait();
		}

		protected internal virtual void SaveChangesWithoutAutoDetectChangesShouldWorkProperly(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = this.CreateServiceProviderAsync(databaseProvider, false).Result)
			{
				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					var autoDetectChangesEnabled = platinaContext.ChangeTracker.AutoDetectChangesEnabled;
					Assert.IsTrue(autoDetectChangesEnabled);

					platinaContext.ChangeTracker.AutoDetectChangesEnabled = false;

					const int id = 10;

					platinaContext.Add(new Document { Category = "Category-A", Id = id, Organization = "Organization-A" });
					platinaContext.ChangeTracker.DetectChanges();
					var numberOfChanges = platinaContext.ChangeTracker.Entries().Count(entry => entry.State is EntityState.Added or EntityState.Modified);
					Assert.AreEqual(1, numberOfChanges);
					Assert.AreEqual(1, platinaContext.SaveChanges());
					var document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var firstSaved = document.Saved;

					document = platinaContext.Documents.First();
					document.Category += " (some more)";
					platinaContext.ChangeTracker.DetectChanges();
					numberOfChanges = platinaContext.ChangeTracker.Entries().Count(entry => entry.State is EntityState.Added or EntityState.Modified);
					Assert.AreEqual(1, numberOfChanges);
					Assert.AreEqual(1, platinaContext.SaveChanges());
					document = platinaContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					var secondSaved = document.Saved;

					Assert.IsTrue(secondSaved > firstSaved);

					platinaContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
				}
			}

			DatabaseHelper.DeleteDatabasesAsync().Wait();
		}

		[TestMethod]
		public async Task Sqlite_Test()
		{
			await this.Test(DatabaseProvider.Sqlite);
		}

		[TestMethod]
		public async Task SqlServer_Test()
		{
			await this.Test(DatabaseProvider.SqlServer);
		}

		protected internal virtual async Task Test(DatabaseProvider databaseProvider)
		{
			using(var serviceProvider = await this.CreateServiceProviderAsync(databaseProvider, true))
			{
				var guidFactory = (FakedGuidFactory)serviceProvider.GetRequiredService<IGuidFactory>();
				var systemClock = (FakedSystemClock)serviceProvider.GetRequiredService<ISystemClock>();

				using(var platinaContext = serviceProvider.GetRequiredService<PlatinaContext>())
				{
					systemClock.UtcNow = DateTimeOffset.UtcNow;
					var created = systemClock.UtcNow.UtcDateTime;

					platinaContext.Add(new Document { Category = "Category-A", Organization = "Organization-A" });
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					var entry = platinaContext.Documents.First();
					Assert.AreEqual(created, entry.Created);
					Assert.AreEqual(guidFactory.Guids.ElementAt(0), entry.Guid);
					Assert.AreEqual(created, entry.Saved);

					var saved = created.AddHours(2);
					systemClock.UtcNow = saved;

					entry = platinaContext.Documents.First();
					entry.Title += " (some more)";
					Assert.AreEqual(1, await platinaContext.SaveChangesAsync());
					Assert.AreEqual(created, entry.Created);
					Assert.AreEqual(guidFactory.Guids.ElementAt(0), entry.Guid);
					Assert.AreEqual(saved, entry.Saved);
				}
			}

			await DatabaseHelper.DeleteDatabasesAsync();
		}

		#endregion
	}
}