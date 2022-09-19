using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class TrainingOptionDto : BaseSelectableOrderedDto
	{
		public TrainingOptionType TrainingOptionTypeId { get; set; }
		public string Name { get; set; }
	}
}
