using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CompanySizeDto : BaseSelectableOrderedDto
	{
		public CompanySizeType CompanySizeTypeId { get; set; }
		public string Name { get; set; }
	}
}
