using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class CategoryOverrideUpdateRequest : JObject
	{
		public string Id
		{
			get => (string)this[ProductCategoryDynamicUtility.IdPropertyName];
			set => this[ProductCategoryDynamicUtility.IdPropertyName] = value;
		}

		public JArray DestinationUrls
		{
			get => (JArray)this[ProductCategoryDynamicUtility.DestinationUrlsPropertyName];
			set => this[ProductCategoryDynamicUtility.DestinationUrlsPropertyName] = JArray.FromObject(value);
		}

		public JArray Descriptions
		{
			get => (JArray)this[ProductCategoryDynamicUtility.DescriptionsPropertyName];
			set => this[ProductCategoryDynamicUtility.DescriptionsPropertyName] = JArray.FromObject(value);
		}
	}
}
