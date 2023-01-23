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
#if NETFRAMEWORK
					var attachDbFileNameWithoutDataDirectorySubstitution = sqlConnectionStringBuilder.AttachDBFilename.Replace($"|{Global.DataDirectoryName}|", string.Empty);
#else
					var attachDbFileNameWithoutDataDirectorySubstitution = sqlConnectionStringBuilder.AttachDBFilename.Replace($"|{Global.DataDirectoryName}|", string.Empty, StringComparison.OrdinalIgnoreCase);
#endif
					var databaseFileName = attachDbFileNameWithoutDataDirectorySubstitution.Trim('\\', '/');
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