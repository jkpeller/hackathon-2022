using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductMediasUpdateRequest
	{
		public List<ProductSiteMediasUpdateRequest> Medias { get; set; }
	}
}
