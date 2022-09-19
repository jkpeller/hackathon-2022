using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductSearchRequest : BaseSearchRequest
	{
		public string TextFilter { get; set; }

		public List<int> ProductStatusIds { get; set; }
	}
}
