using System;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Platina.Data
{
	/// <summary>
	/// Needs a transient lifetime for the platina-context.
	/// </summary>
	public class PlatinaContextFactory(IServiceProvider serviceProvider) : IPlatinaContextFactory
	{
		#region Properties

		protected internal virtual IServiceProvider ServiceProvider { get; } = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

		#endregion

		#region Methods

		public virtual IPlatinaContext Create()
		{
			return this.ServiceProvider.GetRequiredService<IPlatinaContext>();
		}

		#endregion
	}
}