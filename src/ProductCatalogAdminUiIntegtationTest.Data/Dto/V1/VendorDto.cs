using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class VendorDto : VendorAutocompleteDto
	{
		public string YearFounded { get; set; }
		public string PhoneNumber { get; set; }
		public string About { get; set; }
		public VendorAddressDto Address { get; set; }
		public List<VendorSocialMediaDto> SocialMediaUrls { get; set; }
		public string LogoUrl { get; set; }
		public Guid VendorIntegrationId { get; set; }
		public VendorStatusType VendorStatusTypeId { get; set; }
		public string Notes { get; set; }
		public string VendorStatusTypeName { get; set; }
	}
}