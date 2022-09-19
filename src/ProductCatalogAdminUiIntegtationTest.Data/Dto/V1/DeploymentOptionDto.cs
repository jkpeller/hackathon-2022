using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class DeploymentOptionDto : BaseSelectableOrderedDto
	{
		public DeploymentOptionType DeploymentOptionTypeId { get; set; }
		public string Name { get; set; }
		public DeploymentOptionGroupDto DeploymentOptionGroup { get; set; }
	}
}
