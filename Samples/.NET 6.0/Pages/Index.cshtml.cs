using Microsoft.AspNetCore.Mvc.RazorPages;
using RegionOrebroLan.Platina.Data;

namespace Application.Pages
{
	public class IndexModel : PageModel
	{
		#region Constructors

		public IndexModel(DatabaseContextBase databaseContext)
		{
			this.DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
		}

		#endregion

		#region Properties

		protected internal virtual DatabaseContextBase DatabaseContext { get; }
		public virtual Exception Exception { get; set; }
		public virtual string Message { get; set; }

		#endregion

		#region Methods

		public void OnGet()
		{
			try
			{
				var numberOfDocuments = this.DatabaseContext.Documents.Count();

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