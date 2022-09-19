using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class LanguageDto : BaseSelectableDto
	{
		public int LanguageId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
	}
}
