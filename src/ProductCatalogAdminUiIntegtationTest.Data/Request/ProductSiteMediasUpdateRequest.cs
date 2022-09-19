using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductSiteMediasUpdateRequest
	{
		public int SiteId { get; set; }
		public List<ProductScreenshotMediaUpdateRequest> Screenshots { get; set; }
		public List<ProductVideoMediaUpdateRequest> Videos { get; set; }
	}
}
