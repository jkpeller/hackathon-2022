using System.Collections.Generic;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
    public class ProductFeatureDto
    {
        public int CategoryId { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategoryStatusType ProductCategoryStatusTypeId { get; set; }
        public string CategoryName { get; set; }
        public List<FeatureDto> Features { get; set; }
    }
}