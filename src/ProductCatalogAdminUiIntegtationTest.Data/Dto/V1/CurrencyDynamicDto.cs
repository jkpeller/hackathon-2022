using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CurrencyDynamicDto : BaseCodeDynamicDto
	{
		public string Symbol
		{
			get => (string)this[CurrencyDynamicUtility.SymbolPropertyName];
			set => this[CurrencyDynamicUtility.SymbolPropertyName] = value;
		}
	}
}
