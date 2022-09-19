using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ReportProductDto
	{
		public int ReportProductId { get; set; }
		public int Rank { get; set; }
		public int? VendorId { get; set; }
		public string VendorName { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductWebsiteUrl { get; set; }
		public QualificationStatusType? QualificationStatusTypeId { get; set; }
		public string QualificationStatusTypeName { get; set; }
		public PublishStatusType PublishStatusTypeId { get; set; }
		public string PublishStatusTypeName { get; set; }
		public decimal XCoordinate { get; set; }
		public decimal XOffset { get; set; }
		public decimal YCoordinate { get; set; }
		public decimal YOffset { get; set; }
		public decimal FinalScore { get; set; }

		public List<ReportSiteProductDto> SiteProducts { get; set; }
	}
}
