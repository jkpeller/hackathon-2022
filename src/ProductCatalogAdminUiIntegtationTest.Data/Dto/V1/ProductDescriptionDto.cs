using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductDescriptionDto
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public List<ContentDto> Descriptions { get; set; }
	}
}
