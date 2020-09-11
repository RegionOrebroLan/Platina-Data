using System;

namespace RegionOrebroLan.Platina.Data
{
	public class GuidFactory : IGuidFactory
	{
		#region Methods

		public virtual Guid Create()
		{
			return Guid.NewGuid();
		}

		#endregion
	}
}