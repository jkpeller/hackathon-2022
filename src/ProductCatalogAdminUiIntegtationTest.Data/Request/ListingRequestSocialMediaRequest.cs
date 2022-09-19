using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ListingRequestSocialMediaRequest
	{
		public SocialMediaType SocialMediaTypeId { get; set; }

		public string SocialMediaUrl { get; set; }
	}
}
