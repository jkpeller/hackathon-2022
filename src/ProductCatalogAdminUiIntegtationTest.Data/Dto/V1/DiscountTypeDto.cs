using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
    public class DiscountTypeDto : BaseSelectableDto
    {
        public DiscountType DiscountTypeId { get; set; }
        public string Name { get; set; }
    }
}