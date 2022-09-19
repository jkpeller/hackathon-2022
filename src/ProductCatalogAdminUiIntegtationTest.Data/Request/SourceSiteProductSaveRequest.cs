using System.Collections.Generic;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class SourceSiteProductSaveRequest
	{
		public string SiteCode { get; set; }

		public string ProductId { get; set; }

		public string VendorId { get; set; }

		public string VendorName { get; set; }

		public string Name { get; set; }

		public string LongDescription { get; set; }

		public string LogoUrl { get; set; }

		public string ProductWebsiteUrl { get; set;}

		public bool IsPublished { get; set; }

		public SiteProductListingType? ListingType { get; set; }
		public SiteProductListingStatusType? PpcStatus { get; set; }
		public SiteProductListingStatusType? PplStatus { get; set; }

		public List<SiteProductDeploymentOptionType> DeploymentOptions { get; set; }

		public List<SiteProductPaymentFrequencyType> PaymentFrequencies { get; set; }

		public List<SiteProductPriceModelType> PriceModels { get; set; }
	}
}
