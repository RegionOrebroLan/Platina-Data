using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace IntegrationTests.Helpers
{
	public static class SqlServerConnectionStringResolver
	{
		#region Fields

		private const string _localDatabasePrefix = "(LocalDb)";

		#endregion

		#region Methods

		private static async Task<bool> IsLocalDatabaseConnectionStringAsync(SqlConnectionStringBuilder sqlConnectionStringBuilder)
		{
			if(sqlConnectionStringBuilder == null)
				throw new ArgumentNullException(nameof(sqlConnectionStringBuilder));

			return await Task.FromResult(sqlConnectionStringBuilder.DataSource.StartsWith(_localDatabasePrefix, StringComparison.OrdinalIgnoreCase));
		}

		public static async Task<string> ResolveAsync(string connectionString)
		{
			// ReSharper disable InvertIf
			if(!string.IsNullOrWhiteSpace(connectionString))
			{
				var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

				if(await IsLocalDatabaseConnectionStringAsync(sqlConnectionStringBuilder))
				{
					var attachDbFilename = sqlConnectionStringBuilder.AttachDBFilename;

					if(!Path.IsPathRooted(attachDbFilename))
					{
						attachDbFilename = Path.Combine(Global.HostEnvironment.ContentRootPath, attachDbFilename.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar));
						sqlConnectionStringBuilder.AttachDBFilename = attachDbFilename;

						if(string.IsNullOrEmpty(sqlConnectionStringBuilder.InitialCatalog))
							sqlConnectionStringBuilder.InitialCatalog = attachDbFilename;

						connectionString = sqlConnectionStringBuilder.ConnectionString;
					}
				}
			}
			// ReSharper restore InvertIf

			return connectionString;
		}

		#endregion
	}
}