using System.Collections.Generic;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ListingRequestUpdateRequest
	{
		public int StatusTypeId { get; set; }
		public string CompanyName { get; set; }
		public string CompanyWebsiteUrl { get; set; }
		public string CompanyPhoneNumber { get; set; }
		public List<SocialMediaRequest> SocialMediaUrls { get; set; }
		public AddressRequest CompanyAddress { get; set; }
		public string ProductName { get; set; }
		public string ProductWebsiteUrl { get; set; }
		public string ProductShortDescription { get; set; }
		public int ProductAcceptedCategoryId { get; set; }
		public ListingRequestDenialReasonType? DenialReasonTypeId { get; set; }
		public int? DuplicateVendorId { get; set; }
	}
}
