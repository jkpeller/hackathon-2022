using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class IndustryDto : BaseSelectableDto
	{
		public int IndustryId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
	}
}
