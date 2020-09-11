using System;
using Microsoft.Extensions.Internal;

namespace IntegrationTests.Fakes
{
	public class FakedSystemClock : ISystemClock
	{
		#region Properties

		public virtual DateTimeOffset UtcNow { get; set; }

		#endregion
	}
}