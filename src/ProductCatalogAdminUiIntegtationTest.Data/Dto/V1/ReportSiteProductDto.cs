using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ReportSiteProductDto : BaseSelectableDto
	{
		public int SiteProductId { get; set; }
		public string SourceSiteProductId { get; set; }
	}
}
