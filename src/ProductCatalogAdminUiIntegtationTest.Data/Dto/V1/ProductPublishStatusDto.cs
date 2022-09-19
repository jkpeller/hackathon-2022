using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductPublishStatusDto : PublishStatusDto
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public DateTimeOffset ModifiedOnUtc { get; set; }
	}
}
