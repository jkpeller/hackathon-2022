using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class CategoryDetailsPage : BasePage
	{
		#region Single Elements

		//display view
		public readonly Control LabelCategoryName = new Control(ControlUtility.GetElementSelector("label-category-name"));
		public readonly Control DisplayCategoryName = new Control(ControlUtility.GetElementSelector("category-name"));
		public readonly Control DisplayCategoryDefinition = new Control(ControlUtility.GetElementSelector("category-definition"));
		public readonly Control ArchivedStatusLabel = new Control(".h2-info-archived");

		//edit view
		public readonly Control InputEditCategoryName = new Control(ControlUtility.GetElementSelector("input-category-name"));
		public readonly Control InputEditCategoryDefinition = new Control(ControlUtility.GetElementSelector("textarea-category-definition"));
		public readonly Control InputEditCapterraSeoName = new Control(ControlUtility.GetElementSelector("textarea-capterra-seo-name"));
		public readonly Control InputEditGetAppSeoName = new Control(ControlUtility.GetElementSelector("textarea-getapp-seo-name"));
		public readonly Control InputEditSoftwareAdviseSeoName = new Control(ControlUtility.GetElementSelector("textarea-software-advice-seo-name"));
		public readonly Control InputTagName = new Control(ControlUtility.GetElementSelector("input-tag-name"));
		public readonly Control InputTagDefinition = new Control(ControlUtility.GetElementSelector("input-definition"));
		public readonly Control InputTagTypes = new Control(ControlUtility.GetElementSelector("mat-select-tag-types"));
		public readonly Control InputTagTypesTextValue = new Control("[data-qa='mat-select-tag-types'] .mat-select-value-text");
		public readonly Control InputAssociatedTags = new Control(ControlUtility.GetElementSelector("mat-input-associated-tags"));

		public readonly Control ButtonEditCategory = new Control(ControlUtility.GetElementSelector("button-edit-category"));
		public readonly Control ButtonCancelCategoryEdit = new Control(ControlUtility.GetElementSelector("button-cancel-edit"));
		public readonly Control ButtonSaveCategoryChanges = new Control(ControlUtility.GetElementSelector("button-save-changes"));
		public readonly Control ButtonSaveCategoryCapterraSeoNames = new Control(ControlUtility.GetElementSelector("button-save-category-capterra-seo-names"));
		public readonly Control ButtonCancelCategoryCapterraSeoNames = new Control(ControlUtility.GetElementSelector("button-cancel-category-capterra-seo-names"));
		public readonly Control ButtonSaveCategoryGetAppSeoNames = new Control(ControlUtility.GetElementSelector("button-save-category-getapp-seo-names"));
		public readonly Control ButtonCancelCategoryGetAppSeoNames = new Control(ControlUtility.GetElementSelector("button-cancel-category-getapp-seo-names"));
		public readonly Control ButtonSaveCategorySoftwareAdviseSeoNames = new Control(ControlUtility.GetElementSelector("button-save-category-software-advice-seo-names"));
		public readonly Control ButtonCancelCategorySoftwareAdviseSeoNames = new Control(ControlUtility.GetElementSelector("button-cancel-category-software-advice-seo-names"));
		public readonly Control ButtonAddNewTag = new Control(ControlUtility.GetElementSelector("button-dialog-add"));
		public readonly Control ButtonEditCategoryTag = new Control(ControlUtility.GetElementSelector("button-edit-category-tags"));
		public readonly Control ButtonCancelNewTag = new Control(ControlUtility.GetElementSelector("button-dialog-cancel"));
		public readonly Control ButtonSaveTags = new Control(ControlUtility.GetElementSelector("button-save-changes"));

		public readonly Control PublishedOnCapterraSiteIcon = new Control(ControlUtility.GetElementSelector("img-capterra"));
		public readonly Control PublishedOnGetAppSiteIcon = new Control(ControlUtility.GetElementSelector("img-getapp"));
		public readonly Control PublishedOnSoftwareAdviceSiteIcon = new Control(ControlUtility.GetElementSelector("img-software-advice"));

		public readonly Control SeoNameCapTerraTab = new Control("#category-seo-names .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"1\"] ");
		public readonly Control SeoNameGetAppTab = new Control("#category-seo-names .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"2\"] ");
		public readonly Control SeoNameSoftwareAdviseTab = new Control("#category-seo-names .mat-tab-header .mat-tab-label-container .mat-tab-label[aria-posinset=\"3\"] ");

		//success messages 
		public readonly Control MessageNonDuplicateTag = new Control(ControlUtility.GetElementSelector("message-duplicate-tag-none"));
		
		//error messages
		public readonly Control ErrorMessageMinimumCategoryName = new Control(ControlUtility.GetElementSelector("mat-error-minlength-input-category-name"));
		public readonly Control ErrorMessageRequiredCategoryName = new Control(ControlUtility.GetElementSelector("mat-error-required-input-category-name"));
		public readonly Control ErrorMessageRequiredTagName = new Control(ControlUtility.GetElementSelector("mat-error-required-input-tag-name"));
		public readonly Control ErrorMessageRequiredTagDefinition = new Control(ControlUtility.GetElementSelector("mat-error-required-input-definition"));
		public readonly Control ErrorMessageDuplicateTag = new Control(ControlUtility.GetElementSelector("message-duplicate-tag-detected"));

		//site categories card
		public readonly Control ButtonEditSiteCategories = new Control(ControlUtility.GetElementSelector("button-edit-site-categories"));
		public readonly Control ButtonSaveSiteCategories = new Control(ControlUtility.GetElementSelector("button-save-site-categories"));
		public readonly Control ButtonCancelEditSiteCategories = new Control(ControlUtility.GetElementSelector("button-cancel-edit-site-categories"));

		//features card
		public readonly Control FeatureCardButtonAddFeature = new Control(ControlUtility.GetElementSelector("button-add-feature"));
		public readonly Control InputFeatureAutocomplete = new Control(ControlUtility.GetElementSelector("input-feature-autocomplete"));
		public readonly Control SelectFeatureType = new Control(ControlUtility.GetElementSelector("mat-select-feature-types"));
        public readonly Control SelectTagType = new Control(ControlUtility.GetElementSelector("mat-select-tag-types"));
		public readonly Control InputFeatureDefinition = new Control(ControlUtility.GetElementSelector("input-definition"));
		public readonly Control DialogButtonAddFeature = new Control(ControlUtility.GetElementSelector("button-dialog-add"));
		public readonly Control DialogButtonSaveFeatureUpdate = new Control(ControlUtility.GetElementSelector("button-dialog-save"));
		public readonly Control DialogButtonCancelFeatureUpdate = new Control(ControlUtility.GetElementSelector("button-dialog-cancel"));
		public readonly Control FeatureCount = new Control($"{ControlUtility.GetElementSelector("mat-card-title-feature-count")} #mat-badge-content-1");


		//product categories card
		public readonly Control SelectCategoryStatus = new Control(ControlUtility.GetElementSelector("mat-select-product-category-status"));
		public readonly Control SelectCategoryStatusActive = new Control(ControlUtility.GetElementSelector("mat-option-product-category-status-active"));
		public readonly Control SelectCategoryStatusArchived = new Control(ControlUtility.GetElementSelector("mat-option-product-category-status-archived"));
		public readonly Control ErrorMessageInputProductNameFilter = new Control(ControlUtility.GetElementSelector("mat-error-product-name"));
		public readonly Control ErrorMessageSelectCategoryStatusFilter = new Control(ControlUtility.GetElementSelector("mat-error-product-category-status"));
		public readonly Control InputFilterProductCategoryName = new Control(ControlUtility.GetElementSelector("product-category-name"));
		public readonly Control Name = new Control(ControlUtility.GetElementSelector("th-product-name"));
		public readonly Control PublishedOn = new Control(ControlUtility.GetElementSelector("th-published-on"));
		public readonly Control CategoryStatus = new Control(ControlUtility.GetElementSelector("th-status"));
		public readonly Control DateAdded = new Control(ControlUtility.GetElementSelector("th-created-on-utc"));
		public readonly Control AddedBy = new Control(ControlUtility.GetElementSelector("th-added-by"));
		public readonly Control DateChanged = new Control(ControlUtility.GetElementSelector("th-changed-on-utc"));
		public readonly Control ChangedBy = new Control(ControlUtility.GetElementSelector("th-changed-by"));
		public readonly Control VerificationStatus = new Control(ControlUtility.GetElementSelector("th-verification-status"));
		public readonly Control ProductCount = new Control($"{ControlUtility.GetElementSelector("mat-card-title-product-count")} #mat-badge-content-0");

		//category tags card
        public readonly Control SelectCategoryTagFilter = new Control(ControlUtility.GetElementSelector("mat-select-category-tag-id"));
		#endregion

		#region Control Methods

		public static Control GetSiteCategoryChipRemoveBySiteCategoryName(string siteCategoryName)
		{
			return new Control($"{ControlUtility.GetChipListItemByDisplayName(siteCategoryName)} {ControlUtility.GetElementSelector("mat-icon-remove-chip-site-category")}");
		}

		public static Control GetLinkCategoryDetailsNameByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("link-product-name")}");
		}

		public static int GetRowCount()
		{
			return new Control($"tbody {ControlUtility.GetTableRow()}").GetElementCount();
		}

		public static Control GetFeatureTypeDropdownOptionByTypeName(string featureType)
		{
			return new Control($"{ControlUtility.GetElementSelector($"mat-option-feature-type-{featureType.ToLower()}")}");
		}

		public static Control GetFeatureActionRemoveButtonByFeatureName(string featureName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"button-feature-actions-remove-{featureName.ToLower()}")}");
		}
		
		public static Control GetFeatureActionEditButtonByFeatureName(string featureName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"button-feature-actions-edit-{featureName.ToLower()}")}");
		}

		public static Control GetFeatureAutocompleteResultByFeatureName(string featureName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"mat-option-feature-{featureName.ToLower()}")}");
		}

		public static Control GetFeatureTableFeatureNameByRowNumber(int rowNumber)
		{
			return new Control($"[role=row]:nth-of-type({rowNumber}) {ControlUtility.GetElementSelector("td-feature-name")}");
		}

		public static Control GetFeatureTableFeatureTypeByRowNumber(int rowNumber)
		{
			return new Control($"[role=row]:nth-of-type({rowNumber}) {ControlUtility.GetElementSelector("td-feature-type")}");
		}

		public static Control GetFeatureTableFeatureDefinitionByRowNumber(int rowNumber)
		{
			return new Control($"[role=row]:nth-of-type({rowNumber}) {ControlUtility.GetElementSelector("td-feature-definition")}");
		}

		public static Control GetFeatureTableFeatureActionsByRowNumber(int rowNumber)
		{
			return new Control($"[role=row]:nth-of-type({rowNumber}) {ControlUtility.GetElementSelector("td-feature-actions")}");
		}

		public static Control GetEditFeatureTypeButtonByFeatureName(string featureName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"button-feature-actions-edit-category-feature-{featureName.ToLower()}")}");
		}

        public static Control GetCategoryTagFilterOptionByTagName(string tagName)
        {
            return new Control($"{ControlUtility.GetElementSelector($"mat-option-tag-{tagName.ToLower()}")}");
        }
		public static Control GetAssociatedTagsChipByCategoryName(string tagName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"mat-chip-{tagName.ToUpper()}")}");
		}

		public static Control GetRemoveAssociatedTagsChipByCategoryName(string tagName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"mat-icon-remove-chip-{tagName.ToUpper()}")}");
		}

		public static Control GetAssociatedTagsOptionByTagName(string tagName)
		{
			return new Control($"{ControlUtility.GetElementSelector($"mat-option-associated-tags-{tagName.ToUpper()}")}");
		}

		public static Control GetOptionTagTypeByName(string optionName)
		{
			return new Control(ControlUtility.GetElementSelector($"mat-option-tag-type-{optionName}"));
		}
		

		#endregion

		public static class ColumnNameSelector
		{
			public const string Name = "td-product-name";
			public const string CategoryStatus = "td-product-category-status";
			public const string DateAdded = "td-added-on";
			public const string AddedBy = "td-added-by";
			public const string DateChanged = "td-changed-on"; //need correction in ui
			public const string ChangedBy = "td-changed-by";
			public const string VerificationStatus = "td-verification-status";
		}
	}
}
