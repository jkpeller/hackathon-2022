using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class CategoryFeatureInsertRequest
	{
		public int FeatureId { get; set; }
		public FeatureType FeatureTypeId { get; set; }
	}
}