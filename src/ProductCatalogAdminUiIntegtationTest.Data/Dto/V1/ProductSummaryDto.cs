using System;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductSummaryDto : ProductAutocompleteAdminDto
	{
		public int? ProductStatusId { get; set; }
		public DateTimeOffset ModifiedOnUtc { get; set; }
		public int CapterraProductCount { get; set; }
		public int SoftwareAdviceProductCount { get; set; }
		public int GetAppProductCount { get; set; }
		public int TotalProductCount { get; set; }
		public string VendorName { get; set; }
		public int? VendorId { get; set; }
	}
}