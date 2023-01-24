using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.Internal
{
	/// <summary>
	/// Class used when creating migrations.
	/// </summary>
	public class SqlServerDatabaseContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlServerDatabaseContext>
	{
		#region Methods

		public SqlServerDatabaseContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqlServerDatabaseContext>();
			optionsBuilder.UseSqlServer("A value that can not be empty just to be able to create/update migrations.");

			return new SqlServerDatabaseContext(new GuidFactory(), optionsBuilder.Options, new SystemClock());
		}

		#endregion
	}
}