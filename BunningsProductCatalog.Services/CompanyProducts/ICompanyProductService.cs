using BunningsProductCatalog.Domain.Models;
using BunningsProductCatalog.Services.Data.Common;
using BunningsProductCatalog.Services.Data.CompanyProducts.Requests;
using BunningsProductCatalog.Services.Data.CompanyProducts.Results;
using System.Collections.Generic;

namespace BunningsProductCatalog.Services.CompanyProducts
{
	public interface ICompanyProductService
	{
		ImportCompanyProductResult ImportCompanyProductsFromFileStream(ImportCompanyProductRequest request);
		CreateCompanyProductResult CreateCompanyProduct(CreateCompanyProductRequest request);
		IEnumerable<Error> ValidateCompanyProductExist(string productSku, string companyCode);
		DeleteCompanyProductResult DeleteCompanyProduct(DeleteCompanyProductRequest request);
	}
}
