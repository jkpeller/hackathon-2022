using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class PricingModelDto : BaseSelectableDto
	{
		public PricingModelType PricingModelTypeId { get; set; }
		public string Name { get; set; }
    }
}
