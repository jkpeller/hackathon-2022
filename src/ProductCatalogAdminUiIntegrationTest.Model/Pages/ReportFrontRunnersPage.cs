using ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Model.Pages
{
	public class ReportFrontRunnersPage : BasePage
	{
		#region Variables and constructor

		public ReportFrontRunnersPage()
		{
		}

		#endregion

		#region Single Elements

		public readonly Control TableHeaderVendorName = new Control(ControlUtility.GetElementSelector("th-vendor-name"));
		public readonly Control TableHeaderProductName = new Control(ControlUtility.GetElementSelector("th-product-name"));
		public readonly Control TableHeaderQualificationStatus = new Control(ControlUtility.GetElementSelector("th-qualification-status"));
		public readonly Control TableHeaderSoftwareAdviceId = new Control(ControlUtility.GetElementSelector("th-software-advice-id"));
		public readonly Control TableHeaderPublishedOnSA = new Control(ControlUtility.GetElementSelector("th-published-on-sa"));
		public readonly Control TableHeaderTotalScore = new Control(ControlUtility.GetElementSelector("th-total-score"));

		#endregion

		#region Method Controls

		public static Control GetProductLinkByRowNumber(int rowNumber)
		{
			return new Control($"{ControlUtility.GetTableRowByRowNumber(rowNumber)} {ControlUtility.GetElementSelector("product-website-url")}");
		}

		#endregion

		public static class ColumnNameString
		{
			public const string VendorName = "Vendor Name";
			public const string ProductName = "Product Name";
			public const string QualificationStatus = "Qualification Status";
			public const string SoftwareAdviceId = "Software Advice Id";
			public const string PublishedOnSA = "Published on SA?";
			public const string TotalScore = "Total Score";
		}

		public static class ColumnNameSelector
		{
			public const string VendorName = "td-vendor-name";
			public const string ProductName = "td-product-name";
			public const string QualificationStatus = "td-qualification-status";
			public const string SoftwareAdviceId = "td-software-advice-id";
			public const string PublishedOnSA = "td-published-on-sa";
			public const string TotalScore = "td-total-score";
		}
	}
}
