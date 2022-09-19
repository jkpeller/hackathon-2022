using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace ProductCatalogAdminUiIntegrationTest.Model.Shared
{
	/// <summary>
	/// Contains base methods, controls, and variables used by multiple pages.
	/// </summary>
	public class BasePage
	{
		#region Basic functions and variables
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		protected static string Url = "";
		private static readonly string PaginatorSelector = ControlUtility.GetElementSelector("mat-paginator");
		private static readonly string OktaUsername = Configuration.GetValue<string>("OktaUsername");
		private static readonly string OktaPassword = Configuration.GetValue<string>("OktaPassword");
		private static readonly string SiteLevelSearchSelector = ControlUtility.GetElementSelector("input-site-level-search");
		private static string _bearerToken;
		public string UserDataFirstName { get; private set; }
		public string UserDataLastName { get; private set; }

		/// <summary>
		/// Uses the opened instance of IWebDriver to go to the page's Url, maximize the window, click on the link to the correct page, and then click the
		/// on-screen page title to verify that the page has finished loading. Also gets the bearer token value from the UI to use in API interactions.
		/// </summary>
		public void OpenPage()
		{
		
			BrowserUtility.GoToUrl(Url);
			Login();
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			BrowserUtility.WebDriver.Manage().Window.Maximize();
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			var jsExecutor = (IJavaScriptExecutor)BrowserUtility.WebDriver;
			Thread.Sleep(1000);
			var userData = JObject.Parse(jsExecutor.ExecuteScript($"return window.sessionStorage.getItem('userData_{Configuration.GetValue<string>("OktaClientId")}');").ToString());
			UserDataFirstName = userData.Property("given_name", StringComparison.InvariantCultureIgnoreCase)?.Value.Value<string>();
			UserDataLastName = userData.Property("family_name", StringComparison.InvariantCultureIgnoreCase)?.Value.Value<string>();

			var authorizationResult = JObject.Parse(jsExecutor.ExecuteScript($"return window.sessionStorage.getItem('authorizationResult_{Configuration.GetValue<string>("OktaClientId")}');").ToString());
			_bearerToken = authorizationResult.Property("access_token", StringComparison.InvariantCultureIgnoreCase)?.Value.Value<string>();
			BrandedResearchAdminApiService.InitializeHttpClient(_bearerToken);
			CategoryAdminApiService.InitializeHttpClient(_bearerToken);
			ProductAdminApiService.InitializeHttpClient(_bearerToken);
			VendorAdminApiService.InitializeHttpClient(_bearerToken);
		}

		private static void Login()
		{
			BrowserUtility.WaitForPageToLoad();
			Thread.Sleep(10000);
			LoginInputUsername.SendKeys(OktaUsername);
			LoginButtonNext.ClickAndWaitForPageToLoad();
			LoginInputPassword.SendKeys(OktaPassword);
			LoginButtonVerify.ClickAndWaitForPageToLoad();
		}

		#endregion

		#region Login info

		private static readonly Control LoginInputUsername = new Control("[id='okta-signin-username']");
		private static readonly Control LoginButtonNext = new Control("[id='okta-signin-submit']");
		private static readonly Control LoginInputPassword = new Control("[id='input59']");
		private static readonly Control LoginButtonVerify = new Control("[type='submit']");

		#endregion

		public readonly Control PageTitle = new Control(ControlUtility.GetElementSelector("page-title"));
		public readonly Control LinkSideNavCategoryMapping = new Control("[href='/site-categories/mapping']");
		public readonly Control LinkSideNavProductMapping = new Control("[href='/site-products/mapping']");
		public readonly Control LinkSideNavProducts = new Control("[href='/products']");
        public readonly Control LinkSideNavCategories = new Control("[href='/categories']");
		public readonly Control UnsavedChangesDialogSupportingText = new Control(ControlUtility.GetElementSelector("unsaved-changes-warning-dialog-supporting-text"));

		#region Common Filters

		public readonly Control InputFilterCategoryName = new Control(ControlUtility.GetElementSelector("input-category-name"));
		public readonly Control InputFilterProductName = new Control(ControlUtility.GetElementSelector("input-product-name"));
		
		public readonly Control SelectSite = new Control(ControlUtility.GetElementSelector("mat-select-site-name"));
		public readonly Control SelectOptionSiteCapterra = new Control(ControlUtility.GetElementSelector("mat-option-site-name-capterra"));
		public readonly Control SelectOptionSiteSoftwareAdvice = new Control(ControlUtility.GetElementSelector("mat-option-site-name-software advice"));
		public readonly Control SelectOptionSiteGetapp = new Control(ControlUtility.GetElementSelector("mat-option-site-name-getapp"));

		public readonly Control SelectMappingStatus = new Control(ControlUtility.GetElementSelector("mat-select-mapping-status"));
		public readonly Control SelectOptionMappingStatusMapped = new Control(ControlUtility.GetElementSelector("mat-option-mapping-status-mapped"));
		public readonly Control SelectOptionMappingStatusUnmapped = new Control(ControlUtility.GetElementSelector("mat-option-mapping-status-unmapped"));

		public readonly Control SelectPublishStatus = new Control(ControlUtility.GetElementSelector("mat-select-publish-status"));
		public readonly Control SelectOptionPublishStatusPublished = new Control(ControlUtility.GetElementSelector("mat-option-publish-status-published"));
		public readonly Control SelectOptionPublishStatusUnpublished = new Control(ControlUtility.GetElementSelector("mat-option-publish-status-unpublished"));

		public readonly Control SelectStatusActive = new Control(ControlUtility.GetElementSelector("mat-option-status-active"));
		public readonly Control SelectStatusArchived = new Control(ControlUtility.GetElementSelector("mat-option-status-archived"));

		public readonly Control SelectStatusDenied = new Control(ControlUtility.GetElementSelector("mat-option-status-denied"));
		public readonly Control SelectStatusNew = new Control(ControlUtility.GetElementSelector("mat-option-status-new"));
		public readonly Control SelectStatusUnderReview = new Control(ControlUtility.GetElementSelector("mat-option-status-under review"));
		public readonly Control SelectStatusApproved = new Control(ControlUtility.GetElementSelector("mat-option-status-approved"));

		public readonly Control ErrorMessageInputProductName = new Control(ControlUtility.GetElementSelector("mat-error-input-product-name"));
		public readonly Control ErrorMessageSelectSite = new Control(ControlUtility.GetElementSelector("mat-error-select-site"));
		public readonly Control ErrorMessageSelectMappingStatus = new Control(ControlUtility.GetElementSelector("mat-error-select-mapping-status"));
		public readonly Control ErrorMessageSelectPublishStatus = new Control(ControlUtility.GetElementSelector("mat-error-select-publish-status"));
		public readonly Control ErrorMessageSelectProductStatus = new Control(ControlUtility.GetElementSelector("mat-error-product-status"));

		public readonly Control ButtonApplyFilters = new Control(ControlUtility.GetElementSelector("button-apply-filters"));

		public readonly Control ButtonMappingModalCancel = new Control(ControlUtility.GetElementSelector("button-mapping-dialog-cancel"));
		public readonly Control ButtonMappingModalMap = new Control(ControlUtility.GetElementSelector("button-mapping-dialog-map"));

		public readonly Control SnackBarContainer = new Control("snack-bar-container");

		public readonly Control ArchiveMenuOption = new Control(ControlUtility.GetElementSelector("button-category-archiving"));
		public readonly Control UnarchiveMenuOption = new Control(ControlUtility.GetElementSelector("button-category-unarchiving"));
		public readonly Control AddNewTagMenuOption = new Control(ControlUtility.GetElementSelector("button-add-new-tag"));
		public readonly Control ConfirmationDialogTitle = new Control(ControlUtility.GetElementSelector("confirmation-dialog-title"));
		public readonly Control ConfirmationDialogText = new Control(ControlUtility.GetElementSelector("confirmation-dialog-supporting-text"));
		public readonly Control ButtonConfirmationDialogCancel = new Control(ControlUtility.GetElementSelector("button-confirmation-dialog-cancel"));
		public readonly Control ButtonConfirmationDialogAction = new Control(ControlUtility.GetElementSelector("button-confirmation-dialog-action"));

		public readonly Control MessageTableNoResults = new Control(ControlUtility.GetElementSelector("no-results"));

		public readonly Control TableHeaderStatus = new Control(ControlUtility.GetElementSelector("th-status"));
		public readonly Control TableHeaderModifiedOnUtc = new Control(ControlUtility.GetElementSelector("th-modified-on-utc"));

		public readonly Control ButtonGoBack = new Control(ControlUtility.GetElementSelector("button-go-back"));

		#endregion

		#region Common buttons

		public readonly Control ButtonCancel = new Control(ControlUtility.GetElementSelector("button-cancel"));
		public readonly Control ButtonSave = new Control(ControlUtility.GetElementSelector("button-save"));
        public readonly Control ButtonAddCategory = new Control(ControlUtility.GetElementSelector("button-add-category"));
        public readonly Control ButtonSaveChanges = new Control(ControlUtility.GetElementSelector("button-save-changes"));

		#endregion

		#region Table paginator

		public readonly Control PaginatorDropDownItemsPerPage = new Control($"{PaginatorSelector} .mat-select");
		public readonly Control PaginatorDisplayRange = new Control($"{PaginatorSelector} .mat-paginator-range-label");
		public readonly Control PaginatorButtonFirstPage = new Control($"{PaginatorSelector} [aria-label='First page']");
		public readonly Control PaginatorButtonNextPage = new Control($"{PaginatorSelector} [aria-label='Next page']");

		public static int GetResultsOnPage()
		{
			return int.Parse(Regex.Split(new Control($"{ControlUtility.GetElementSelector("mat-paginator")} .mat-paginator-range-label").GetText(), @"\D+")[1]);
		}

		/// <summary>
		/// Gets the Control for the items per page option that corresponds to the passed-in value in the drop down
		/// </summary>
		/// <param name="value"></param>
		public static Control GetTablePaginatorItemsPerPageOptionByValue(int value)
		{
			return new Control($"mat-option:nth-of-type({value})");
		}

		/// <summary>
		/// Gets the Control for the entire cell that corresponds to the passed-in row number and column name values
		/// </summary>
		/// <param name="rowNumber"></param>
		/// <param name="columnName"></param>
		public static Control GetTableCellByRowNumberAndColumnName(int rowNumber, string columnName)
		{
			return new Control(ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, columnName));
		}

		/// <summary>
		/// Returns a control for the autocomplete suggestion inside an autocomplete suggestion results box.
		/// Note: the add it as a new item counts as position 1, so matching suggestions start at position = 2
		/// </summary>
		/// <param name="position"></param>
		/// <returns />
		public static Control GetAutocompleteSuggestionByPosition(int position)
		{
			return new Control(ControlUtility.GetAutocompleteSuggestionSelector(position));
		}

		public int GetItemsPerPage()
		{
			//all displayed records on the first page have the publishStatusValue value in the published column
			var itemsPerPageIntegerValues = Regex.Split(PaginatorDisplayRange.GetText(), @"\D+");
			return int.Parse(itemsPerPageIntegerValues[1]);
		}

		/// <summary>
		/// Gets a control for the Capterra chip item containing the given name
		/// </summary>
		/// <param name="name"></param>
		public static Control GetCapterraChipByName(string name)
		{
			return new Control($"{ControlUtility.GetElementSelector("capterra-site-container")} {ControlUtility.GetChipListItemByDisplayName(name)}");
		}

		/// <summary>
		/// Gets a control for the Capterra chip item by the position in the list. Primarily for validating alphabetical order.
		/// </summary>
		/// <param name="position"></param>
		public static Control GetCapterraChipByPosition(int position)
		{
			return new Control($"[role='option']:nth-of-type({position})");
		}

		#endregion

		#region Global Summary

		public readonly Control InputSiteLevelSearchCapterra = new Control($"{ControlUtility.GetElementSelector("capterra-site-container")} {SiteLevelSearchSelector}");
		public readonly Control InputSiteLevelSearchSoftwareAdvice = new Control($"{ControlUtility.GetElementSelector("software-advice-site-container")} {SiteLevelSearchSelector}");
		public readonly Control InputSiteLevelSearchGetapp = new Control($"{ControlUtility.GetElementSelector("getapp-site-container")} {SiteLevelSearchSelector}");

		#endregion

		#region User Navigation

		public readonly Control ButtonUserSection = new Control(ControlUtility.GetElementSelector("button-user-menu"));
		public readonly Control DivCurrentUserEmail = new Control(ControlUtility.GetElementSelector("user-menu-item-email"));
		public readonly Control ButtonLogout = new Control(ControlUtility.GetElementSelector("button-user-menu-item-logout"));

		public readonly Control H3EnvironmentNameDisplay = new Control(ControlUtility.GetElementSelector("environment-name"));

		#endregion

		#region Autocomplete

		public readonly Control OptionFirstAutocompleteSuggestion = new Control(ControlUtility.GetAutocompleteSuggestionSelector(1));

		public IEnumerable<string> AutocompleteSuggestionNames
		{
			get
			{
				return BrowserUtility.WebDriver
					.FindElements(By.CssSelector(ControlUtility.GetAutocompleteSuggestionSelector()))
					.Select(c=>c.Text);
			}
		}

		#endregion

		#region Table Columns

		public static Control GetTableColumnByColumnNumber(int columnNumber)
		{
			return new Control($"[role='columnheader']:nth-of-type({columnNumber})");
		}

		#endregion

		#region Icons and Images

		public const string GrayIcon = "icon-gray.png";
		public const string ColorIcon = "icon-color.png";

		#endregion

        #region Common Input Fields

        public readonly Control InputCategoryName = new Control(ControlUtility.GetElementSelector("input-category-name"));

        #endregion

		/// <summary>
		/// Returns a control for the country option in the vendor country drop down in the HQ address section.
		/// </summary>
		/// <param name="countryName"></param>
		/// <returns />
		public static Control GetCountryOptionByName(string countryName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-country-{countryName.ToLower()}"));
		}

		public static Control GetStateProvinceByName(string stateProvinceRegion)
		{			
			return new Control(ControlUtility.GetElementSelector($"mat-option-state-province-{stateProvinceRegion.ToLower()}"));
		}

		public static Control GetCategoryOptionByName(string categoryName)
		{			
			return new Control(ControlUtility.GetElementSelector($"mat-option-category-name-{categoryName.ToLower()}"));
		}


		public static Control GetProductLinkByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("link-product-name")}");
		}

		public string GetCurrentUrl()
        {
			return BrowserUtility.WebDriver.Url;
        }

		public void ClickBrowserBackButton()
        {
			BrowserUtility.WebDriver.Navigate().Back();
		}

		#region Site publish status

		public static Control GetSitePublishStatusBySite(string siteName)
		{
			return new Control(ControlUtility.GetElementSelector($"publish-status-{siteName}"));
		}

		public static Control GetSitePublishButtonBySite(string siteName)
		{
			return new Control(ControlUtility.GetElementSelector($"button-publish-status-{siteName}"));
		}

		#endregion
	}
}