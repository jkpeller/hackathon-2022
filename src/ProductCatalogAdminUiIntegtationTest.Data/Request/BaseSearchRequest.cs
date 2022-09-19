using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class BaseSearchRequest
	{
		public string SortColumn { get; set; }

		public SortDirectionType? SortDirection { get; set; }

		public int? PageNumber { get; set; }

		public int? PageSize { get; set; }
	}
}
