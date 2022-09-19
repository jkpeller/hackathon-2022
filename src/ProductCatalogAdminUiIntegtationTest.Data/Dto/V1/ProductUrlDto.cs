using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductUrlDto
	{
		public int ProductUrlId { get; set; }
		public UrlType UrlTypeId { get; set; }
		public string Url { get; set; }
	}
}
