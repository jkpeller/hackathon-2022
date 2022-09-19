namespace ProductCatalogAdminUiIntegrationTest.Core.Utility
{
	public static class ProductDetailsUtility
	{
		public static class TrainingOptions
		{
			public const string NotOffered = "not offered";
			public const string InPerson = "in person";
			public const string LiveOnline = "live online";
			public const string Webinars = "webinars";
			public const string Documentation = "documentation";
			public const string Videos = "videos";
		}

		public static class DeploymentOptions
		{
			public static string CloudSaasWebBased = "cloud, saas, web-based";
			public static string Mac = "mac";
			// note: these aren't working in the UI right now
			public static string DesktopWindows = "desktop windows";
			public static string DesktopLinux = "desktop linux";
			public static string OnPremiseWindows = "on premise windows";
			public static string OnPremiseLinux = "on premise linux";
		}
	}
}
