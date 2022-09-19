using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductStatusSelectableDto : BaseSelectableDto
	{
		public ProductStatusType ProductStatusTypeId { get; set; }
		public string Name { get; set; }
	}
}
