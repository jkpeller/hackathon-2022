using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductsPage
{
	[TestFixture]
	public class LayoutTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _sourceSiteProductId;
		private SourceSiteProductSaveRequest _sourceSiteProductSaveRequest;
		private string _sourceSiteProductName;
		private string _productName;
		private const string SourceSiteVendorId = "987654321";

		public LayoutTests() : base(nameof(LayoutTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_sourceSiteProductId = RequestUtility.GetUniqueId();
			_sourceSiteProductSaveRequest = RequestUtility.GetSourceSiteProductSaveRequest(_sourceSiteProductId, includeVendor: true, vendorId: SourceSiteVendorId);
			_sourceSiteProductName = _sourceSiteProductSaveRequest.Name;
			_productName = RequestUtility.GetRandomString(8);
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void ValidateLayout_PageTitleAndNavLink_Succeeds()
		{
			const string expectedText = "Products";
			ExecuteTimedTest(() =>
			{
				OpenPage(_productsPage);

				//Assert that the name of the page is correct
				Assert.AreEqual(expectedText, _productsPage.PageTitle.GetText());
				Log($"Validated the page title. {_productsPage.PageTitle.GetText()}");

				//Assert that the page name is correct in the nav bar
				Assert.IsTrue(_productsPage.LinkSideNavProducts.GetHref().Contains("/products"));
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void ValidateLayout_LocationOfVendorColumn_Succeeds()
		{
			const string expectedColumnName = "Vendor Name";
			ExecuteTimedTest(() =>
			{
				OpenPage(_productsPage);

				//Assert that the vendor column is the first one in the table
				Assert.AreEqual(expectedColumnName, BasePage.GetTableColumnByColumnNumber(1).GetText());
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_ValidateSiteProductLink_Succeeds()
		{
			var productLinkText = $"https://www.capmain.com/vendors/{SourceSiteVendorId}/products/{_sourceSiteProductId}/edit";
			ExecuteTimedTest(() =>
			{
				//Setup
				var productId = CreateAndMapSiteProduct();

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Assert that the link for the chip shown is of the correct format
				Assert.AreEqual(productLinkText, _productDetailsPage.GetSiteProductChipLinkBySiteProductName(_sourceSiteProductName));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProducts(new List<string>{ _sourceSiteProductSaveRequest.ProductId });
				Log("Cleanup completed.");
			});
		}

		private void FilterByProductName(BasePage page, string productName)
		{
			//type the name of the new site product in the product name filter box
			page.InputFilterProductName.SendKeys(productName);

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
			Thread.Sleep(3000);
		}

		private string CreateAndMapSiteProduct()
		{
			OpenPage(_productsPage);

			//Create a global product
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName, ProductWebsiteUrl = "http://www.test.com/"}).Result.ProductId;

			//Create a new site product
			IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
			var getResponse = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId);
			var siteProductId = getResponse.Result.ProductCatalogSiteProductId;

			//map the new site product to a known global product
			var mapSaveRequest = RequestUtility.GetSiteProductToProductMapSaveRequest(productId, siteProductId);
			ProductAdminApiService.PostSiteProductToProductMapping(mapSaveRequest);

			return productId.ToString();
		}
	}
}
