using IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
	// ReSharper disable All
	[TestClass]
	public class DatabaseContextTest
	{
		#region Methods

		[TestCleanup]
		public async Task Cleanup()
		{
			await Task.CompletedTask;
			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, Global.DataDirectoryPath);
		}

		[TestInitialize]
		public async Task Initialize()
		{
			await this.Cleanup();
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_ShouldResolveCreatedAndGuidAndSaved()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext(true))
			{
				this.SaveChangesShouldResolveCreatedAndGuidAndSaved(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_WithAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext())
			{
				this.SaveChangesWithAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChanges_Sqlite_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext())
			{
				this.SaveChangesWithoutAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_ShouldResolveCreatedAndGuidAndSaved()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext(true))
			{
				this.SaveChangesShouldResolveCreatedAndGuidAndSaved(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_WithAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext())
			{
				this.SaveChangesWithAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChanges_SqlServer_WithoutAutoDetectChangesShouldWorkProperly()
		{
			await Task.CompletedTask;

			using(var databaseTestContext = new SqliteTestContext())
			{
				this.SaveChangesWithoutAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_ShouldResolveCreatedAndGuidAndSaved()
		{
			using(var databaseTestContext = new SqliteTestContext(true))
			{
				await this.SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_WithAutoDetectChangesShouldWorkProperly()
		{
			using(var databaseTestContext = new SqliteTestContext())
			{
				await this.SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_Sqlite_WithoutAutoDetectChangesShouldWorkProperly()
		{
			using(var databaseTestContext = new SqliteTestContext())
			{
				await this.SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_ShouldResolveCreatedAndGuidAndSaved()
		{
			using(var databaseTestContext = new SqliteTestContext(true))
			{
				await this.SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_WithAutoDetectChangesShouldWorkProperly()
		{
			using(var databaseTestContext = new SqliteTestContext())
			{
				await this.SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		[TestMethod]
		public async Task SaveChangesAsync_SqlServer_WithoutAutoDetectChangesShouldWorkProperly()
		{
			using(var databaseTestContext = new SqliteTestContext())
			{
				await this.SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(databaseTestContext);
			}
		}

		protected internal virtual async Task SaveChangesAsyncShouldResolveCreatedAndGuidAndSaved(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				var created = DateTime.UtcNow;
				databaseTestContext.FakedSystemClock.UtcNow = created;
				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				Assert.AreEqual(created, document.Created);
				Assert.AreEqual(databaseTestContext.FakedGuidFactory.Guids.ElementAt(0), document.Guid);
				Assert.AreEqual(created, document.Saved);

				var saved = created.AddHours(2);
				databaseTestContext.FakedSystemClock.UtcNow = saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				Assert.AreEqual(id, document.Id);
				Assert.AreEqual(created, document.Created);
				Assert.AreEqual(databaseTestContext.FakedGuidFactory.Guids.ElementAt(0), document.Guid);
				Assert.AreEqual(saved, document.Saved);
			}
		}

		protected internal virtual async Task SaveChangesAsyncWithAutoDetectChangesShouldWorkProperly(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				Assert.IsTrue(databaseContext.ChangeTracker.AutoDetectChangesEnabled);

				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var firstSaved = document.Saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var secondSaved = document.Saved;

				Assert.IsTrue(secondSaved > firstSaved);
			}
		}

		protected internal virtual async Task SaveChangesAsyncWithoutAutoDetectChangesShouldWorkProperly(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				var autoDetectChangesEnabled = databaseContext.ChangeTracker.AutoDetectChangesEnabled;
				Assert.IsTrue(autoDetectChangesEnabled);

				databaseContext.ChangeTracker.AutoDetectChangesEnabled = false;

				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				databaseContext.ChangeTracker.DetectChanges();
				var numberOfChanges = databaseContext.ChangeTracker.Entries().Count(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);
				Assert.AreEqual(1, numberOfChanges);
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var firstSaved = document.Saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				databaseContext.ChangeTracker.DetectChanges();
				numberOfChanges = databaseContext.ChangeTracker.Entries().Count(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);
				Assert.AreEqual(1, numberOfChanges);
				Assert.AreEqual(1, await databaseContext.SaveChangesAsync());
				document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var secondSaved = document.Saved;

				Assert.IsTrue(secondSaved > firstSaved);

				databaseContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
			}
		}

		protected internal virtual void SaveChangesShouldResolveCreatedAndGuidAndSaved(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				var created = DateTime.UtcNow;
				databaseTestContext.FakedSystemClock.UtcNow = created;
				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				Assert.AreEqual(1, databaseContext.SaveChanges());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				Assert.AreEqual(created, document.Created);
				Assert.AreEqual(databaseTestContext.FakedGuidFactory.Guids.ElementAt(0), document.Guid);
				Assert.AreEqual(created, document.Saved);

				var saved = created.AddHours(2);
				databaseTestContext.FakedSystemClock.UtcNow = saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				Assert.AreEqual(1, databaseContext.SaveChanges());
				Assert.AreEqual(id, document.Id);
				Assert.AreEqual(created, document.Created);
				Assert.AreEqual(databaseTestContext.FakedGuidFactory.Guids.ElementAt(0), document.Guid);
				Assert.AreEqual(saved, document.Saved);
			}
		}

		protected internal virtual void SaveChangesWithAutoDetectChangesShouldWorkProperly(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				Assert.IsTrue(databaseContext.ChangeTracker.AutoDetectChangesEnabled);

				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				Assert.AreEqual(1, databaseContext.SaveChanges());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var firstSaved = document.Saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				Assert.AreEqual(1, databaseContext.SaveChanges());
				document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var secondSaved = document.Saved;

				Assert.IsTrue(secondSaved > firstSaved);
			}
		}

		protected internal virtual void SaveChangesWithoutAutoDetectChangesShouldWorkProperly(DatabaseTestContext databaseTestContext)
		{
			if(databaseTestContext == null)
				throw new ArgumentNullException(nameof(databaseTestContext));

			using(var scope = databaseTestContext.ServiceProvider.CreateScope())
			{
				var databaseContext = databaseTestContext.GetDatabaseContext(scope);

				var autoDetectChangesEnabled = databaseContext.ChangeTracker.AutoDetectChangesEnabled;
				Assert.IsTrue(autoDetectChangesEnabled);

				databaseContext.ChangeTracker.AutoDetectChangesEnabled = false;

				const int id = 10;

				databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
				databaseContext.ChangeTracker.DetectChanges();
				var numberOfChanges = databaseContext.ChangeTracker.Entries().Count(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);
				Assert.AreEqual(1, numberOfChanges);
				Assert.AreEqual(1, databaseContext.SaveChanges());
				var document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var firstSaved = document.Saved;

				document = databaseContext.Documents.First();
				document.Category += " (some more)";
				databaseContext.ChangeTracker.DetectChanges();
				numberOfChanges = databaseContext.ChangeTracker.Entries().Count(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);
				Assert.AreEqual(1, numberOfChanges);
				Assert.AreEqual(1, databaseContext.SaveChanges());
				document = databaseContext.Documents.First();
				Assert.AreEqual(id, document.Id);
				var secondSaved = document.Saved;

				Assert.IsTrue(secondSaved > firstSaved);

				databaseContext.ChangeTracker.AutoDetectChangesEnabled = autoDetectChangesEnabled;
			}
		}

		#endregion
	}
	// ReSharper restore All
}