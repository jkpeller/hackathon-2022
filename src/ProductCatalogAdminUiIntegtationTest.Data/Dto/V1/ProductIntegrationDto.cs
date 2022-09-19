using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductIntegrationDto
	{
		public List<VerifiedDto> OpenApiOfferedVerifieds { get; set; }
		public List<IntegrationProductDto> Integrations { get; set; }
	}
}