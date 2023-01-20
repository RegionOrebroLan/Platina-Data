using System;
using System.IO;
using System.Linq;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.Platina.Data;
using RegionOrebroLan.Platina.Data.Builder;
using RegionOrebroLan.Platina.Data.DependencyInjection.Extensions;

namespace IntegrationTests.Builder
{
	[TestClass]
	public class ApplicationBuilderExtensionTest
	{
		#region Methods

		[TestMethod]
		public void Sqlite_Test()
		{
			var services = new ServiceCollection();
			services.AddSqliteDatabaseContext(builder => builder.UseSqlite(Global.Configuration.GetConnectionString("SQLite")));

			var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
			applicationBuilder.UseSqliteDatabaseContext();

			// ReSharper disable ConvertToUsingDeclaration
			using(var scope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var sqliteDatabaseContext = scope.ServiceProvider.GetRequiredService<SqliteDatabaseContext>();

				Assert.IsFalse(sqliteDatabaseContext.Documents.Any());

				sqliteDatabaseContext.Database.EnsureDeleted();
			}
			// ReSharper restore ConvertToUsingDeclaration
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

			var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
			applicationBuilder.UseSqlServerDatabaseContext();

			// ReSharper disable ConvertToUsingDeclaration
			using(var scope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var sqlServerDatabaseContext = scope.ServiceProvider.GetRequiredService<SqlServerDatabaseContext>();

				Assert.IsFalse(sqlServerDatabaseContext.Documents.Any());

				sqlServerDatabaseContext.Database.EnsureDeleted();
			}
			// ReSharper restore ConvertToUsingDeclaration

			AppDomain.CurrentDomain.SetData(Global.DataDirectoryName, originalDataDirectoryPath);
			Directory.Delete(dataDirectoryPath, true);
		}

		#endregion
	}
}