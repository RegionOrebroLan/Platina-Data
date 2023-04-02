namespace RegionOrebroLan.Platina.Data
{
	/// <summary>
	/// Needs a transient lifetime for the platina-context.
	/// </summary>
	public interface IPlatinaContextFactory
	{
		#region Methods

		IPlatinaContext Create();

		#endregion
	}
}