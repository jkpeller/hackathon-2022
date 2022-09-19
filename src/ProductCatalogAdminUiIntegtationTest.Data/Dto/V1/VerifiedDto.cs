using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class VerifiedDto : BaseSelectableDto
	{
		public VerifiedType VerifiedTypeId { get; set; }
		public string Name { get; set; }
	}
}
