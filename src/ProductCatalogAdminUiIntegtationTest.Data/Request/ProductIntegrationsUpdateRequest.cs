using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductIntegrationsUpdateRequest
	{
		public VerifiedType OpenApiOfferedVerifiedTypeId { get; set; }
		public List<int> IntegrationProductIds { get; set; }
	}
}