using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductAdminDto : ProductAutocompleteAdminDto
	{
		public ProductStatusType StatusTypeId { get; set; }
		public string Status { get; set; }
		public List<SiteProductRelatedAdminDto> SiteProducts { get; set; }
		public Guid ProductIntegrationId { get; set; }
	}
}