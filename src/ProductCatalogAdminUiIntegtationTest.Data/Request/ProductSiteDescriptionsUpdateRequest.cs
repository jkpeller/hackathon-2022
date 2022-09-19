using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductSiteDescriptionsUpdateRequest
	{
		public SiteType SiteId { get; set; }
		public List<DescriptionUpdateRequest> Descriptions { get; set; }
	}
}