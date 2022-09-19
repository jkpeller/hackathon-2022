using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductCategoryDto
	{
		public int ProductCategoryId { get; set; }
		public ProductCategoryStatusType StatusTypeId { get; set; }
		public string StatusTypeName { get; set; }
	}
}
