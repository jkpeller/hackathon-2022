using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base
{
	public class BaseCodeDynamicDto : BaseDynamicDto
	{
		public string Code
		{
			get => (string)this[BaseDynamicUtility.CodePropertyName];
			set => this[BaseDynamicUtility.CodePropertyName] = value;
		}
	}
}
