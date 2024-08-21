using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace RegionOrebroLan.Platina.Data.Sqlite
{
	public class SqlitePlatinaContext(IGuidFactory guidFactory, DbContextOptions<SqlitePlatinaContext> options, ISystemClock systemClock) : PlatinaContext<SqlitePlatinaContext>(guidFactory, options, systemClock) { }
}