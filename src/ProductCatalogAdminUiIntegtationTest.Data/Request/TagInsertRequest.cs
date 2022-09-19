using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
    public class TagInsertRequest
    {
        public TagType TagTypeId { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
    }
}