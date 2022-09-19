using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class VendorSummaryDto : VendorAutocompleteDto
	{

		public int ProductCount { get; set; }

		public int CategoryCount { get; set; }

		public string Status { get; set; }

		public DateTimeOffset CreatedOnUtc { get; set; }

		public DateTimeOffset ModifiedOnUtc { get; set; }
	}
}
