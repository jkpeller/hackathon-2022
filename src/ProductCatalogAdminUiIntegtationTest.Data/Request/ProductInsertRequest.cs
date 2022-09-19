using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductInsertRequest
	{
		public string Name { get; set; }
		public string ProductWebsiteUrl { get; set; }
		public int? VendorId { get; set; }
		public string ShortDescription { get; set; }
		public List<int> CategoryIds { get; set; }
		public TargetSourceSiteVendorRequest TargetSourceSiteVendor { get; set; }
	}
}