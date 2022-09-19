using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteProductMappingPage
{
	[TestFixture]
	[Ignore("Mapping capabilities were disabled on GCT-1315")]
	public class LayoutTests : BaseTest
	{
		private Model.Pages.SiteProductMappingPage _page;
		private string _sourceSiteProductId;
		private SourceSiteProductSaveRequest _sourceSiteProductSaveRequest;
		private string _sourceSiteProductName;
		private string _sourceSiteVendorId;
		private string _productName;
		private const string ProductWebsiteUrl = "http://www.test.com/";

		public LayoutTests() : base(nameof(LayoutTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteProductMappingPage();
			_sourceSiteProductId = RequestUtility.GetUniqueId();
			_sourceSiteVendorId = RequestUtility.GetUniqueId();
			_productName = RequestUtility.GetRandomString(9);
			_sourceSiteProductSaveRequest = RequestUtility.GetSourceSiteProductSaveRequest(_sourceSiteProductId, includeVendor: true, vendorId: _sourceSiteVendorId);
			_sourceSiteProductName = _sourceSiteProductSaveRequest.Name;
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void ValidateLayout_PageTitleAndNavLink_Succeeds()
		{
			const string expectedTitle = "Product Mapping";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Assert that the name of the page is correct
				Assert.AreEqual(expectedTitle, _page.PageTitle.GetText());
				Log($"Validated the page title. {_page.PageTitle.GetText()}");

				//Assert that the page name is correct in the nav bar
				Assert.IsTrue(_page.LinkSideNavProductMapping.GetHref().Contains("/site-products"));
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_ValidateMappingDialogLayout_Succeeds()
		{
			const string expectedDialogTitle = "Assign a Global Product";
			const string expectedSaveButtonText = "SAVE MAPPING";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				var setupResponse = CreateProductAndSiteProduct();
				var productId = setupResponse.Item2;

				FilterByAllFilters(_sourceSiteProductName);

				//Click the global product mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
				Log("Click the site product mapping button for the result record row.");

				//Assert that the save mapping button is disabled
				Assert.IsFalse(_page.ButtonMappingModalMap.IsDisplayed());

				//Assert the column 1 items in the dialog
				Assert.AreEqual(expectedDialogTitle, _page.MappingDialogTitle.GetText());
				Log($"Validated dialog title text. {_page.MappingDialogTitle.GetText()}");
				Assert.AreEqual(nameof(SiteType.Capterra), _page.MappingDialogSiteName.GetText());
				Log($"Validated dialog site name text. {_page.MappingDialogSiteName.GetText()}");
				Assert.AreEqual(_sourceSiteProductName, _page.MappingDialogSiteProductName.GetText());
				Log($"Validated dialog site product name text. {_page.MappingDialogSiteProductName.GetText()}");
				Assert.AreEqual($"({_sourceSiteProductId})", _page.MappingDialogSourceSiteProductId.GetText());
				Log($"Validated dialog site product id text. {_page.MappingDialogSourceSiteProductId.GetText()}");
				Assert.AreEqual($"{_sourceSiteProductSaveRequest.ProductWebsiteUrl}", _page.MappingDialogProductUrl.GetText());
				Log($"Validated dialog site product url text. {_page.MappingDialogProductUrl.GetText()}");

				//Type the name of the new product in the search box and click on it
				_page.InputMappingModalSearchText.SendKeys(_productName, sendEscape: false);
				Thread.Sleep(500);
				Log($"Typed {_productName} into the modal search box.");
				Model.Pages.SiteProductMappingPage.GetModalGlobalProductSuggestionByName(_productName.ToLower()).Click(false);
				Log($"Clicked the suggested product name: {_productName}");

				//Assert the column 2 items in the dialog
				Assert.AreEqual(_productName, _page.MappingDialogGlobalProductName.GetText());
				Log($"Validated dialog global product name text. {_page.MappingDialogGlobalProductName.GetText()}");
				Assert.AreEqual(ProductWebsiteUrl, _page.MappingDialogGlobalProductUrl.GetText());
				Log($"Validated dialog global product url text. {_page.MappingDialogGlobalProductUrl.GetText()}");

				//Assert the text of the save mapping button and that it is enabled
				Assert.IsFalse(_page.ButtonMappingModalMap.IsDisabled());
				Assert.AreEqual(expectedSaveButtonText, _page.ButtonMappingModalMap.GetText());
				Log($"Validated dialog save mapping button text. {_page.ButtonMappingModalMap.GetText()}");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_SourceSiteProductIdInProductColumn_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				FilterByProductNameAndAllFilters(_page, _sourceSiteProductName);
				Log("Opened the page and filtered on the new product.");

				//Assert that the returned record contains the correct name and id
				Assert.IsTrue(Model.Pages.SiteProductMappingPage.GetVendorIdByRow(1).Contains(_sourceSiteVendorId));

				//cleanup the site product 
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_SourceSiteVendorIdInVendorColumn_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				FilterByProductNameAndAllFilters(_page, _sourceSiteProductName);
				Log("Opened the page and filtered on the new product.");

				//Assert that the returned record contains the correct id


				//Cleanup the site product 
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void ValidateLayout_LocationOfColumns_Succeeds()
		{			
			const string expectedVendorColumnName = "Vendor (ID)";
			const string expectedProductColumnName = "Product (ID)";
			const string expectedGlobalProductColumnName = "Global Product";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Assert that the Vendor, Product, and Global Product columns are in the correct places
				Assert.AreEqual(expectedVendorColumnName, BasePage.GetTableColumnByColumnNumber(1).GetText());
				Assert.AreEqual(expectedProductColumnName, BasePage.GetTableColumnByColumnNumber(2).GetText());
				Assert.AreEqual(expectedGlobalProductColumnName, BasePage.GetTableColumnByColumnNumber(3).GetText());
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_ValidateVendorLink_Succeeds()
		{
			var vendorLinkText = $"https://www.capmain.com/vendors/{_sourceSiteVendorId}/edit";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				FilterByProductNameAndAllFilters(_page, _sourceSiteProductName);
				Log("Opened the page and filtered on the new product.");

				//Assert that the returned record contains the correct vendor link
				Assert.AreEqual(vendorLinkText, Model.Pages.SiteProductMappingPage.GetVendorUrlByRow(1));

				//Cleanup the site product 
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void ValidateLayout_ValidateProductLink_Succeeds()
		{
			var productLinkText = $"https://www.capmain.com/vendors/{_sourceSiteVendorId}/products/{_sourceSiteProductId}/edit";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				FilterByProductNameAndAllFilters(_page, _sourceSiteProductName);
				Log("Opened the page and filtered on the new product.");

				//Assert that the returned record contains the correct vendor link
				Assert.AreEqual(productLinkText, Model.Pages.SiteProductMappingPage.GetProductUrlByRow(1));

				//Cleanup the site product 
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		private void FilterByProductNameAndAllFilters(BasePage page, string sourceSiteProductName)
		{
			//type the name of the new site product in the product name filter box
			page.InputFilterProductName.SendKeys(sourceSiteProductName);

			//select all options from the mapping status filter box
			page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened.");
			page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			page.SelectOptionMappingStatusMapped.SendKeys(hoverOver: false);

			//select all options from the publish status filter box
			page.SelectPublishStatus.Click();
			Log("Publish status filter drop down clicked.");
			page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			page.SelectOptionPublishStatusUnpublished.SendKeys(hoverOver: false);
			Thread.Sleep(1000);

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
			BrowserUtility.WaitForPageToLoad();
		}

		private Tuple<ResponsePackage<SourceSiteProductDto>, string> CreateProductAndSiteProduct()
		{
			//Create a global product
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName, ProductWebsiteUrl = ProductWebsiteUrl }).Result.ProductId;

			//Create a new site product
			IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
			var getResponse = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId);

			return Tuple.Create(getResponse, productId.ToString());
		}

		private void FilterByAllFilters(string siteProductName)
		{
			//Filter on the site product name
			_page.InputFilterProductName.SendKeys(siteProductName);

			//select all options from the mapping status filter box
			_page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened.");
			_page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			_page.SelectOptionMappingStatusMapped.SendKeys();

			//select all options from the publish status filter box
			_page.SelectPublishStatus.Click();
			Log("Publish status filter drop down clicked.");
			_page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			_page.SelectOptionPublishStatusUnpublished.SendKeys();

			//click the Apply Filters button and wait for the page to load
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
		}
	}
}