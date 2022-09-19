namespace ProductCatalogAdminUiIntegrationTest.Data.Shared
{
	public class ResponsePage
	{
		/// <summary>
		/// Page number for data being returned
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		/// Page size of data being returned
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Total record count for all records matching the request
		/// </summary>
		public int TotalRecordCount { get; set; }
	}
}
