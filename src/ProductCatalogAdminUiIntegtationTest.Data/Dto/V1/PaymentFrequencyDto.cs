using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class PaymentFrequencyDto : BaseSelectableDto
	{
		public PaymentFrequencyType PaymentFrequencyTypeId { get; set; }
		public string Name { get; set; }
	}
}
