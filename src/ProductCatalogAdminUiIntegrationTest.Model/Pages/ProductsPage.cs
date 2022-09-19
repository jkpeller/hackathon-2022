using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class ProductsPage : BasePage
	{
		#region Variables and constructor

		private const string PageUrl = "/products";

		public ProductsPage()
		{
			Url = PageUrl;
		}

		#endregion

		#region Single Elements

		public readonly Control TableHeaderProductName = new Control(ControlUtility.GetElementSelector("th-product-name"));
		public readonly Control TableHeaderCapterra = new Control(ControlUtility.GetElementSelector("th-capterra-product-count"));
		public readonly Control TableHeaderSoftwareAdvice = new Control(ControlUtility.GetElementSelector("th-software-advice-product-count"));
		public readonly Control TableHeaderGetapp = new Control(ControlUtility.GetElementSelector("th-getapp-product-count"));
		public readonly Control TableHeaderTotalProductCount = new Control(ControlUtility.GetElementSelector("th-total-product-count"));

		public readonly Control SelectProductStatus = new Control(ControlUtility.GetElementSelector("mat-select-product-status"));

		public IEnumerable<string> TableBodyProductNames
		{
			get
			{
				return BrowserUtility.WebDriver
					.FindElements(By.CssSelector(ControlUtility.GetElementSelector("td-product-name")))
					.Select(c=>c.Text);
			}
		}
		
		#endregion

		#region Method Controls

		public static Control GetVendorLinkByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("link-vendor-name")}");
		}

		#endregion

		public static class ColumnNameString
		{
			public const string VendorName = "Vendor Name";
			public const string ProductName = "Product Name";
			public const string Status = "Status";
			public const string Updated = "Updated";
			public const string CapterraCount = "Capterra";
			public const string SoftwareAdviceCount = "Software Advice";
			public const string GetappCount = "GetApp";
			public const string TotalCount = "Total";
		}

		public static class ColumnNameSelector
		{
			public const string VendorName = "td-vendor-name";
			public const string Name = "td-product-name";
			public const string Status = "td-status";
			public const string Updated = "td-modified-on-utc";
			public const string Capterra = "td-capterra-product-count";
			public const string SoftwareAdvice = "td-software-advice-product-count";
			public const string Getapp = "td-getapp-product-count";
			public const string Total = "td-total-product-count";
		}
	}
}
