using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
    public class TagDto
    {
        public int TagId { get; set; }
        public TagType TagTypeId { get; set; }
        public string TagTypeName { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
    }
}
