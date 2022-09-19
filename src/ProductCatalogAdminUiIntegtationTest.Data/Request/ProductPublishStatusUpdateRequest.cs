using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductPublishStatusUpdateRequest
	{
		public int SiteId { get; set; }
		public PublishStatusType PublishStatusTypeId { get; set; }
	}
}