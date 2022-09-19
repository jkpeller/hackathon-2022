namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class SourceSiteCategorySingleSaveRequest
	{
		public string SiteCode { get; set; }
		public string Name { get; set; }
		public string CategoryId { get; set; }
		public bool IsPublished { get; set; }
	}
}