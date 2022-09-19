using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class VendorSearchRequest : BaseSearchRequest
	{
		public string TextFilter { get; set; }

		public List<int> CategoryIds{ get; set; }

		public List<int> CountryIds { get; set; }
	}
}
