using System;
using System.IO;
using Microsoft.Data.SqlClient;

namespace IntegrationTests.Helpers
{
	public static class SqlServerHelper
	{
		#region Methods

		public static string ResolveConnectionString(string connectionString, string dataDirectoryPath)
		{
			// ReSharper disable InvertIf
			if(connectionString != null)
			{
				var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

				if(string.IsNullOrEmpty(sqlConnectionStringBuilder.InitialCatalog))
				{
					var databaseFileName = sqlConnectionStringBuilder.AttachDBFilename.Replace($"|{Global.DataDirectoryName}|", string.Empty, StringComparison.OrdinalIgnoreCase).Trim('\\', '/');
					sqlConnectionStringBuilder.InitialCatalog = Path.Combine(dataDirectoryPath, databaseFileName);
					connectionString = sqlConnectionStringBuilder.ConnectionString;
				}
			}
			// ReSharper restore InvertIf

			return connectionString;
		}

		#endregion
	}
}