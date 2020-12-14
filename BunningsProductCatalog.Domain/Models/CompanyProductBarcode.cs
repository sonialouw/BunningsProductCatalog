using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BunningsProductCatalog.Domain.Models
{
	public class CompanyProductBarcode
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CompanyProductBarcodeId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Barcode { get; set; }

		public int CompanyProductId { get; set; }
		[ForeignKey("CompanyProductId")]
		public virtual CompanyProduct CompanyProduct { get; set; }

		public int SupplierId { get; set; }
		[ForeignKey("SupplierId")]
		public virtual Supplier Supplier { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

	}
}