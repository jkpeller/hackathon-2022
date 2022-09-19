using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.MessageApi
{
	public class ListingRequestDto
	{
		public int ListingRequestId { get; set; }
		public Guid? SugarLeadId { get; set; }
		public Guid? SugarUserId { get; set; }
		public Guid? VendorId { get; set; }
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
		public string ProductWebsiteUrl { get; set; }
		public int? ProductProposedCategoryId { get; set; }
		public string ProductProposedCategoryName { get; set; }
		public List<BaseSocialMediaDto> SocialMediaUrls { get; set; }
		public List<ListingRequestDuplicateDto> DuplicateTypes { get; set; }
		public int? DenialTypeId { get; set; }
		public string DenialDescription { get; set; }
		public int? DuplicateVendorId { get; set; }
		public string DuplicateVendorName { get; set; }
		public int? ApprovalDenialUserId { get; set; }
		public string ApprovalDenialUserDisplayName { get; set; }
		public DateTimeOffset? ApprovalDenialModifiedOn { get; set; }
		public int? AssignedToUserId { get; set; }
		public string AssignedToUserDisplayName { get; set; }
		public int StatusTypeId { get; set; }
		public string StatusTypeDescription { get; set; }
	}
}