using System;
using System.IO;
using System.Linq;
using IntegrationTests.Fakes;
using IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;
using RegionOrebroLan.Platina.Data.Entities;

namespace IntegrationTests
{
	[TestClass]
	public class DatabaseContextTest
	{
		#region Methods

		[TestMethod]
		public void Sqlite_Test()
		{
			var services = new ServiceCollection();
			services.AddSqliteDatabaseContext(builder => builder.UseSqlite(Global.Configuration.GetConnectionString("SQLite")));
			this.Test<SqliteDatabaseContext>(services);
		}

		[TestMethod]
		public void SqlServer_Test()
		{
			var connectionString = Global.Configuration.GetConnectionString("SQLServer");
			var dataDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			var originalDataDirectoryPath = AppDomain.CurrentDomain.GetData(Global.DataDirectoryName);
			Directory.CreateDirectory(dataDirectoryPath);
			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, dataDirectoryPath);

			connectionString = SqlServerHelper.ResolveConnectionString(connectionString, dataDirectoryPath);

			var services = new ServiceCollection();
			services.AddSqlServerDatabaseContext(builder => builder.UseSqlServer(connectionString));

			this.Test<SqlServerDatabaseContext>(services);

			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, originalDataDirectoryPath);
			Directory.Delete(dataDirectoryPath, true);
		}

		protected internal virtual void Test<TDatabaseContext>(IServiceCollection services) where TDatabaseContext : DatabaseContextBase
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			var guidFactory = new FakedGuidFactory();
			var systemClock = new FakedSystemClock();

			services.AddSingleton<IGuidFactory>(guidFactory);
			services.AddSingleton<ISystemClock>(systemClock);

			// ReSharper disable ConvertToUsingDeclaration
			using(var scope = services.BuildServiceProvider().CreateScope())
			{
				var databaseContext = scope.ServiceProvider.GetRequiredService<TDatabaseContext>();

				try
				{
					databaseContext.Database.Migrate();

					var created = DateTime.UtcNow;
					systemClock.UtcNow = created;
					const int id = 10;

					databaseContext.Add(new Document {Category = "Category-A", Id = id, Organization = "Organization-A"});
					Assert.AreEqual(1, databaseContext.SaveChanges());
					var document = databaseContext.Documents.First();
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(guidFactory.Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(created, document.Saved);

					var saved = created.AddHours(2);
					systemClock.UtcNow = saved;

					document = databaseContext.Documents.First();
					document.Category += " (some more)";
					Assert.AreEqual(1, databaseContext.SaveChanges());
					Assert.AreEqual(id, document.Id);
					Assert.AreEqual(created, document.Created);
					Assert.AreEqual(guidFactory.Guids.ElementAt(0), document.Guid);
					Assert.AreEqual(saved, document.Saved);
				}
				finally
				{
					databaseContext.Database.EnsureDeleted();
				}
			}
			// ReSharper restore ConvertToUsingDeclaration
		}

		#endregion
	}
}