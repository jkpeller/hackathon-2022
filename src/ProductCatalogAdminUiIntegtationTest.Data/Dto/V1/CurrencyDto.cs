using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CurrencyDto : BaseSelectableDto
	{
		public int CurrencyId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Symbol { get; set; }
	}
}
