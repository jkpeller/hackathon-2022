using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	/// <inheritdoc />
	/// <summary>
	/// Controls and control methods for items on the Site Categories Mapping page in the UI.
	/// </summary>
	public class SiteCategoryMappingPage : BasePage
	{
		#region Variables and constructor

		private const string PageUrl = "/site-categories/mapping";

		public SiteCategoryMappingPage()
		{
			Url = PageUrl;
		}

		#endregion

		#region Single Elements

		//these are single, unique elements in the UI to be located in the DOM when they are used by calling methods in the Control class
		public readonly Control TableHeaderCategoryName = new Control(ControlUtility.GetElementSelector("th-category-name"));
		public readonly Control TableHeaderPublishStatus = new Control(ControlUtility.GetElementSelector("th-publish-status"));
		public readonly Control TableHeaderSite = new Control(ControlUtility.GetElementSelector("th-site-name"));
		public readonly Control TableHeaderUrl = new Control(ControlUtility.GetElementSelector("th-url"));
		public readonly Control TableHeaderGlobalCategory = new Control(ControlUtility.GetElementSelector("th-global-category-name"));

		public readonly Control InputMappingModalSearchText = new Control(ControlUtility.GetElementSelector("input-mapping-dialog-global-category-search"));
		public readonly Control SpanAddGlobalCategory = new Control(ControlUtility.GetElementSelector("add-global-category-cta"));

		public readonly Control TitleMappingDialog = new Control(ControlUtility.GetElementSelector("mapping-dialog-title"));
		public readonly Control DivMappingDialogSiteName = new Control(ControlUtility.GetElementSelector("site-name"));
		public readonly Control SpanMappingDialogCategoryName = new Control(ControlUtility.GetElementSelector("site-category-name"));
		public readonly Control SpanMappingDialogCategoryId = new Control(ControlUtility.GetElementSelector("source-site-category-id"));
		public readonly Control DivMappingDialogGlobalHeader = new Control(ControlUtility.GetElementSelector("global-upc"));
		public readonly Control SpanMappingDialogGlobalCategoryName = new Control(ControlUtility.GetElementSelector("global-category-name"));
		public readonly Control InputMappingDialogGlobalCategorySearch = new Control(ControlUtility.GetElementSelector("input-mapping-dialog-global-category-search"));

		#endregion

		#region Method Elements

		public static Control GetSiteCategoryMappingButtonByRowNumber(int rowNumber, string columnName = ColumnNameSelector.GlobalCategory)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, columnName)} .global-category-actions");
		}

		public static Control GetModalGlobalCategorySuggestionByName(string categoryName)
		{
			return new Control($"{ControlUtility.GetSiteCategoriesMappingModalSuggestionSelector(categoryName)}");
		}

		public static Control GetTableGlobalCategoryContainerByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("global-category-container")}");
		}

		public static Control GetTableGlobalCategoryDisplayByRow(int rowNumber)
		{
			return new Control(ControlUtility.GetSiteCategoriesMappingTableGlobalCategoryDisplay(rowNumber));
		}

		public static Control GetSiteCategoryDeleteMappingButtonByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, ColumnNameSelector.GlobalCategory)} {ControlUtility.GetElementSelector("button-site-category-unmapping")}");
		}

		#endregion

		public static class ColumnNameSelector
		{
			public const string Name = "td-category-name";
			public const string PublishStatus = "td-publish-status";
			public const string Site = "td-site-name";
			public const string Url = "td-url";
			public const string GlobalCategory = "td-global-category-name";
		}

		public static class ColumnNameString
		{
			public const string CategoryName = "Category Name (ID)";
			public const string PublishStatus = "Publish Status";
			public const string Site = "Site";
			public const string Url = "URL";
			public const string GlobalCategory = "Global Category";
		}
	}
}
