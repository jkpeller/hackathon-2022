using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using System;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class CategoryAdminDto : BaseCategoryAdminDto
	{
		public List<SiteCategoryRelatedAdminDto> SiteCategories { get; set; }
		public Guid CategoryIntegrationId { get; set; }
	}
}