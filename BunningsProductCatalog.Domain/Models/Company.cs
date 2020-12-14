using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BunningsProductCatalog.Domain.Models
{
	public class Company
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CompanyId { get; set; }

		[Required]
		[MaxLength(10)]
		public string CompanyCode { get; set; }

		[Required]
		[MaxLength(300)]
		public string CompanyName { get; set; }

		public virtual ICollection<CompanyProduct> CompanyProducts { get; set; }
	}
}