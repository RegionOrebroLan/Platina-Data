using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RegionOrebroLan.Platina.Data.Entities
{
	public class Document
	{
		#region Properties

		[MaxLength(500)]
		public virtual string Category { get; set; }

		/// <summary>
		/// Datetime from Platina, local or UTC not known.
		/// </summary>
		public virtual DateTime Confirmed { get; set; }

		/// <summary>
		/// Datetime UTC
		/// </summary>
		public virtual DateTime Created { get; set; }

		public virtual bool Disabled { get; set; }

		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual byte[] File { get; set; }

		[MaxLength(10)]
		public virtual string FileExtension { get; set; }

		[SuppressMessage("Naming", "CA1720:Identifier contains type name")]
		public virtual Guid Guid { get; set; }

		public virtual int Id { get; set; }

		/// <summary>
		/// Multiple values separated.
		/// </summary>
		[MaxLength(1000)]
		public virtual string KeywordIdentities { get; set; }

		/// <summary>
		/// Multiple values separated.
		/// </summary>
		[MaxLength(2000)]
		public virtual string Keywords { get; set; }

		public virtual string Organization { get; set; }

		[MaxLength(1000)]
		public virtual string OrganizationCode { get; set; }

		[SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
		public virtual byte[] PdfFile { get; set; }

		[MaxLength(10)]
		public virtual string PdfFileExtension { get; set; }

		public virtual int RevisionNumber { get; set; }

		/// <summary>
		/// Datetime UTC
		/// </summary>
		public virtual DateTime Saved { get; set; }

		[MaxLength(200)]
		public virtual string Title { get; set; }

		#endregion
	}
}