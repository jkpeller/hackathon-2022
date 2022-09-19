using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class SupportOptionDto : BaseSelectableOrderedDto
	{
		public SupportOptionType SupportOptionTypeId { get; set; }
		public string Name { get; set; }
	}
}
