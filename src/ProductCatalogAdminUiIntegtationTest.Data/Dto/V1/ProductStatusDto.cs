using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductStatusDto : BaseSelectableDto
	{
		public int ProductId { get; set; }
		public ProductStatusType ProductStatusTypeId { get; set; }
		public string ProductStatusTypeName { get; set; }
	}
}
