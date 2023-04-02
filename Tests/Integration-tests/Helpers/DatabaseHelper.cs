using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Helpers
{
	public static class DatabaseHelper
	{
		#region Methods

		public static async Task DeleteDatabasesAsync()
		{
			var configuration = Global.Configuration;

			await DeleteSqliteDatabaseAsync(configuration);
			await DeleteSqlServerDatabaseAsync(configuration);
		}

		public static async Task DeleteSqliteDatabaseAsync(IConfiguration configuration)
		{
			configuration ??= Global.Configuration;

			var connectionString = configuration.GetConnectionString("Sqlite");

			var sqliteContextOptionsBuilder = new DbContextOptionsBuilder();
			sqliteContextOptionsBuilder.UseSqlite(connectionString);

			using(var context = new DbContext(sqliteContextOptionsBuilder.Options))
			{
				await context.Database.EnsureDeletedAsync();
			}
		}

		public static async Task DeleteSqlServerDatabaseAsync(IConfiguration configuration)
		{
			configuration ??= Global.Configuration;

			var connectionString = configuration.GetConnectionString("SqlServer");
			connectionString = await SqlServerConnectionStringResolver.ResolveAsync(connectionString);

			var sqlServerContextOptionsBuilder = new DbContextOptionsBuilder();
			sqlServerContextOptionsBuilder.UseSqlServer(connectionString);

			using(var context = new DbContext(sqlServerContextOptionsBuilder.Options))
			{
				await context.Database.EnsureDeletedAsync();
			}
		}

		#endregion
	}
}