using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class VendorInsertRequest
	{
		public string Name { get; set; }
		public string WebsiteUrl { get; set; }
		public string YearFounded { get; set; }
		public string PhoneNumber { get; set; }
		public string About { get; set; }
		public AddressRequest Address { get; set; }
		public List<SocialMediaRequest> SocialMediaUrls { get; set; }
	}
}
