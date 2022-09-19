namespace ProductCatalogAdminUiIntegrationTest.Data.Utility
{
	public static class SocialMediaUtility
	{
		public static readonly string CompanyName = RequestUtility.GetRandomString(9);
		public static readonly string LinkedInUrl = $"https://linkedin.com/{CompanyName}";
		public static readonly string TwitterUrl = $"https://twitter.com/{CompanyName}";
		public static readonly string FacebookUrl = $"https://facebook.com/{CompanyName}";
		public static readonly string YoutubeUrl = $"https://youtube.com/{CompanyName}";
		public static readonly string InstagramUrl = $"https://instagram.com/{CompanyName}";
	}
}
