using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Core.Utility
{
	/// <summary>
	/// This class is used to interact with the browser itself.
	/// Its primary functions are to navigate to Urls passed in by the user and to refresh the browser when necessary.
	/// </summary>
	public static class BrowserUtility
	{
        private static readonly IConfiguration Configuration = InitConfiguration();
		//Timeout used for UI loading actions
		private static readonly TimeSpan LoadingTimeout = TimeSpan.FromSeconds(30);
		//Timeout used for actions on elements
		public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
		public static ChromeDriver WebDriver;
        public static readonly Uri BaseUri = new(Configuration.GetValue<string>("BaseUri"));
		private static readonly bool IsHeadlessMode = Convert.ToBoolean(Configuration.GetValue<string>("IsHeadlessMode"));

		public const string CategoryPageName = "categories";
		public const string ListingRequestPageName = "listing-requests";
		public const string ProductPageName = "products";
		public const string SiteCategoryMappingPageName = "site-categories/mapping";
		public const string SiteProductMappingPageName = "site-products/mapping";
		public const string VendorPageName = "vendors";

		#region Browser Navigation
		public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("secrets.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }

		/// <summary>
		/// Determines whether or not to prefix the given url value with the baseUrl and then calls the method to go there.
		/// Can be used for external sites, such as Google, by passing in isInternal = false
		/// </summary>
		/// <param name="url" />
		/// <param name="isInternal" />
		/// <returns>The URL returned by the WebDriver after the navigation operation is complete to be used to verify the
		/// test made it to the correct page.</returns>
		public static void GoToUrl(string url, bool isInternal = true)
		{
			if (IsHeadlessMode)
			{
				var option = new ChromeOptions();
				option.AddArgument("--headless");
				option.AddArgument("--window-size=1920,1080");
				WebDriver = new ChromeDriver(option);
				
			}
			else
			{
				WebDriver = new ChromeDriver();
			}

			var uri = isInternal ? new Uri(BaseUri, url) : new Uri(url);
			WebDriver.Navigate().GoToUrl(uri);
		}

		/// <summary>
		/// Waits for the loading overlay shade to disappear from the screen, indicating that the page has finished loading.
		/// </summary>
		public static void WaitForPageToLoad()
		{
			var wait = new WebDriverWait(WebDriver, LoadingTimeout);
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".loading-shade")));
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".loading-shade")));
		}

		/// <summary>
		/// Waits for the loading overlay shade to disappear from the screen, indicating that the page has finished loading.
		/// </summary>
		public static void WaitForOverlayToDisappear()
		{
			var wait = new WebDriverWait(WebDriver, LoadingTimeout);
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("[class=\'cdk-overlay-container\']")));
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("[class=\'cdk-overlay-container\']")));
		}

		/// <summary>
		/// Switches the browser focus to the most recently-opened tab
		/// </summary>
		public static void SwitchToNewTab()
		{
			WebDriver.SwitchTo().Window(WebDriver.WindowHandles.LastOrDefault());
		}

		/// <summary>
		/// Refreshes the page
		/// </summary>
		public static void Refresh()
		{
			WebDriver.Navigate().Refresh();
			WaitForPageToLoad();
			WaitForOverlayToDisappear();
		}

		/// <summary>
		/// Navigate to a specific page
		/// </summary>
		/// <param name="pageName"></param>
		/// <param name="id"></param>
		/// <param name="waitFor"></param>
		public static void NavigateToPage(string pageName, string id = null, int waitFor = 10000)
		{
			WebDriver.Navigate().GoToUrl(new Uri(BaseUri, $"{pageName}/{id}"));
			WaitForPageToLoad();
			WaitForOverlayToDisappear();
			Thread.Sleep(waitFor);
		}

		/// <summary>
        /// Navigate to a specific page
        /// </summary>
        /// <param name="waitFor"></param>
        public static void NavigateBack(int waitFor = 10000)
        {
            WebDriver.Navigate().Back();
            WaitForPageToLoad();
            WaitForOverlayToDisappear();
            Thread.Sleep(waitFor);
        }

		/// <summary>
		/// Waits of an element to disappear from the UI
		/// </summary>
		/// <param name="dataQaSelector">data-qa value</param>
		public static void WaitForElementToDisappear(string dataQaSelector)
		{
			var selector = ControlUtility.GetElementSelector(dataQaSelector);
			var wait = new WebDriverWait(WebDriver, LoadingTimeout);
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(selector)));
		}

		/// <summary>
		/// Waits for an element to appear in the UI
		/// </summary>
		/// <param name="dataQaSelector">data-qa value</param>
		public static void WaitForElementToAppear(string dataQaSelector)
		{
			var selector = ControlUtility.GetElementSelector(dataQaSelector);
			var wait = new WebDriverWait(WebDriver, LoadingTimeout);
			wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(selector)));
		}

		#endregion
	}
}