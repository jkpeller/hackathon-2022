using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductMediaDto
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public List<ProductScreenshotDto> Screenshots { get; set; }
		public List<ProductVideoDto> Videos { get; set; }
	}
}
