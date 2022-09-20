using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class CategoriesPage : BasePage
	{
		#region Variables and constructor

		private const string PageUrl = "";

		public CategoriesPage()
		{
			Url = PageUrl;
		}

		#endregion

		#region Single Controls

		public readonly Control TableHeaderCategoryName = new Control(ControlUtility.GetElementSelector("th-category-name"));
        public readonly Control TableHeaderProductCount = new Control(ControlUtility.GetElementSelector("th-product-count"));
        public readonly Control TableHeaderFeatureCount = new Control(ControlUtility.GetElementSelector("th-feature-count"));
		public readonly Control TableHeaderCapterra = new Control(ControlUtility.GetElementSelector("th-capterra-category-count"));
		public readonly Control TableHeaderSoftwareAdvice = new Control(ControlUtility.GetElementSelector("th-software-advice-category-count"));
		public readonly Control TableHeaderGetapp = new Control(ControlUtility.GetElementSelector("th-getapp-category-count"));
		public readonly Control TableHeaderTotalCategoryCount = new Control(ControlUtility.GetElementSelector("th-total-category-count"));

		public readonly Control SelectCategoryStatus = new Control(ControlUtility.GetElementSelector("mat-select-category-status"));

		#endregion

		#region Control Methods

		public static Control GetLinkCategoryNameByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("link-category-name")}");
		}

		public static Control GetButtonArchiveCategoryByRowNumber(int rowNumber, string categoryName)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, ColumnNameSelector.CategoryActions)} {ControlUtility.GetElementSelector($"button-category-actions-{categoryName.ToLower()}")}");
		}

		public static Control GetButtonUnarchiveCategoryByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, ColumnNameSelector.CategoryActions)} {ControlUtility.GetElementSelector("button-category-unarchiving")}");
		}

		public static Control GetCategoryUpdatedByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("td-modified-on-utc")}");
		}

        public static int GetRowCountByCategoryTagsColumn()
        {
            var count = BrowserUtility.WebDriver.FindElements(By.CssSelector("[data-qa=\"td-category-tags\"]")).Count;
            return count;
        }

		#endregion

		public static class ColumnNameSelector
		{
			public const string Name = "td-category-name";
			public const string Status = "td-status";
			public const string ModifiedOnUtc = "td-modified-on-utc";
            public const string ProductCount = "td-product-count";
            public const string FeatureCount = "td-feature-count";
			public const string CategoryTags = "td-category-tags";
			public const string Capterra = "td-capterra-category-count";
			public const string SoftwareAdvice = "td-software-advice-category-count";
			public const string Getapp = "td-getapp-category-count";
			public const string Total = "td-total-category-count";
			public const string CategoryActions = "td-category-product-actions";
		}
	}
}