using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class CategoryPublishStatusDto
	{
		public DateTimeOffset ModifiedOnUtc { get; set; }
		public string Name { get; set; }
		public PublishStatusType PublishStatusTypeId { get; set; }
		public int SiteId { get; set; }
		public string SiteName { get; set; }
	}
}