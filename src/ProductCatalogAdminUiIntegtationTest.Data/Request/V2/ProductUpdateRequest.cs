using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.V2
{
	public class ProductUpdateRequest : BaseProductRequest
	{
		public string ProductWebsiteUrl { get; set; }
		public List<CompanySizeType> TargetCompanySizeTypeIds { get; set; }
		public List<NumberUserType> TargetNumberUserTypeIds { get; set; }
		public List<int> TargetIndustryIds { get; set; }
		public List<SupportOptionType> SupportOptionTypeIds { get; set; }
		public List<TrainingOptionType> TrainingOptionTypeIds { get; set; }
		public List<DeploymentOptionType> DeploymentOptionTypeIds { get; set; }
		public List<int> SupportedLanguageIds { get; set; }
		public List<int> SupportedCountryIds { get; set; }
		public LicensingModelType? LicensingModelTypeId { get; set; }
		public List<MobileUrlRequest> MobileUrls { get; set; }
		public List<SocialMediaUrlRequest> SocialMediaUrls { get; set; }
	}
}