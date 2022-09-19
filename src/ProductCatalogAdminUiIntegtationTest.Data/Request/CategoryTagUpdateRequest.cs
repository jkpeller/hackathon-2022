using System.Collections.Generic;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
    public class CategoryTagUpdateRequest
    {
        public List<int> TagIds { get; set; }
    }
}