using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegionOrebroLan.Platina.Data;

namespace Application.Pages
{
	public class IndexModel : PageModel
	{
		#region Constructors

		public IndexModel(IPlatinaContext platinaContext)
		{
			this.PlatinaContext = platinaContext ?? throw new ArgumentNullException(nameof(platinaContext));
		}

		#endregion

		#region Properties

		public virtual Exception Exception { get; set; }
		public virtual string Message { get; set; }
		protected internal virtual IPlatinaContext PlatinaContext { get; }

		#endregion

		#region Methods

		public void OnGet()
		{
			try
			{
				var numberOfDocuments = this.PlatinaContext.Documents.Count();

				this.Message = $"There are {numberOfDocuments} documents in the database.";
			}
			catch(Exception exception)
			{
				this.Exception = exception;
			}
		}

		#endregion
	}
}