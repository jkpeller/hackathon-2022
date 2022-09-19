using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class ProductUpdateRequest : JObject
	{

		public string Name
		{
			get => (string)this[ProductDynamicUtility.NamePropertyName];
			set => this[ProductDynamicUtility.NamePropertyName] = value;
		}

		public BaseDynamicDto Status
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.StatusPropertyName];
			set => this[ProductDynamicUtility.StatusPropertyName] = value;
		}

		public string WebsiteUrl
		{
			get => (string)this[ProductDynamicUtility.WebsiteUrlPropertyName];
			set => this[ProductDynamicUtility.WebsiteUrlPropertyName] = value;
		}

		public bool? HasFreeVersion
		{
			get => (bool?)this[ProductDynamicUtility.HasFreeVersionPropertyName];
			set => this[ProductDynamicUtility.HasFreeVersionPropertyName] = value;
		}

		public bool? HasFreeTrial
		{
			get => (bool?)this[ProductDynamicUtility.HasFreeTrialPropertyName];
			set => this[ProductDynamicUtility.HasFreeTrialPropertyName] = value;
		}

		public decimal? StartingPrice
		{
			get => (decimal)this[ProductDynamicUtility.StartingPricePropertyName];
			set => this[ProductDynamicUtility.StartingPricePropertyName] = value;
		}

		public BaseDynamicDto CreditCardRequired
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.CreditCardRequiredPropertyName];
			set => this[ProductDynamicUtility.CreditCardRequiredPropertyName] = value;
		}

		public int? PriceRange
		{
			get => (int?)this[ProductDynamicUtility.PriceRangeIdPropertyName];
			set => this[ProductDynamicUtility.PriceRangeIdPropertyName] = value;
		}

		public BaseDynamicDto PricingModel
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.PricingModelPropertyName];
			set => this[ProductDynamicUtility.PricingModelPropertyName] = value;
		}

		public BaseDynamicDto OpenApiOffered
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.OpenApiOfferedPropertyName];
			set => this[ProductDynamicUtility.OpenApiOfferedPropertyName] = value;
		}

		public CurrencyDynamicDto Currency
		{
			get => (CurrencyDynamicDto)this[ProductDynamicUtility.CurrencyPropertyName];
			set => this[ProductDynamicUtility.CurrencyPropertyName] = value;
		}

		public BaseDynamicDto LicensingModel
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.LicensingModelPropertyName];
			set => this[ProductDynamicUtility.LicensingModelPropertyName] = value;
		}

		public BaseDynamicDto PaymentFrequency
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.PaymentFrequencyPropertyName];
			set => this[ProductDynamicUtility.PaymentFrequencyPropertyName] = value;
		}

		public JArray SupportOptionIds
		{
			get => (JArray)this["supportOptionIds"];
			set => this["supportOptionIds"] = value;
		}

		public JArray TrainingOptionIds
		{
			get => (JArray)this["trainingOptionIds"];
			set => this["trainingOptionIds"] = value;
		}

		public JArray DeploymentOptionIds
		{
			get => (JArray)this["deploymentOptionIds"];
			set => this["deploymentOptionIds"] = value;
		}

		public JArray TargetCompanySizeIds
		{
			get => (JArray)this["targetCompanySizeIds"];
			set => this["targetCompanySizeIds"] = value;
		}

		public JArray TargetNumberUserIds
		{
			get => (JArray)this["targetNumberUserIds"];
			set => this["targetNumberUserIds"] = value;
		}

		public JArray TargetIndustryCodes
		{
			get => (JArray)this["targetIndustryCodes"];
			set => this["targetIndustryCodes"] = value;
		}

		public bool? IsPricingVisible
		{
			get => (bool?)this[ProductDynamicUtility.IsPricingVisiblePropertyName];
			set => this[ProductDynamicUtility.IsPricingVisiblePropertyName] = value;
		}

		public JArray Sites
		{
			get => (JArray)this[ProductDynamicUtility.SitesPropertyName];
			set => this[ProductDynamicUtility.SitesPropertyName] = value;
		}
	}
}
