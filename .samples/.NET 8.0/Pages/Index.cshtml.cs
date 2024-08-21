using Microsoft.AspNetCore.Mvc.RazorPages;
using RegionOrebroLan.Platina.Data;

namespace Application.Pages
{
	public class IndexModel(IPlatinaContextFactory platinaContextFactory) : PageModel
	{
		#region Properties

		public virtual Exception Exception { get; set; }
		public virtual string Message { get; set; }
		protected internal virtual IPlatinaContextFactory PlatinaContextFactory { get; } = platinaContextFactory ?? throw new ArgumentNullException(nameof(platinaContextFactory));

		#endregion

		#region Methods

		public void OnGet()
		{
			try
			{
				using(var platinaContext = this.PlatinaContextFactory.Create())
				{
					var numberOfDocuments = platinaContext.Documents.Count();

					this.Message = $"There are {numberOfDocuments} documents in the database.";
				}
			}
			catch(Exception exception)
			{
				this.Exception = exception;
			}
		}

		#endregion
	}
}