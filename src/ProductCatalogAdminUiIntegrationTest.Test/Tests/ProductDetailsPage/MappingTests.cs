using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	public class MappingTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private string _siteProductId;
		private List<SourceSiteProductSaveRequest> _sourceSiteProductSaveRequest;
		private string _sourceSiteProductName;

		public MappingTests() : base(nameof(MappingTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(9);
			_siteProductId = RequestUtility.GetUniqueId();
			_sourceSiteProductSaveRequest = new List<SourceSiteProductSaveRequest>{ RequestUtility.GetSourceSiteProductSaveRequest(_siteProductId) };
			_sourceSiteProductName = _sourceSiteProductSaveRequest.First().Name;
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_ValidateReadonlyState_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var productId = SetupTest(true, false).Item1;

				//Validate that the site search boxes do not appear in readonly state
				Assert.IsFalse(_productDetailsPage.InputSiteLevelSearchCapterra.IsDisplayed());
				Assert.IsFalse(_productDetailsPage.InputSiteLevelSearchSoftwareAdvice.IsDisplayed());
				Assert.IsFalse(_productDetailsPage.InputSiteLevelSearchGetapp.IsDisplayed());

				//Validate that a chip for the mapped site product is shown correctly, and there is no 'X' button to remove it
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).IsDisplayed());
				Assert.IsFalse(Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_ValidateEditableState_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var productId = SetupTest(true, true).Item1;

				//Validate that the save and cancel buttons are visible
				Assert.IsTrue(_productDetailsPage.ButtonSaveSiteProducts.IsDisplayed());
				Assert.IsTrue(_productDetailsPage.ButtonCancelEditSiteProducts.IsDisplayed());

				//Validate that a chip for the mapped site product is shown correctly and an 'X' button is present to remove it
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).IsDisplayed());
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Validate that the site search boxes appear
				Assert.IsTrue(_productDetailsPage.InputSiteLevelSearchCapterra.IsDisplayed());
				Assert.IsTrue(_productDetailsPage.InputSiteLevelSearchSoftwareAdvice.IsDisplayed());
				Assert.IsTrue(_productDetailsPage.InputSiteLevelSearchGetapp.IsDisplayed());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_AddSiteProductAndSaveThenRemove_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var setupResponse = SetupTest(false, true);
				var productId = setupResponse.Item1;
				var siteProductId = setupResponse.Item2;

				//Type the site product name into the capterra search box
				_productDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteProductName, sendEscape: false);
				Thread.Sleep(1000);
				Log($"Typed {_sourceSiteProductName} into the Capterra site products search box.");

				//Click on the suggested result
				_productDetailsPage.OptionFirstAutocompleteSuggestion.Click(false);
				Log("Clicked on the first autocomplete suggestion.");

				//Validate that a chip for the mapped site product is shown correctly
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Save the site products
				_productDetailsPage.ButtonSaveSiteProducts.ClickAndWaitForPageToLoad();
				Thread.Sleep(3000);
				Log("Clicked the save site products button.");

				//Click the edit site categories button
				_productDetailsPage.ButtonEditSiteProducts.Click();
				Log("Clicked the edit site products button.");
				Thread.Sleep(1000);

				//Click the 'X' button to remove the site product mapping
				Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).Click();
				Log($"Removed site product chip. Chip text: {_sourceSiteProductName.ToLower()}");
				Thread.Sleep(1000);

				//Validate that the site product chip no longer shows up
				Assert.IsFalse(Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Get the product from the Api and validate that the site product is still mapped
				var getResponseResult = ProductAdminApiService.GetProductById(productId.ToString(), true).Result;
				Assert.AreEqual(siteProductId, getResponseResult.SiteProducts.Single().SiteProductId);

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_AddSiteProductAndCancel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var productId = SetupTest(false, true).Item1;

				//Type the site products name into the capterra search box and click on the suggested result
				_productDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteProductName, sendEscape: false);
				Thread.Sleep(500);
				Log($"Typed {_sourceSiteProductName} into the Capterra site categories search box.");

				//Click on the suggested result
				_productDetailsPage.ButtonCancelEditSiteProducts.HoverOver();
				_productDetailsPage.OptionFirstAutocompleteSuggestion.Click();
				Log("Clicked on the first autocomplete suggestion.");

				//Cancel the edit site products
				_productDetailsPage.ButtonCancelEditSiteProducts.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the cancel edit site products button.");

				//Validate that the site product chip does not show up
				Assert.IsFalse(Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Get the product by Id from the Api and validate that the site product is not mapped
				var getResponseResult = ProductAdminApiService.GetProductById(productId.ToString(), true).Result;
				Assert.IsEmpty(getResponseResult.SiteProducts);

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_RemoveSiteProductAndSave_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var setupResponse = SetupTest(false, true);
				var productId = setupResponse.Item1;

				//Type the site product name into the capterra search box
				_productDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteProductName, sendEscape: false);
				Thread.Sleep(500);
				Log($"Typed {_sourceSiteProductName} into the Capterra site products search box.");

				//Click on the suggested result
				_productDetailsPage.OptionFirstAutocompleteSuggestion.Click(false);
				Log("Clicked on the first autocomplete suggestion.");

				//Validate that a chip for the mapped site product is shown correctly
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).IsDisplayed());

				//Save the site products
				_productDetailsPage.ButtonSaveSiteProducts.ClickAndWaitForPageToLoad();
				Thread.Sleep(3000);
				Log("Clicked the save site products button.");

				//Click the edit site categories button
				_productDetailsPage.ButtonEditSiteProducts.Click();
				Log("Clicked the edit site products button.");
				Thread.Sleep(1000);

				//Click the 'X' button to remove the site product mapping
				Model.Pages.ProductDetailsPage.GetSiteProductChipRemoveBySiteProductName(_sourceSiteProductName.ToLower()).Click();
				Log($"Removed site product chip. Chip text: {_sourceSiteProductName.ToLower()}");
				Thread.Sleep(1000);

				//Save the change
				_productDetailsPage.ButtonSaveSiteProducts.Click();
				Thread.Sleep(3000);

				//Assert that the chip doesn't show up
				Assert.IsTrue(_productDetailsPage.CapterraMessageNoSiteProducts.IsDisplayed());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		[Ignore("Invalid Test. Edit Site Product functionality was removed on GCT-1314")]
		public void GlobalProductMapping_ValidateSourceSiteProductId_Succeeds()
		{
			var siteProductDisplayText = $"{_sourceSiteProductName} ({_siteProductId})";
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var setupResponse = SetupTest(false, true);
				var productId = setupResponse.Item1;

				//Type the site product name into the capterra search box
				_productDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteProductName, sendEscape: false);
				Thread.Sleep(1000);
				Log($"Typed {_sourceSiteProductName} into the Capterra site products search box.");

				//Click on the suggested result
				_productDetailsPage.OptionFirstAutocompleteSuggestion.Click(false);
				Log("Clicked on the first autocomplete suggestion.");

				//Validate that the sourceSiteProductId is shown in parenthesis after the name
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).GetText().Contains(siteProductDisplayText));

				//Save the site products
				_productDetailsPage.ButtonSaveSiteProducts.ClickAndWaitForPageToLoad();
				Thread.Sleep(1000);
				Log("Clicked the save site products button.");

				//Validate that the sourceSiteProductId is shown in parenthesis after the name
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteProductName.ToLower()).GetText().Contains(siteProductDisplayText));

				//Cleanup
				IntegrationApiService.DeleteSourceSiteProduct(_siteProductId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		private Tuple<int, int> SetupTest(bool mapProductToSiteProduct, bool editSiteProducts)
		{
			//Open the page
			OpenPage(_productsPage);

			//Create a product, a site product, and map them together
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName }).Result.ProductId;
			Log($"Created product. ProductId: {productId}");

			var sourceSiteProductId = IntegrationApiService.PostAndGetSiteProducts(_sourceSiteProductSaveRequest).First();
			Log($"Created site product. SiteProductId: {_siteProductId}, SourceSiteProductId: {sourceSiteProductId}");

			if (mapProductToSiteProduct)
			{
				var postMapRequest = RequestUtility.GetSiteProductToProductMapSaveRequest(productId, sourceSiteProductId);
				ProductAdminApiService.PostSiteProductToProductMapping(postMapRequest);
				Log($"Map site product post succeeded. ProductId: {postMapRequest.ProductId}, SiteProductId: {postMapRequest.SiteProductId}.");
			}

			//Open the page for the newly-created listing request
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/products/{productId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(5000);

			if (!editSiteProducts) return Tuple.Create(productId, sourceSiteProductId);
			//Click the edit site products button
			_productDetailsPage.ButtonEditSiteProducts.ClickAndWaitForPageToLoad();
			Log("Clicked the edit site products button.");

			return Tuple.Create(productId, sourceSiteProductId);
		}
	}
}
