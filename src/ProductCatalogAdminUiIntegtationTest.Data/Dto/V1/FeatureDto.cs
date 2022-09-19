using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class FeatureDto : FeatureAutocompleteDto
	{
		public bool IsArchived { get; set; }
		public Guid FeatureIntegrationId { get; set; }
	}
}
