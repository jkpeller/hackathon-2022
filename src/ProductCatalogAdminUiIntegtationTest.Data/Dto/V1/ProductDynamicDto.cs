using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductDynamicDto : JObject
	{
		public ProductDynamicDto(JObject jObject)
			: base(jObject)
		{
		}

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

		public int? PriceRangeId
		{
			get => (int?)this[ProductDynamicUtility.PriceRangeIdPropertyName];
			set => this[ProductDynamicUtility.PriceRangeIdPropertyName] = value;
		}

		public BaseDynamicDto PricingModel
		{
			get => (BaseDynamicDto)this[ProductDynamicUtility.PricingModelPropertyName];
			set => this[ProductDynamicUtility.PricingModelPropertyName] = value;
		}

		public bool? IsPricingVisible
		{
			get => (bool?)this[ProductDynamicUtility.IsPricingVisiblePropertyName];
			set => this[ProductDynamicUtility.IsPricingVisiblePropertyName] = value;
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

		public JArray SupportOptions
		{
			get => (JArray)this[ProductDynamicUtility.SupportOptionsPropertyName];
			set => this[ProductDynamicUtility.SupportOptionsPropertyName] = JArray.FromObject(value);
		}

		public JArray TrainingOptions
		{
			get => (JArray)this[ProductDynamicUtility.TrainingOptionsPropertyName];
			set => this[ProductDynamicUtility.TrainingOptionsPropertyName] = JArray.FromObject(value);
		}

		public JArray DeploymentOptions
		{
			get => (JArray)this[ProductDynamicUtility.DeploymentOptionsPropertyName];
			set => this[ProductDynamicUtility.DeploymentOptionsPropertyName] = JArray.FromObject(value);
		}

		public JArray TargetCompanySizes
		{
			get => (JArray)this[ProductDynamicUtility.TargetCompanySizesPropertyName];
			set => this[ProductDynamicUtility.TargetCompanySizesPropertyName] = JArray.FromObject(value);
		}

		public JArray TargetNumberUsers
		{
			get => (JArray)this[ProductDynamicUtility.TargetNumberUsersPropertyName];
			set => this[ProductDynamicUtility.TargetNumberUsersPropertyName] = JArray.FromObject(value);
		}

		public JArray TargetIndustries
		{
			get => (JArray)this[ProductDynamicUtility.TargetIndustriesPropertyName];
			set => this[ProductDynamicUtility.TargetIndustriesPropertyName] = JArray.FromObject(value);
		}

		public JArray Sites
		{
			get => (JArray)this[ProductDynamicUtility.SitesPropertyName];
			set => this[ProductDynamicUtility.SitesPropertyName] = JArray.FromObject(value);
		}
	}
}
