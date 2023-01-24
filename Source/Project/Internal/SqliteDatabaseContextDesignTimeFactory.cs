using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.Internal
{
	/// <summary>
	/// Class used when creating migrations.
	/// </summary>
	public class SqliteDatabaseContextDesignTimeFactory : IDesignTimeDbContextFactory<SqliteDatabaseContext>
	{
		#region Methods

		public SqliteDatabaseContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqliteDatabaseContext>();
			optionsBuilder.UseSqlite("A value that can not be empty just to be able to create/update migrations.");

			return new SqliteDatabaseContext(new GuidFactory(), optionsBuilder.Options, new SystemClock());
		}

		#endregion
	}
}