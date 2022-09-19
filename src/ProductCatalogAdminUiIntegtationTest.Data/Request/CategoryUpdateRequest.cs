namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class CategoryUpdateRequest : CategoryInsertRequest
	{
		public int CategoryStatusId { get; set; }
	}
}
