using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class LicensingModelDto : BaseSelectableDto
	{
		public LicensingModelType LicensingModelTypeId { get; set; }
		public string Name { get; set; }
	}
}
