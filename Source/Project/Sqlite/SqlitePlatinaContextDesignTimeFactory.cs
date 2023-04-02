using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.Sqlite
{
	/// <summary>
	/// Class used when creating migrations.
	/// </summary>
	public class SqlitePlatinaContextDesignTimeFactory : IDesignTimeDbContextFactory<SqlitePlatinaContext>
	{
		#region Methods

		public SqlitePlatinaContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<SqlitePlatinaContext>();
			optionsBuilder.UseSqlite("A value that can not be empty just to be able to create/update migrations.");

			return new SqlitePlatinaContext(new GuidFactory(), optionsBuilder.Options, new SystemClock());
		}

		#endregion
	}
}