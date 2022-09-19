using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CategoryFeatureDto
	{
		public string Name { get; set; }
		public int FeatureId { get; set; }
		public int CategoryFeatureId { get; set; }
		public string Definition { get; set; }
		public FeatureType FeatureTypeId { get; set; }
		public string FeatureTypeName { get; set; }
		public DateTimeOffset DateAddedUtc { get; set; }
	}
}
