using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductDescriptionsUpdateRequest
	{
		public List<ProductSiteDescriptionsUpdateRequest> Sites { get; set; }
	}
}