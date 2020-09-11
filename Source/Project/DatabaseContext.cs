using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data
{
	public class DatabaseContext : DatabaseContextBase<DatabaseContext>
	{
		#region Constructors

		public DatabaseContext(IGuidFactory guidFactory, DbContextOptions<DatabaseContext> options, ISystemClock systemClock) : base(guidFactory, options, systemClock) { }

		#endregion
	}
}