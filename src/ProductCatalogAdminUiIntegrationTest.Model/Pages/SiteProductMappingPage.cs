using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	/// <inheritdoc />
	/// <summary>
	/// Controls and control methods for items on the Site Products Mapping page in the UI.
	/// </summary>
	public class SiteProductMappingPage : BasePage
	{
		#region Variables and constructor

		private const string PageUrl = "/site-products/mapping";

		public SiteProductMappingPage()
		{
			Url = PageUrl;
		}

		#endregion

		#region Single Elements

		//these are single, unique elements in the UI to be located in the DOM when they are used by calling methods in the Control class
		public readonly Control TableHeaderProductName = new Control(ControlUtility.GetElementSelector("th-site-product-name"));
		public readonly Control TableHeaderPublishStatus = new Control(ControlUtility.GetElementSelector("th-publish-status"));
		public readonly Control TableHeaderSite = new Control(ControlUtility.GetElementSelector("th-site-name"));
		public readonly Control TableHeaderUrl = new Control(ControlUtility.GetElementSelector("th-url"));
		public readonly Control TableHeaderGlobalProduct = new Control(ControlUtility.GetElementSelector("th-global-product-name"));

		public readonly Control InputMappingModalSearchText = new Control(ControlUtility.GetElementSelector("input-mapping-dialog-global-product-search"));
		public readonly Control HintNoMatchingProduct = new Control(ControlUtility.GetElementSelector("mat-hint-no-matches"));

		public readonly Control MappingDialogTitle = new Control(ControlUtility.GetElementSelector("mapping-dialog-title"));
		public readonly Control MappingDialogSiteName = new Control(ControlUtility.GetElementSelector("site-name"));
		public readonly Control MappingDialogSiteProductName = new Control(ControlUtility.GetElementSelector("site-product-name"));
		public readonly Control MappingDialogSourceSiteProductId = new Control(ControlUtility.GetElementSelector("source-site-product-id"));
		public readonly Control MappingDialogProductUrl = new Control(ControlUtility.GetElementSelector("site-product-website-url"));
		public readonly Control MappingDialogGlobalProductName = new Control(ControlUtility.GetElementSelector("global-product-name"));
		public readonly Control MappingDialogGlobalProductUrl = new Control(ControlUtility.GetElementSelector("global-product-website-url"));

		#endregion

		#region Method elements

		public static Control GetTableGlobalProductContainerByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("global-product-container")}");
		}

		public static Control GetSiteProductMappingButtonByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, ColumnNameSelector.GlobalProduct)} {ControlUtility.GetElementSelector("button-site-product-mapping")}");
		}

		public static Control GetModalGlobalProductSuggestionByName(string productName)
		{
			return new Control($"{ControlUtility.GetSiteProductsMappingModalSuggestionSelector(productName)}");
		}

		public static Control GetSiteProductDeleteMappingButtonByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableCellByRowNumberAndColumnName(rowNumber, ColumnNameSelector.GlobalProduct)} {ControlUtility.GetElementSelector("button-site-product-unmapping")}");
		}

		public static Control GetTableGlobalProductDisplayByRow(int rowNumber)
		{
			return new Control(ControlUtility.GetSiteProductsMappingTableGlobalProductDisplay(rowNumber));
		}

		public static string GetVendorUrlByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("a-admin-vendor-url")}").GetHref();
		}

		public static string GetProductUrlByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("a-admin-site-product-url")}").GetHref();
		}

		public static string GetSiteProductIdByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("a-admin-site-product-url")}").GetText();
		}

		public static string GetVendorIdByRow(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("a-admin-vendor-url")}").GetText();
		}

		#endregion

		public static class ColumnNameSelector
		{
			public const string Vendor = "td-site-vendor-name";
			public const string ProductName = "td-site-product-name";
			public const string PublishStatus = "td-publish-status";
			public const string Site = "td-site-name";
			public const string Url = "td-url";
			public const string GlobalProduct = "td-global-product-name";
		}

		public static class ColumnNameString
		{
			public const string Vendor = "Vendor (ID)";
			public const string ProductName = "Product (ID)";
			public const string PublishStatus = "Publish Status";
			public const string Site = "Site";
			public const string Url = "URL";
			public const string GlobalProduct = "Global Product";
		}
	}
}
