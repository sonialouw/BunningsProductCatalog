using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BunningsProductCatalog.Domain.Models
{
	public class CompanyProduct
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CompanyProductId { get; set; }

		[Required]
		[MaxLength(300)]
		public string ProductName { get; set; }

		[Required]
		[MaxLength(100)]
		public string ProductSku { get; set; }

		public int CompanyId { get; set; }
		[ForeignKey("CompanyId")]
		public virtual Company Company { get; set; }

		public virtual ICollection<CompanyProductBarcode> CompanyProductBarcodes { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		[Required]
		public bool IsDeleted { get; set; }
	}
}