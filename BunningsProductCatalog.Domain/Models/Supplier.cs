using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BunningsProductCatalog.Domain.Models
{
	public class Supplier
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int SupplierId { get; set; }

		[Required]
		[MaxLength(300)]
		public string SupplierName { get; set; }

		[Required]
		[MaxLength(50)]
		public string SupplierCode { get; set; }

		public int CompanyId { get; set; }
		[ForeignKey("CompanyId")]
		public virtual Company Company { get; set; }

		public virtual ICollection<CompanyProductBarcode> CompanyProductBarcode { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

	}
}