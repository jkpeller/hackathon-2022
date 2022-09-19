using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CategorySummaryDto
	{
		public int CategoryId { get; set; }
		public int ProductCategoryId { get; set; }
		public string CategoryName { get; set; }
		public DateTimeOffset DateAddedUtc { get; set; }
		public ProductCategoryStatusType ProductCategoryStatusTypeId { get; set; }
	}
}
