using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ReportDto
	{
		public int ReportId { get; set; }
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public DateTimeOffset PublishOnUtc { get; set; }
		public ReportType ReportTypeId { get; set; }
		public string ReportTypeName { get; set; }
		public ReportStatusType ReportStatusTypeId { get; set; }
		public string ReportStatusTypeName { get; set; }
		public List<ReportProductDto> Products { get; set; }
	}
}
