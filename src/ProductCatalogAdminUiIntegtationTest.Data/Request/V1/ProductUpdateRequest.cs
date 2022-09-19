namespace ProductCatalogAdminUiIntegrationTest.Data.Request.V1
{
	public class ProductUpdateRequest : ProductInsertRequest
	{
		public int ProductStatusId { get; set; }
	}
}