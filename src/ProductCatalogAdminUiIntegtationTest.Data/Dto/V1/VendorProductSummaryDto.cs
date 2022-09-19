using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class VendorProductSummaryDto
	{
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Status { get; set; }
		public int CategoryCount { get; set; }
		public DateTimeOffset CreatedOnUtc { get; set; }
		public DateTimeOffset ModifiedOnUtc { get; set; }
	}
}
