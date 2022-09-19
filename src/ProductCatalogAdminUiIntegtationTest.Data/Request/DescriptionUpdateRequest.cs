using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class DescriptionUpdateRequest
	{
		public ContentType ContentTypeId { get; set; }
		public string Description { get; set; }
	}
}