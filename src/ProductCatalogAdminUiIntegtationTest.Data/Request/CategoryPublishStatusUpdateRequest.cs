using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class CategoryPublishStatusUpdateRequest
	{
		public PublishStatusType PublishStatusTypeId { get; set; }
		public SiteType SiteId { get; set; }
	}
}
