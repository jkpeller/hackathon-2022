using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductVideoAttributesDto : BaseMediaDto
	{
		public string ThumbnailUrl { get; set; }
		public int VideoTypeId { get; set; }
		public string VideoId { get; set; }
	}
}
