using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CountryDto : BaseSelectableDto
	{
		public int CountryId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
	}
}
