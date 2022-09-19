using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ListingRequestInsertRequest
	{
		public Guid? SugarLeadId { get; set; }
		public Guid? SugarUserId { get; set; }
		public Guid? VendorId { get; set; }
		public string TargetSourceSiteVendorId { get; set; }
		public string TargetSourceSiteVendorSiteCode { get; set; }
		public int SourceTypeId { get; set; }
		public string CompanyName { get; set; }
		public string CompanyPhoneNumber { get; set; }
		public string CompanyContactEmail { get; set; }
		public string CompanyContactFirstName { get; set; }
		public string CompanyContactLastName { get; set; }
		public string CompanyStreetAddress1 { get; set; }
		public string CompanyStreetAddress2 { get; set; }
		public string CompanyCity { get; set; }
		public string CompanyStateProvinceRegionName { get; set; }
		public string CompanyZipPostalCode { get; set; }
		public string CompanyCountryCode { get; set; }
		public string CompanyWebsiteUrl { get; set; }
		public string ProductName { get; set; }
		public string ProductShortDescription { get; set; }
		public Guid? ProductProposedCategoryId { get; set; }
		public string ProductProposedCategoryName { get; set; }
		public Guid CreatedByEventId { get; set; }
		public List<ListingRequestSocialMediaRequest> SocialMedialUrls { get; set; }
		public string SugarUserFirstName { get; set; }
		public string SugarUserLastName { get; set; }
		public string SugarUserEmail { get; set; }
	}
}
