using System;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Platina.Data
{
	/// <summary>
	/// Needs a transient lifetime for the platina-context.
	/// </summary>
	public class PlatinaContextFactory : IPlatinaContextFactory
	{
		#region Constructors

		public PlatinaContextFactory(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		#endregion

		#region Properties

		protected internal virtual IServiceProvider ServiceProvider { get; }

		#endregion

		#region Methods

		public virtual IPlatinaContext Create()
		{
			return this.ServiceProvider.GetRequiredService<IPlatinaContext>();
		}

		#endregion
	}
}