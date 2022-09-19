using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class BaseUrlRequest
	{
		public UrlType UrlTypeId { get; set; }
		public string Url { get; set; }
	}
}