using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductCategoriesUpsertRequest
	{
		public int ProductId { get; set; }
		public List<int> CategoryIds { get; set; }
	}
}
