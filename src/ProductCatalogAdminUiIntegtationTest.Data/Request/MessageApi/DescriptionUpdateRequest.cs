using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class DescriptionUpdateRequest : JObject
	{
		public int Id
		{
			get => (int)this[DescriptionDynamicUtility.IdPropertyName];
			set => this[DescriptionDynamicUtility.IdPropertyName] = value;
		}

		public string Text
		{
			get => (string)this[DescriptionDynamicUtility.TextPropertyName];
			set => this[DescriptionDynamicUtility.TextPropertyName] = value;
		}
	}
}
