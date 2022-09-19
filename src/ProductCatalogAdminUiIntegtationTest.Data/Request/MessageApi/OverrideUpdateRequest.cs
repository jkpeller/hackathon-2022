using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class OverrideUpdateRequest : JObject
	{
		public JArray Categories
		{
			get => (JArray)this[OverrideDynamicUtility.CategoriesPropertyName];
			set => this[OverrideDynamicUtility.CategoriesPropertyName] = value;
		}
	}
}
