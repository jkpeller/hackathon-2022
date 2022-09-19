using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class NewListingRequestPage : BasePage
	{
		private const string PageUrl = "/listing-requests";

		public NewListingRequestPage()
		{
			Url = PageUrl;
		}

		#region Single Elements

		//table headers
		public readonly Control TableHeaderReceived = new Control(ControlUtility.GetElementSelector("th-received"));
		public readonly Control TableHeaderVendor = new Control(ControlUtility.GetElementSelector("th-company-name"));
		public readonly Control TableHeaderProduct = new Control(ControlUtility.GetElementSelector("th-product-name"));
		public readonly Control TableHeaderCategory = new Control(ControlUtility.GetElementSelector("th-category-name"));
		public readonly Control TableHeaderHqCountry = new Control(ControlUtility.GetElementSelector("th-country-code"));
		public readonly Control TableHeaderSource = new Control(ControlUtility.GetElementSelector("th-source"));
		public readonly Control TableHeaderUpdated = new Control(ControlUtility.GetElementSelector("th-updated"));

		public readonly Control TableAllClear = new Control(ControlUtility.GetElementSelector("no-results"));

		//display listing request details card
		public readonly Control ListingRequestCompanyName = new Control(ControlUtility.GetElementSelector("listing-request-company-name"));
		public readonly Control ListingRequestCompanyUrl = new Control(ControlUtility.GetElementSelector("listing-request-company-website-url"));
		public readonly Control ListingRequestContactFullName = new Control(ControlUtility.GetElementSelector("listing-request-company-contact-full-name"));
		public readonly Control ListingRequestContactEmail = new Control(ControlUtility.GetElementSelector("listing-request-company-contact-email"));
		public readonly Control ListingRequestAddress = new Control(ControlUtility.GetElementSelector("listing-request-address"));
		public readonly Control ListingRequestPhoneNumber = new Control(ControlUtility.GetElementSelector("listing-request-company-phone-number"));

		public readonly Control ListingRequestSocialMedia = new Control(ControlUtility.GetElementSelector("listing-request-social-media"));
		public readonly Control ListingRequestLinkedinUrl = new Control(ControlUtility.GetElementSelector("listing-request-linkedin-url"));
		public readonly Control ListingRequestTwitterUrl = new Control(ControlUtility.GetElementSelector("listing-request-twitter-url"));
		public readonly Control ListingRequestFacebookUrl = new Control(ControlUtility.GetElementSelector("listing-request-facebook-url"));
		public readonly Control ListingRequestYoutubeUrl = new Control(ControlUtility.GetElementSelector("listing-request-youtube-url"));
		public readonly Control ListingRequestInstagram = new Control(ControlUtility.GetElementSelector("listing-request-instagram-url"));

		public readonly Control ListingRequestProductName = new Control(ControlUtility.GetElementSelector("listing-request-product-name"));
		public readonly Control ListingRequestProductUrl = new Control(ControlUtility.GetElementSelector("listing-request-product-website-url"));
		public readonly Control ListingRequestProductDesc = new Control(ControlUtility.GetElementSelector("listing-request-product-short-description"));

		public readonly Control ListingRequestProposedCategory = new Control(ControlUtility.GetElementSelector("listing-request-product-proposed-category"));
		public readonly Control ListingRequestApprovedCategory = new Control(ControlUtility.GetElementSelector("listing-request-product-approved-category"));
		public readonly Control ListingRequestApprovedCategoryDefinition = new Control(ControlUtility.GetElementSelector("listing-request-product-approved-category-definition"));

		//duplicate vendor check
		public readonly Control DuplicateVendorsSearchInProgress = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-search-in-progress"));
		public readonly Control DuplicateVendorsDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-detected"));
		public readonly Control DuplicateVendorsNoneDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-none"));
		public readonly Control DuplicateVendorsBadUrlDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-bad-url"));

		//duplicate vendor product check
		public readonly Control DuplicateVendorProductSearchInProgress = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-search-in-progress"));
		public readonly Control DuplicateVendorProductDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-detected"));
		public readonly Control DuplicateVendorProductNoneDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-none"));

		public readonly Control SelectListingRequestStatus = new Control(ControlUtility.GetElementSelector("mat-select-listing-request-status"));
		public readonly Control ListingRequestFilterStatusApproved = new Control(ControlUtility.GetElementSelector("mat-option-status-approved"));
		public readonly Control ListingRequestFilterStatusDenied = new Control(ControlUtility.GetElementSelector("mat-option-status-denied"));
		public readonly Control ListingRequestFilterStatusNew = new Control(ControlUtility.GetElementSelector("mat-option-status-new"));
		public readonly Control ListingRequestFilterStatusUnderReview = new Control(ControlUtility.GetElementSelector("mat-option-status-under review"));
		public readonly Control ErrorListingRequestFilterStatus = new Control(ControlUtility.GetElementSelector("mat-error-listing-request-status"));
		public readonly Control InputCompanyProductNameFilter = new Control(ControlUtility.GetElementSelector("input-company-product-name"));
		public readonly Control ErrorCompanyProductNameFilter = new Control(ControlUtility.GetElementSelector("mat-error-company-product-name"));
		public readonly Control SelectVendorCountry = new Control(ControlUtility.GetElementSelector("mat-select-vendor-country"));

		// listing request status
		public readonly Control SelectNewStatus = new Control(ControlUtility.GetElementSelector("mat-option-listing-request-status-new"));
		public readonly Control SelectUnderReviewStatus = new Control(ControlUtility.GetElementSelector("mat-option-listing-request-status-under review"));
		public readonly Control ListingStatusDisplay = new Control(ControlUtility.GetElementSelector("status-name"));

		// listing request HQ Country filter
		public readonly Control SelectListingRequestHqCountry = new Control(ControlUtility.GetElementSelector("mat-select-vendor-country"));
		public readonly Control SelectListingRequestHqCountryInput = new Control(".mat-select-search-inner .mat-input-element");

		// edit listing request details card
		public readonly Control ButtonEditListingRequest = new Control(ControlUtility.GetElementSelector("button-edit-listing-request"));
		public readonly Control InputCompanyName = new Control(ControlUtility.GetElementSelector("input-company-name"));
		public readonly Control InputCompanyWebsiteUrl = new Control(ControlUtility.GetElementSelector("input-company-website-url"));
		public readonly Control InputCompanyPhoneNumber = new Control(ControlUtility.GetElementSelector("input-company-phone-number"));
		public readonly Control InputTwitter = new Control(ControlUtility.GetElementSelector("input-twitter"));
		public readonly Control InputFacebook = new Control(ControlUtility.GetElementSelector("input-facebook"));
		public readonly Control InputLinkedIn = new Control(ControlUtility.GetElementSelector("input-linkedin"));
		public readonly Control InputYouTube = new Control(ControlUtility.GetElementSelector("input-youtube"));
		public readonly Control InputInstagram = new Control(ControlUtility.GetElementSelector("input-instagram"));
		public readonly Control SelectCountry = new Control(ControlUtility.GetElementSelector("mat-select-country"));
		public readonly Control InputCountrySearch = new Control("[ng-reflect-klass='mat-select-search-inner mat-ty'] .cdk-text-field-autofill-monitored");
		public readonly Control InputAddress1 = new Control(ControlUtility.GetElementSelector("input-streetAddress1"));
		public readonly Control InputAddress2 = new Control(ControlUtility.GetElementSelector("input-streetAddress2"));
		public readonly Control SelectStateProvince = new Control(ControlUtility.GetElementSelector("mat-select-state-province"));
		public readonly Control InputStateProvince = new Control(ControlUtility.GetElementSelector("input-state-province-name"));
		public readonly Control OptionFirstStateProvince = new Control("[role='option']:nth-of-type(1)");
		public readonly Control InputCity = new Control(ControlUtility.GetElementSelector("input-city"));
		public readonly Control InputZipPostalCode = new Control(ControlUtility.GetElementSelector("input-zip-postal-code"));
		public readonly Control InputProductWebsiteUrl = new Control(ControlUtility.GetElementSelector("input-product-website-url"));
		public readonly Control InputProductShortDescription = new Control(ControlUtility.GetElementSelector("textarea-product-short-description"));
		public readonly Control SearchApprovedCategory = new Control(".mat-select-search-inner .mat-select-search-input");
		public readonly Control SelectApprovedCategory = new Control(ControlUtility.GetElementSelector("mat-select-approved-category"));
		public readonly Control CategoryPublishedOn = new Control(ControlUtility.GetElementSelector("approved-category-published-on"));
		public readonly Control CategoryCoreFeatureNoResult = new Control(ControlUtility.GetElementSelector("no-category-features-result"));
		public readonly Control CategoryNoSelectedToDisplayCoreFeature = new Control(ControlUtility.GetElementSelector("no-category-features-selected"));
		public readonly Control ApprovedCategoryDefinition = new Control(ControlUtility.GetElementSelector("approved-category-definition"));

		// error messages details card
		public readonly Control ErrorRequiredCompanyName = new Control(ControlUtility.GetElementSelector("mat-error-required-company-name"));
		public readonly Control ErrorRequiredCompanyWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-required-company-website-url"));
		public readonly Control ErrorRequiredPhoneNumber = new Control(ControlUtility.GetElementSelector("mat-error-required-phone-number"));
		public readonly Control ErrorRequiredProductName = new Control(ControlUtility.GetElementSelector("mat-error-required-product-name"));
		public readonly Control ErrorRequiredProductWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-required-product-website-url"));
		public readonly Control ErrorRequiredProductShortDescription = new Control(ControlUtility.GetElementSelector("mat-error-required-product-short-description"));
		public readonly Control ErrorLengthProductShortDescription = new Control(ControlUtility.GetElementSelector("mat-error-product-short-description"));

		// request actions details card
		public readonly Control ButtonApproveListing = new Control(ControlUtility.GetElementSelector("button-approve-listing"));
		public readonly Control ButtonDenyListing = new Control(ControlUtility.GetElementSelector("button-deny-listing"));

		// display listing request notes card
		public readonly Control ListingRequestNotes = new Control(ControlUtility.GetElementSelector("p-notes"));

		// edit listing request notes
		public readonly Control ButtonEditListingRequestNotes = new Control(ControlUtility.GetElementSelector("button-edit-listing-request-notes"));
		public readonly Control InputListingRequestNotes = new Control(ControlUtility.GetElementSelector("text-area-notes"));
		public readonly Control ButtonSaveListingRequestNotes = new Control(ControlUtility.GetElementSelector("button-save-notes"));
		public readonly Control ButtonCancelListingRequestNotes = new Control(ControlUtility.GetElementSelector("button-cancel-notes"));

		#endregion

		#region Element methods

		/// <summary>
		/// Returns a control for the Approved Category options in the select drop down
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns />
		public static Control GetApprovedCategoryOptionByName(string categoryName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-approved-category-{categoryName.ToLower()}"));
		}

		/// <summary>
		/// Returns a control for the Approved Category options in the select drop down
		/// </summary>
		/// <param name="stateProvinceName"></param>
		/// <returns />
		public static Control GetStateOptionByName(string stateProvinceName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-state-province-{stateProvinceName.ToLower()}"));
		}

		public static Control GetDuplicateVendorLinkByVendorName(string vendorName)
		{
			return new Control(ControlUtility.GetElementSelector($"duplicate-vendor-{vendorName}"));
		}

		public static Control GetDuplicateVendorProductLinkByProductName(string productName)
		{
			return new Control(ControlUtility.GetElementSelector($"duplicate-vendor-product-{productName}"));
		}

		public static int GetSelectedProposedCategoryCoreFeaturesRowCount()
		{
			var count = BrowserUtility.WebDriver.FindElements(By.CssSelector("[data-qa=\"td-feature-name\"]")).Count;
			return count;
		}
		public static Control GetCountryOptionText(string countryName)
		{
			return new Control($"[data-qa='mat-option-country-{countryName.ToLower()}'] .mat-option-text");
		}

		#endregion

		public static class ColumnNameString
		{
			public const string Status = "Status";
			public const string Received = "Received";
			public const string Company = "Company";
			public const string Product = "Product";
			public const string Category = "Category";
			public const string HqCountry = "HQ Country";
			public const string Source = "Source";
			public const string Updated = "Updated";
		}

		public static class ColumnNameSelector
		{
			public const string Status = "td-status";
			public const string Received = "td-received";
			public const string Company = "td-company-name";
			public const string Product = "td-product-name";
			public const string Category = "td-category-name";
			public const string HqCountry = "td-country-code";
			public const string Source = "td-source";
			public const string Updated = "td-updated";
		}
	}
}
