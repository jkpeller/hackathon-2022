using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class VendorPage : BasePage
	{
		#region Variables and constructor

		private const string PageUrl = "/vendors";

		public VendorPage()
		{
			Url = PageUrl;
		}

		#endregion

		#region Single Elements

		public readonly Control TitleVendorName = new Control(ControlUtility.GetElementSelector("vendor-name-title"));

		//these are single, unique elements in the UI to be located in the DOM when they are used by calling methods in the Control class
		public readonly Control TableHeaderName = new Control(ControlUtility.GetElementSelector("th-name"));
		public readonly Control TableHeaderProducts = new Control(ControlUtility.GetElementSelector("th-product-count"));
		public readonly Control TableHeaderCategories = new Control(ControlUtility.GetElementSelector("th-category-count"));
		public readonly Control TableHeaderWebsite = new Control(ControlUtility.GetElementSelector("th-website"));
		public readonly Control TableHeaderCreatedOnUtc = new Control(ControlUtility.GetElementSelector("th-created-on-utc"));

		public readonly Control InputVendorName = new Control(ControlUtility.GetElementSelector("input-vendor-name"));
		public readonly Control SelectVendorStatus = new Control(ControlUtility.GetElementSelector("mat-select-vendor-status"));

		public readonly Control ErrorMessageMinimumVendorName = new Control(ControlUtility.GetElementSelector("mat-error-input-vendor-name"));

		public readonly Control SelectCategory = new Control(ControlUtility.GetElementSelector("mat-select-category"));
		public readonly Control VendorActiveStatus = new Control(ControlUtility.GetElementSelector("mat-option-vendor-status-active"));
		public readonly Control VendorArchivedStatus = new Control(ControlUtility.GetElementSelector("mat-option-vendor-status-archived"));

		//add vendor
		public readonly Control ButtonAddVendor = new Control(ControlUtility.GetElementSelector("button-add-vendor"));
		public readonly Control InputVendorCompanyName = new Control(ControlUtility.GetElementSelector("input-vendor-company-name"));
		public readonly Control InputVendorCompanyWebsite = new Control(ControlUtility.GetElementSelector("input-vendor-company-website"));
		public readonly Control InputVendorTwitter = new Control(ControlUtility.GetElementSelector("input-vendor-twitter"));
		public readonly Control InputVendorFacebook = new Control(ControlUtility.GetElementSelector("input-vendor-facebook"));
		public readonly Control InputVendorLinkedIn = new Control(ControlUtility.GetElementSelector("input-vendor-linkedin"));
		public readonly Control InputVendorYouTube = new Control(ControlUtility.GetElementSelector("input-vendor-youtube"));
		public readonly Control InputVendorInstagram = new Control(ControlUtility.GetElementSelector("input-vendor-instagram"));
		public readonly Control SelectVendorCountry = new Control(ControlUtility.GetElementSelector("mat-select-vendor-country"));
		public readonly Control InputCountrySearch = new Control(".mat-select-search-inner .mat-input-element");
		public readonly Control InputCategorySearch = new Control(".mat-select-search-inner .mat-input-element");

		public readonly Control ButtonCancelSubmitVendorForm = new Control(ControlUtility.GetElementSelector("button-cancel"));
		public readonly Control ButtonSubmitVendorForm = new Control(ControlUtility.GetElementSelector("button-submit-vendor-form"));
		public readonly Control ErrorMessageCompanyName = new Control(ControlUtility.GetElementSelector("mat-error-required-vendor-name"));
		public readonly Control ErrorMessageDuplicateVendor = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-detected"));
		public readonly Control ErrorMessageRequiredWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-required-vendor-url"));
		public readonly Control ErrorMessageInvalidWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-url"));
		public readonly Control ErrorMessageCountry = new Control(ControlUtility.GetElementSelector("mat-error-required-vendor-country"));
		public readonly Control ErrorMessageTwitterUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-twitter-url"));
		public readonly Control ErrorMessageFacebookUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-facebook-url"));
		public readonly Control ErrorMessageLinkedInUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-linkedin-url"));
		public readonly Control ErrorMessageYoutubeUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-youtube-url"));
		public readonly Control ErrorMessageInstagramUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-vendor-instagram-url"));

		public readonly Control InputYearFounded = new Control(ControlUtility.GetElementSelector("input-vendor-year-founded"));
		public readonly Control InputPhoneNumber = new Control(ControlUtility.GetElementSelector("input-vendor-phone-number"));
		public readonly Control InputAbout = new Control(ControlUtility.GetElementSelector("input-vendor-about"));
		public readonly Control InputVendorAddress1 = new Control(ControlUtility.GetElementSelector("input-vendor-streetAddress1"));
		public readonly Control InputVendorAddress2 = new Control(ControlUtility.GetElementSelector("input-vendor-streetAddress2"));
		public readonly Control SelectStateProvince = new Control("[formcontrolname='stateProvince']");
		public readonly Control InputStateProvince = new Control(ControlUtility.GetElementSelector("input-vendor-state-province-name"));
		public readonly Control SelectInputStateProvince = new Control(ControlUtility.GetElementSelector("mat-select-vendor-state-province"));		
		public readonly Control OptionFirstStateProvince = new Control("[role='option']:nth-of-type(1)");
		public readonly Control InputVendorCity = new Control(ControlUtility.GetElementSelector("input-vendor-city"));
		public readonly Control InputVendorZipPostalCode = new Control(ControlUtility.GetElementSelector("input-vendor-zip-postal-code"));
		public readonly Control SelectCountryNoMatches = new Control(".mat-select-search-no-entries-found");

		//view vendor details
		public readonly Control DetailsVendorStatus = new Control(ControlUtility.GetElementSelector("vendor-status"));
		public readonly Control DetailsVendorSelectVendorStatus = new Control(ControlUtility.GetElementSelector("mat-select-vendor-status"));
		public readonly Control DetailsVendorActiveVendorStatus = new Control(ControlUtility.GetElementSelector("mat-option-vendor-status-active"));
		public readonly Control DetailsVendorArchivedVendorStatus = new Control(ControlUtility.GetElementSelector("mat-option-vendor-status-archived"));
		public readonly Control DetailsVendorName = new Control(ControlUtility.GetElementSelector("vendor-name"));
		public readonly Control DetailsVendorAddress = new Control(ControlUtility.GetElementSelector("vendor-address"));
		public readonly Control DetailsVendorWebsiteUrl = new Control(ControlUtility.GetElementSelector("vendor-website-url"));
		public readonly Control DetailsYearFounded = new Control(ControlUtility.GetElementSelector("vendor-year-founded"));
		public readonly Control DetailsPhoneNumber = new Control(ControlUtility.GetElementSelector("vendor-phone"));
		public readonly Control DetailsAbout = new Control(ControlUtility.GetElementSelector("vendor-about"));
		public readonly Control DetailsVendorLinkedinUrl = new Control(ControlUtility.GetElementSelector("vendor-linkedin-url"));
		public readonly Control DetailsVendorTwitterUrl = new Control(ControlUtility.GetElementSelector("vendor-twitter-url"));
		public readonly Control DetailsVendorFacebookUrl = new Control(ControlUtility.GetElementSelector("vendor-facebook-url"));
		public readonly Control DetailsVendorYoutubeUrl = new Control(ControlUtility.GetElementSelector("vendor-youtube-url"));
		public readonly Control DetailsVendorInstagramUrl = new Control(ControlUtility.GetElementSelector("vendor-instagram-url"));

		//edit vendor details
		public readonly Control EditVendorDetailsTitle = new Control(ControlUtility.GetElementSelector("vendor-card-title"));
		public readonly Control ButtonEditVendor = new Control(ControlUtility.GetElementSelector("button-edit-basic-info"));

		//vendor products
		public readonly Control VendorProductsCardTitle = new Control(ControlUtility.GetElementSelector("vendor-products-card-title"));
		public readonly Control ButtonAddVendorProduct = new Control(ControlUtility.GetElementSelector("button-add-vendor-product"));
		public readonly Control VendorProductsNoResult = new Control(ControlUtility.GetElementSelector("no-products-result"));

		//vendor logo
		public readonly Control ContainerVendorLogo = new Control(ControlUtility.GetElementSelector("vendor-logo"));
		public readonly Control ImageVendorLogo = new Control(ControlUtility.GetElementSelector("img-logo"));
		public readonly Control ButtonAddChangeLogo = new Control(ControlUtility.GetElementSelector("button-add-change-logo"));
		public readonly Control ButtonRemoveLogo = new Control(ControlUtility.GetElementSelector("button-remove-logo"));

		//vendor notes card
		public readonly Control Notes = new Control(ControlUtility.GetElementSelector("p-notes"));
		public readonly Control TextAreaNotes = new Control(ControlUtility.GetElementSelector("text-area-notes"));
		public readonly Control ButtonEditVendorNotes = new Control(ControlUtility.GetElementSelector("button-edit-vendor-notes"));
		public readonly Control ButtonCancelVendorNotes = new Control(ControlUtility.GetElementSelector("button-cancel-notes"));
		public readonly Control ButtonSaveVendorNotes = new Control(ControlUtility.GetElementSelector("button-save-notes"));

		#endregion

		public static class ColumnNameString
		{
			public const string Name = "Name";
			public const string Products = "Products";
			public const string Categories = "Categories";
			public const string Website = "Website";
			public const string Status = "Status";
			public const string ModifiedOnUtc = "Updated";
			public const string CreatedOnUtc = "Created";
		}

		public static class ColumnNameSelector
		{
			public const string Name = "td-name";
			public const string Products = "td-product-count";
			public const string Categories = "td-category-count";
			public const string Website = "td-website";
			public const string Status = "td-status";
			public const string ModifiedOnUtc = "td-modified-on-utc";
			public const string CreatedOnUtc = "td-created-on-utc";
		}

		/// <summary>
		/// Returns a control for the category inside a category dropdown list.
		/// </summary>
		/// <param name="name"></param>
		/// <returns />
		public static Control GetCategoryFromFilterByName(string name)
		{
			return new Control(ControlUtility.GetCategoryFromFilterByName(name));
		}

		/// <summary>
		/// Returns a control for the link to the vendor detail page for the row number passed in.
		/// </summary>
		/// <param name="rowNumber"></param>
		/// <returns />
		public static Control GetVendorLinkByRowNumber(int rowNumber)
		{
			return new Control($"[role='row']:nth-of-type({rowNumber}) [data-qa='td-name'] a");
		}
	}
}