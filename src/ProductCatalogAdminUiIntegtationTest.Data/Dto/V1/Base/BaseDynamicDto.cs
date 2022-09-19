using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base
{
	public class BaseDynamicDto : JObject
	{
		public int Id
		{
			get => (int)this[BaseDynamicUtility.IdPropertyName];
			set => this[BaseDynamicUtility.IdPropertyName] = value;
		}

		public string Name
		{
			get => (string)this[BaseDynamicUtility.NamePropertyName];
			set => this[BaseDynamicUtility.NamePropertyName] = value;
		}
	}
}
