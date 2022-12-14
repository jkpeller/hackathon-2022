using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class NumberUserDto : BaseSelectableOrderedDto
	{
		public NumberUserType NumberUserTypeId { get; set; }
		public string Name { get; set; }
	}
}
