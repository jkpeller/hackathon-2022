using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductCategoryUpdateRequest
	{
		public ProductCategoryStatusType StatusTypeId { get; set; }
		public ProductCategoryVerificationType? VerificationTypeId { get; set; }
		public bool Primary { get; set; }
	}
}
