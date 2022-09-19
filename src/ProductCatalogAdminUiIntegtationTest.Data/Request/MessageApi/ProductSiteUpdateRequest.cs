using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class ProductSiteUpdateRequest : JObject
	{
		public string SiteCode
		{
			get => (string)this[ProductSiteDynamicUtility.SiteCodePropertyName];
			set => this[ProductSiteDynamicUtility.SiteCodePropertyName] = value;
		}

		public JArray Overrides
		{
			get => (JArray)this[ProductSiteDynamicUtility.OverridesPropertyName];
			set => this[ProductSiteDynamicUtility.OverridesPropertyName] = value;
		}
	}
}
