using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi
{
	public class ProductDestinationUrlUpdateRequest : JObject
	{
		public int Id
		{
			get => (int)this[UrlDynamicUtility.IdPropertyName];
			set => this[UrlDynamicUtility.IdPropertyName] = value;
		}

		public string Url
		{
			get => (string)this[UrlDynamicUtility.UrlPropertyName];
			set => this[UrlDynamicUtility.UrlPropertyName] = value;
		}

		public string Name
		{
			get => (string)this[UrlDynamicUtility.NamePropertyName];
			set => this[UrlDynamicUtility.NamePropertyName] = value;
		}
	}
}
