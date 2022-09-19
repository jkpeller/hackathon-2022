using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class ProductDetailsPage : BasePage
	{
		#region Sidenav anchors

		public readonly Control SelectProductStatus = new Control(ControlUtility.GetElementSelector("mat-select-product-status"));
		public readonly Control OptionProductStatusActive = new Control(ControlUtility.GetElementSelector("mat-option-product-status-status-active"));
		public readonly Control OptionProductStatusArchived = new Control(ControlUtility.GetElementSelector("mat-option-product-status-status-archived"));
		public readonly Control OptionProductStatusDiscontinued = new Control(ControlUtility.GetElementSelector("mat-option-product-status-status-discontinued"));

		public readonly Control LinkRightSidnavProductDetails = new Control(ControlUtility.GetElementSelector("link-product-details"));
		public readonly Control IconRightSidenavProductDetailsEdit = new Control(ControlUtility.GetElementSelector("mat-icon-product-details-edit"));
		public readonly Control IconRightSidenavProductDetailsValidationError = new Control(ControlUtility.GetElementSelector("mat-icon-product-details-validation-error"));

		public readonly Control LinkRightSidnavProductIntegrations = new Control(ControlUtility.GetElementSelector("link-product-integrations"));

		public readonly Control LinkRightSidnavProductMedias = new Control(ControlUtility.GetElementSelector("link-product-media"));
		public readonly Control IconRightSidenavProductMediasEdit = new Control(ControlUtility.GetElementSelector("mat-icon-product-media-edit"));
		public readonly Control IconRightSidenavProductMediasValidationError = new Control(ControlUtility.GetElementSelector("mat-icon-product-media-validation-error"));

		public readonly Control LinkRightSidenavVendorNotes = new Control(ControlUtility.GetElementSelector("link-vendor-notes"));
		public readonly Control IconRightSidenavVendorNotes = new Control(ControlUtility.GetElementSelector("mat-icon-vendor-notes"));

		#endregion

		#region Publish status

		public readonly Control ConfirmationDialogMessage = new Control(ControlUtility.GetElementSelector("confirmation-dialog-supporting-text"));

		#endregion

		#region Site product section

		public readonly Control ButtonEditSiteProducts = new Control(ControlUtility.GetElementSelector("button-edit-site-products"));
		public readonly Control ButtonSaveSiteProducts = new Control(ControlUtility.GetElementSelector("button-save-site-products"));
		public readonly Control ButtonCancelEditSiteProducts = new Control(ControlUtility.GetElementSelector("button-cancel-edit-site-products"));

		public readonly Control CapterraMessageNoSiteProducts = new Control($"{ControlUtility.GetElementSelector("capterra-site-container")} {ControlUtility.GetElementSelector("message-no-site-products")}");

		#endregion

		#region Details section

		//Product Logo
		public readonly Control InvalidProductLogoRequired = new Control(ControlUtility.GetElementSelector("mat-error-required-input-logo"));
		public readonly Control ContainerProductLogo = new Control(ControlUtility.GetElementSelector("product-logo"));
		public readonly Control ProductLogo = new Control(ControlUtility.GetElementSelector("img-logo"));
		public readonly Control ButtonAddChangeLogo = new Control(ControlUtility.GetElementSelector("button-add-change-logo"));
		public readonly Control ButtonRemoveLogo = new Control(ControlUtility.GetElementSelector("button-remove-logo"));
		public readonly Control FileInput = new Control(ControlUtility.GetElementSelector("file-input"));
		public readonly Control ImageProductLogo = new Control(ControlUtility.GetElementSelector("img-logo"));

		//Product Reviews Count
		public readonly Control ProductReviewsCount = new Control(ControlUtility.GetElementSelector("product-reviews-count"));

		//Product Name
		public readonly Control InputProductName = new Control(ControlUtility.GetElementSelector("input-product-name"));
		public readonly Control InvalidProductNameMinLength = new Control(ControlUtility.GetElementSelector("mat-error-minlength-input-product-name"));
		public readonly Control InvalidProductNameRequired = new Control(ControlUtility.GetElementSelector("mat-error-required-input-product-name"));

		//duplicate vendor product check
		public readonly Control DuplicateVendorProductDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-detected"));
		public readonly Control DuplicateVendorProductNoneDetected = new Control(ControlUtility.GetElementSelector("message-duplicate-vendor-product-none"));
				
		//Product Url
		public readonly Control InputProductWebsiteUrl = new Control(ControlUtility.GetElementSelector("input-product-website-url"));
		public readonly Control InvalidProductWebsiteUrl = new Control(ControlUtility.GetElementSelector("mat-error-invalid-url-input-product-website-url"));

		//Target Company Size
		public readonly Control CheckboxTargetCompanySizeSelfEmployed = new Control(ControlUtility.GetElementSelector("mat-checkbox-self-employed"));
		public readonly Control InvalidTargetCompanySizeRequired = new Control(ControlUtility.GetElementSelector("mat-error-invalid-checkbox-list-target-company-size"));

		//Target Industries
		public readonly Control InputTargetIndustries = new Control(ControlUtility.GetElementSelector("mat-input-target-industries"));

		//Deployment Options
		public readonly Control CheckboxDeploymentOptionCloudSaasWebBased = new Control(ControlUtility.GetElementSelector("mat-checkbox-cloud, saas, web-based"));
		public readonly Control CheckboxDeploymentOptionDesktopMac = new Control(ControlUtility.GetElementSelector("mat-checkbox-mac-desktop"));
		public readonly Control CheckboxDeploymentOptionDesktopWindows = new Control(ControlUtility.GetElementSelector("mat-checkbox-windows-desktop"));
		public readonly Control CheckboxDeploymentOptionDesktopLinux = new Control(ControlUtility.GetElementSelector("mat-checkbox-linux-desktop"));
		public readonly Control CheckboxDeploymentOptionDesktopChromebook = new Control(ControlUtility.GetElementSelector("mat-checkbox-chromebook-desktop"));
		public readonly Control CheckboxDeploymentOptionOnPremiseWindows = new Control(ControlUtility.GetElementSelector("mat-checkbox-windows-on-premise"));
		public readonly Control CheckboxDeploymentOptionOnPremiseLinux = new Control(ControlUtility.GetElementSelector("mat-checkbox-linux-on-premise"));
		public readonly Control CheckboxDeploymentOptionMobileAndroid = new Control(ControlUtility.GetElementSelector("mat-checkbox-android-mobile"));
		public readonly Control CheckboxDeploymentOptionMobileIPhone = new Control(ControlUtility.GetElementSelector("mat-checkbox-iphone-mobile"));
		public readonly Control CheckboxDeploymentOptionMobileIPad = new Control(ControlUtility.GetElementSelector("mat-checkbox-ipad-mobile"));
		public readonly Control InvalidDeploymentOptionRequired = new Control(ControlUtility.GetElementSelector("mat-error-invalid-checkbox-list-deployment-options"));

		//Licensing Models
		public readonly Control SelectLicensingModel = new Control(ControlUtility.GetElementSelector("mat-select-licensing-model"));
		public readonly Control OptionLicensingModelProprietary = new Control(ControlUtility.GetElementSelector("mat-option-licensing-model-proprietary"));
		public readonly Control OptionLicensingModelOpenSource = new Control(ControlUtility.GetElementSelector("mat-option-licensing-model-open source"));
		public readonly Control OptionLicensingModelNone = new Control(ControlUtility.GetElementSelector("mat-option-licensing-model-none"));

		//Supported Languages
		public readonly Control InputSupportedLanguages = new Control(ControlUtility.GetElementSelector("mat-input-supported-languages"));

		//Supported Countries
		public readonly Control InputSupportedCountries = new Control(ControlUtility.GetElementSelector("mat-input-supported-countries"));

		//Social Media Profiles
		public readonly Control InputTwitterSocialMediaProfile = new Control(ControlUtility.GetElementSelector("input-product-twitter"));
		public readonly Control InputFacebookSocialMediaProfile = new Control(ControlUtility.GetElementSelector("input-product-facebook"));
		public readonly Control InputLinkedInSocialMediaProfile = new Control(ControlUtility.GetElementSelector("input-product-linkedin"));
		public readonly Control InputYoutubeSocialMediaProfile = new Control(ControlUtility.GetElementSelector("input-product-youtube"));
		public readonly Control InputInstagramSocialMediaProfile = new Control(ControlUtility.GetElementSelector("input-product-instagram"));

		//Mobile App URLs
		public readonly Control InputAndroidMobileAppUrl = new Control(ControlUtility.GetElementSelector("input-product-android"));
		public readonly Control InputIosMobileAppUrl = new Control(ControlUtility.GetElementSelector("input-product-ios"));

		//Buttons
		public readonly Control ButtonCancelProductDetailsEdit = new Control(ControlUtility.GetElementSelector("button-cancel-edit"));
		public readonly Control ButtonSaveProductDetailsChanges = new Control(ControlUtility.GetElementSelector("button-save-changes"));

		#endregion

		#region Pricing section

		public readonly Control IsPricingVisibleMessageDisplay = new Control(ControlUtility.GetElementSelector("pricing-visibility-text"));
		public readonly Control SelectCurrency = new Control(ControlUtility.GetElementSelector("mat-select-currency"));
		public readonly Control SelectPricingRange = new Control(ControlUtility.GetElementSelector("mat-select-price-range"));
		public readonly Control SelectPaymentFrequency = new Control(ControlUtility.GetElementSelector("mat-select-payment-frequency"));
		public readonly Control TextareaPricingDescription = new Control(ControlUtility.GetElementSelector("text-area-pricing-description-"));
		public readonly Control ButtonSaveProductPricing = new Control(ControlUtility.GetElementSelector("button-save-product-pricing"));
		public readonly Control SelectCCVerifyRequired = new Control(ControlUtility.GetElementSelector("mat-select-cc-required"));
        public readonly Control ButtonAddAnotherPlan = new Control(ControlUtility.GetElementSelector("button-add-another-plan"));
        public readonly Control InputDiscountTypes = new Control(ControlUtility.GetElementSelector("mat-input-discount-types"));
        public readonly Control ModifiedOnUtc = new Control(ControlUtility.GetElementSelector("product-pricing-modified-on-utc"));
		public readonly Control ModifiedBy = new Control(ControlUtility.GetElementSelector("product-pricing-modified-by"));
        public readonly Control InputVendorPricingUrl = new Control(ControlUtility.GetElementSelector("input-vendor-pricing-url"));

		// Plan section

		#endregion

		#region Categories section

		private const string CategoryTablePrefixSelector = "[id='product-categories'] table";
		public readonly Control CategoryTableTbody = new Control($"{CategoryTablePrefixSelector} tbody");
		public readonly Control CategoryTableHeaderChangedByUserFullName = new Control(ControlUtility.GetElementSelector("th-changedByUserFullName"));
		

		#endregion

		#region Integrations section

		public readonly Control SelectOpenApi = new Control(ControlUtility.GetElementSelector("mat-select-open-api"));
		public readonly Control InputProductIntegrationName = new Control("#product-integrations input[data-qa=\"input-product-name\"]");
		public readonly Control ButtonAddProductIntegration = new Control(ControlUtility.GetElementSelector("button-add-product-integration"));
		public readonly Control ButtonCancelProductIntegration = new Control(ControlUtility.GetElementSelector("button-cancel-product-integrations"));
		public readonly Control ButtonSaveProductIntegration = new Control(ControlUtility.GetElementSelector("button-save-product-integrations"));
		public readonly Control ProductIntegrationNoResultsMessage = new Control(ControlUtility.GetElementSelector("no-integrations-result"));
		public readonly Control MatProgressBarIntegrations = new Control(ControlUtility.GetElementSelector("mat-progress-bar-integrations"));

		#endregion

		#region Medias section

		public readonly Control TabMediasCapterra = new Control("#product-media .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"1\"]");
		public readonly Control IconEditTabMediasCapterra = new Control(ControlUtility.GetElementSelector("mat-icon-edit-tab-capterra"));
		public readonly Control ValidationMessageScreenshotsCapterra = new Control(ControlUtility.GetElementSelector("validation-message-screenshots-capterra"));
		public readonly Control ValidationMessageVideosCapterra = new Control(ControlUtility.GetElementSelector("validation-message-videos-capterra"));
		public readonly Control ButtonAddProductScreenshotCapterra = new Control(ControlUtility.GetElementSelector("button-add-product-screenshot-capterra"));
		public readonly Control ButtonAddProductVideoCapterra = new Control(ControlUtility.GetElementSelector("button-add-product-video-capterra"));
		public readonly Control ButtonSaveMediasCapterra = new Control(ControlUtility.GetElementSelector("button-save-product-media-capterra"));
		public readonly Control ButtonCancelMediasCapterra = new Control(ControlUtility.GetElementSelector("button-cancel-product-media-capterra"));
		public const string MatProgressBarMediasCapterra = "mat-progress-bar-medias-capterra";

		public readonly Control TabMediasGetApp = new Control("#product-media .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"2\"]");
		public readonly Control IconEditTabMediasGetApp = new Control(ControlUtility.GetElementSelector("mat-icon-edit-tab-getapp"));
		public readonly Control ValidationMessageScreenshotsGetApp = new Control(ControlUtility.GetElementSelector("validation-message-screenshots-getapp"));
		public readonly Control ValidationMessageVideosGetApp = new Control(ControlUtility.GetElementSelector("validation-message-videos-getapp"));
		public readonly Control ButtonAddProductScreenshotGetApp = new Control(ControlUtility.GetElementSelector("button-add-product-screenshot-getapp"));
		public readonly Control ButtonAddProductVideoGetApp = new Control(ControlUtility.GetElementSelector("button-add-product-video-getapp"));
		public readonly Control ButtonSaveMediasGetApp = new Control(ControlUtility.GetElementSelector("button-save-product-media-getapp"));
		public readonly Control ButtonCancelMediasGetApp = new Control(ControlUtility.GetElementSelector("button-cancel-product-media-getapp"));
		public const string MatProgressBarMediasGetApp = "mat-progress-bar-medias-getapp";

		public readonly Control TabMediasSoftwareAdvice = new Control("#product-media .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"3\"]");
		public readonly Control IconEditTabMediasSoftwareAdvice = new Control(ControlUtility.GetElementSelector("mat-icon-edit-tab-software-advice"));
		public readonly Control ValidationMessageScreenshotsSoftwareAdvice = new Control(ControlUtility.GetElementSelector("validation-message-screenshots-software-advice"));
		public readonly Control ValidationMessageVideosSoftwareAdvice = new Control(ControlUtility.GetElementSelector("validation-message-videos-software-advice"));
		public readonly Control ButtonAddProductVideoSoftwareAdvice = new Control(ControlUtility.GetElementSelector("button-add-product-video-software-advice"));
		public readonly Control ButtonAddProductScreenshotSoftwareAdvice = new Control(ControlUtility.GetElementSelector("button-add-product-screenshot-software-advice"));
		public readonly Control ButtonSaveMediasSoftwareAdvice = new Control(ControlUtility.GetElementSelector("button-save-product-media-software-advice"));
		public readonly Control ButtonCancelMediasSoftwareAdvice = new Control(ControlUtility.GetElementSelector("button-cancel-product-media-software-advice"));
		public const string MatProgressBarMediasSoftwareAdvice = "mat-progress-bar-medias-software-advice";

		public readonly Control InvalidVideoURL = new Control(ControlUtility.GetElementSelector("mat-url-invlid"));

		#endregion

		#region Descriptions section

		public readonly Control TabDescriptionsCapterra = new Control("#product-descriptions .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"1\"]");
		public readonly Control TextareaCapterraTargetMarket = new Control(ControlUtility.GetElementSelector("textarea-capterra-target-market"));
		public readonly Control TextareaCapterraShortDescription = new Control(ControlUtility.GetElementSelector("textarea-capterra-short-description"));
		public readonly Control TextareaCapterraLongDescription = new Control(ControlUtility.GetElementSelector("textarea-capterra-long-description"));
		public readonly Control ButtonSaveCapterraProductDescriptions = new Control(ControlUtility.GetElementSelector("button-save-product-capterra-descriptions"));
		public const string MatProgressBarDescriptionsCapterra = "mat-progress-bar-descriptions-capterra";

		public readonly Control TabDescriptionsSoftwareAdvice = new Control("#product-descriptions .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"3\"]");
		public readonly Control TextareaSoftwareAdviceLongDescription = new Control(ControlUtility.GetElementSelector("textarea-software-advice-long-description"));
		public readonly Control ButtonSaveSoftwareAdviceProductDescriptions = new Control(ControlUtility.GetElementSelector("button-save-product-software-advice-descriptions"));
		public const string MatProgressBarDescriptionsSoftwareAdvice = "mat-progress-bar-descriptions-software-advice";

		public readonly Control TabDescriptionsGetApp = new Control("#product-descriptions .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"2\"]");
		public readonly Control TextareaGetAppTagline = new Control(ControlUtility.GetElementSelector("textarea-getapp-tagline"));
		public readonly Control TextareaGetAppShortDescription = new Control(ControlUtility.GetElementSelector("textarea-getapp-short-description"));
		public readonly Control TextareaGetAppLongDescription = new Control(ControlUtility.GetElementSelector("textarea-getapp-long-description"));
		public readonly Control TextareaGetAppBenefits = new Control(ControlUtility.GetElementSelector("textarea-getapp-benefits"));
		public readonly Control ButtonSaveGetAppProductDescriptions = new Control(ControlUtility.GetElementSelector("button-save-product-getapp-descriptions"));
		public const string MatProgressBarDescriptionsGetApp = "mat-progress-bar-descriptions-getapp";

		#endregion

		#region Vendor notes section

		public readonly Control LinkToVendorPage = new Control(ControlUtility.GetElementSelector("link-vendor-page"));
		public readonly Control VendorNotes = new Control(ControlUtility.GetElementSelector("p-notes"));
		public readonly Control VendorNotesGoToVendorMessage = new Control(ControlUtility.GetElementSelector("vendor-notes-go-to-message"));

		#endregion

		#region Control methods

        public static Control GetInputPlanName(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"input-plan-name-{index}"));
        }

        public static Control GetRadioButtonPriceTypeNumeric(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-radio-button-price-type-numeric-{index}"));
        }

        public static Control GetRadioButtonPriceTypeCustom(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-radio-button-price-type-custom-{index}"));
        }

        public static Control GetInputStartingPrice(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"input-starting-price-{index}"));
        }

        public static Control GetInputStartingPriceDecimalErrorMessage(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-error-starting-price-decimal-{index}"));
        }

        public static Control GetInputStartingPriceMaxErrorMessage(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-error-starting-price-max-{index}"));
        }

		public static Control GetInputEndingPrice(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"input-ending-price-{index}"));
        }

        public static Control GetInputEndingPriceGreaterThanErrorMessage(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-error-ending-price-greater-than-{index}"));
        }

        public static Control GetInputEndingPriceMaxErrorMessage(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-error-ending-price-max-{index}"));
        }

        public static Control GetInputEndingPriceDecimalErrorMessage(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-error-ending-price-decimal-{index}"));
        }

		public static Control GetInputCustomPrice(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"input-custom-price-{index}"));
        }

		public static Control GetSelectPricingModel(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-select-pricing-model-{index}"));
        }

        public static Control GetSelectPaymentFrequency(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-select-payment-frequency-{index}"));
        }

        public static Control GetTextareaPlanDescription(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"text-area-plan-description-{index}"));
        }

        public static Control GetChipListInput(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-chip-list-input-{index}"));
        }

        public static Control GetAttributesAutocomplete(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"attributes-autocomplete-{index}"));
        }

		public static Control GetOptionPricingModelByName(string optionName, int index)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-option-pricing-model-{optionName.ToLower()}-{index}"));
        }

		public static Control GetOptionPaymentFrequencyByName(string optionName, int index)
		{
		    return new Control(ControlUtility.GetElementSelector($"mat-option-pricing-model-{optionName.ToLower()}-{index}"));
		}

        public static Control GetOptionDiscountTypeByName(string optionName)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-option-discount-types-{optionName}"));
        }

        public static Control GetRemoveChipDiscountTypeByName(string optionName)
        {
            return new Control(ControlUtility.GetElementSelector($"mat-icon-remove-chip-{optionName}"));
        }

		public static Control GetButtonDeletePlan(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"button-delete-plan-{index}"));
        }

        public static Control GetIconReOrderPlan(int index)
        {
            return new Control(ControlUtility.GetElementSelector($"icon-re-order-plan-{index}"));
        }

		public static Control GetSiteProductChipRemoveBySiteProductName(string siteProductName)
		{
			return new Control($"{ControlUtility.GetChipListItemByDisplayName(siteProductName)} {ControlUtility.GetElementSelector("mat-icon-remove-chip-site-product")}");
		}

		public string GetSiteProductChipLinkBySiteProductName(string siteProductName)
		{
			return new Control($"{ControlUtility.GetChipListItemByDisplayName(siteProductName.ToLower())} {ControlUtility.GetElementSelector("link-site-product-name")}").GetHref();
		}

		public static Control GetDetailsCheckboxByOptionName(string optionName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-checkbox-{optionName}"));
		}

		public static Control GetCurrencyOptionByCode(string currencyCode)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-currency-{currencyCode}"));
		}

		public static Control GetPriceRangeOptionByName(string priceRange)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-price-range-{priceRange}"));
		}

		public static Control GetPaymentFrequencyOptionByName(string paymentFrequency)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-pricing-model-{paymentFrequency}"));
		}

		public static Control GetCCVerifyRequiredOptionByName(string ccVerifyOption)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-cc-verify-{ccVerifyOption}"));
		}
		

		public static Control GetTargetIndustryOptionByName(string optionName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-target-industries-{optionName}"));
		}

		public static Control GetTargetIndustryActiveChipByName(string optionName)
        {
			return new Control(ControlUtility.GetElementSelector($"mat-chip-{optionName}"));
		}

		public static Control GetTargetIndustryActiveChipRemoveButtonByName(string optionName)
        {
			return new Control(ControlUtility.GetElementSelector($"mat-icon-remove-chip-{optionName}"));
		}

		public static Control GetSupportedLanguageOptionByName(string optionName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-supported-languages-{optionName}"));
		}

		public static Control GetSupportedCountryOptionByName(string optionName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-supported-countries-{optionName}"));
		}

		public static Control GetCategoryNameFromCategoriesTableByRowNumber(int rowNumber)
		{
			return new Control($"{CategoryTablePrefixSelector} {ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("link-category")}");
		}

		public static Control GetSiteIconFromCategoriesTableByRowNumber(int rowNumber, string siteName)
		{
			return new Control($"{CategoryTablePrefixSelector} {ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector($"img-{siteName}")}");
		}

		public static Control GetChangedByFullNameFromCategoriesTableByRowNumber(int rowNumber)
		{
			return new Control($"{CategoryTablePrefixSelector} {ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("td-changedByUserFullName")}");
		}

		public static Control GetCategoryStatusNameFromCategoriesTableByRowNumber(int rowNumber)
		{
			return new Control($"{CategoryTablePrefixSelector} {ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("td-category-status")}");
		}
		
		public static Control GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(int rowNumber)
		{
			return new Control($"{CategoryTablePrefixSelector} {ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("td-category-verification")}");
		}

		public static Control GetProductScreenshot(int fileId)
		{
			return new Control(ControlUtility.GetElementSelector($"img-product-screenshot-{fileId}"));
		}

		public static Control GetTextAreaProductVideoUrl(int videoId)
		{
			return new Control(ControlUtility.GetElementSelector($"textarea-product-video-url-{videoId}"));
		}

		public static Control GetOpenApiOption(VerifiedType verifiedType)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-open-api-{verifiedType.ToString().ToLower()}"));
		}

		public static Control GetProductIntegrationOptionByName(string integrationProductName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-suggested-product-{integrationProductName.ToLower()}"));
		}

		public static Control GetImageProductIntegrationLogo(string integrationProductId)
		{
			return new Control(ControlUtility.GetElementSelector($"img-integration-product-logo-{integrationProductId}"));
		}

		public static Control GetLinkProductIntegration(string integrationProductId)
		{
			return new Control(ControlUtility.GetElementSelector($"link-integration-product-{integrationProductId}"));
		}

		public static Control GetButtonRemoveProductIntegration(string integrationProductName)
		{
			return new Control(ControlUtility.GetElementSelector($"button-remove-product-integration-{integrationProductName.ToLower()}"));
		}

		public static Control GetFeatureCheckboxByFeatureName(string featureName)
		{
			return new Control(ControlUtility.GetElementSelector($"input-checkbox-feature-{featureName.ToLower()}"));
		}

		#endregion

		#region Feature section
		public readonly Control ButtonSaveFeatures = new Control(ControlUtility.GetElementSelector("button-save-features"));
		#endregion
	}
}