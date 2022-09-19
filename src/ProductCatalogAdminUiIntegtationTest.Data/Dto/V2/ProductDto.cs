using ProductCatalogAdminUiIntegrationTest.Data.Dto.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V2
{
	public class ProductDto : ProductAutocompleteDto
	{
		public string LogoUrl { get; set; }
		public VendorDto Vendor { get; set; }
		public List<ProductStatusSelectableDto> ProductStatuses { get; set; }
		public List<ProductPublishStatusDto> PublishStatuses { get; set; }
		public List<CompanySizeDto> TargetCompanySizes { get; set; }
		public List<NumberUserDto> TargetNumberUsers { get; set; }
		public List<IndustryDto> TargetIndustries { get; set; }
		public List<SupportOptionDto> SupportOptions { get; set; }
		public List<TrainingOptionDto> TrainingOptions { get; set; }
		public List<DeploymentOptionDto> DeploymentOptions { get; set; }
		public List<LanguageDto> SupportedLanguages { get; set; }
		public List<CountryDto> SupportedCountries { get; set; }
		public List<LicensingModelDto> LicensingModels { get; set; }
		public List<ProductUrlDto> MobileUrls { get; set; }
		public List<ProductUrlDto> SocialMediaUrls { get; set; }
	}
}
