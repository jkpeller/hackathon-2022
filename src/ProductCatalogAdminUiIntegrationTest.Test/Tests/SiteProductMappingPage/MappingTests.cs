using NUnit.Framework;
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
	public class MappingTests : BaseTest
	{
		private Model.Pages.SiteProductMappingPage _page;
		private string _sourceSiteProductId;
		private SourceSiteProductSaveRequest _sourceSiteProductSaveRequest;
		private string _productName;

		public MappingTests() : base(nameof(MappingTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteProductMappingPage();
			_sourceSiteProductId = RequestUtility.GetUniqueId();
			_sourceSiteProductSaveRequest = RequestUtility.GetSourceSiteProductSaveRequest(_sourceSiteProductId);
			_productName = RequestUtility.GetRandomString(9).ToLower();
		}

		[Test]
		[Category("Product")]
		public void SiteProductAutocomplete_ByBelowMinimumSearchTextLength_Fails()
		{
			var searchText = RequestUtility.GetRandomString(2);
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Click the global product mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
				Log("Click the site product mapping button for the result record row.");

				//type 2 characters into the modal search box
				_page.InputMappingModalSearchText.SendKeys(searchText);
				Log($"Entered {searchText} into the product mapping modal search box.");

				//assert that no suggestion appears for the invalid search text
				Assert.IsFalse(Model.Pages.SiteProductMappingPage.GetModalGlobalProductSuggestionByName(_productName).IsDisplayed());
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductAutocomplete_ByValidSearchTextWithNoResults_Succeeds()
		{
			var searchText = RequestUtility.GetUniqueId();
			ExecuteTimedTest(() =>
			{
				//open the page and then the mapping modal for the 1st result record in the table
				OpenPageAndRowOneMappingModal();

				//type random text characters into the modal search box
				_page.InputMappingModalSearchText.SendKeys(searchText, sendEscape: false);
				Log($"Entered {searchText} into the product mapping modal search box.");

				//validate that the message appears that no product was found is displayed
				Assert.IsFalse(_page.HintNoMatchingProduct.IsDisplayed(false));
				});
		}

		[Test]
		[Category("Product")]
		public void SiteProductAutocomplete_ByValidSearchTextWithResults_Succeeds()
		{
			var searchText = _productName;
			var globalProduct1 = $"{_productName} a";
			var globalProduct2 = $"{_productName} b";

			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = globalProduct1 }).Result.ProductId.ToString();
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = globalProduct2 }).Result.ProductId.ToString();

				//Click the global product mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
				Log("Click the site product mapping button for the result record row.");

				//Type valid text characters into the modal search box
				_page.InputMappingModalSearchText.SendKeys(searchText, sendEscape: false);
				Thread.Sleep(1000);
				Log($"Entered {searchText} into the product mapping modal search box.");

				//Validate that the appropriate suggestions appear in ascending alphabetical order
				Assert.AreEqual(globalProduct1, BasePage.GetAutocompleteSuggestionByPosition(1).GetText());
				Assert.AreEqual(globalProduct2, BasePage.GetAutocompleteSuggestionByPosition(2).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductMapping_MappingModalOptions_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName }).Result.ProductId.ToString();
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
				var siteProductName = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId).Result.Name;

				//Select all publish and mapping status filter options and click apply filters
				FilterByAllFilters(_page, siteProductName);

				//Click the global product mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
				Log("Click the site product mapping button for the result record row.");

				//Type the name of a known global product into the search box
				_page.InputMappingModalSearchText.SendKeys(_productName);
				Log($"Entered {_productName} into the product mapping modal search box.");

				//Assert that the Map button is disabled and the Cancel button is enabled
				Assert.IsFalse(_page.ButtonMappingModalCancel.IsDisabled());
				Assert.IsFalse(_page.ButtonMappingModalMap.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductMapping_BySelectedGlobalProductSuggestion_Succeeds()
		{
			const string snackBarMessage = "Product mapped successfully";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName }).Result.ProductId.ToString();
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
				var siteProductName = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId).Result.Name;

				//select all publish and mapping status filter options and click apply filters
				FilterByAllFilters(_page, siteProductName);

				//Click the global product mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
				Log("Click the site product mapping button for the result record row.");

				//type the name of a known global product into the search box
				_page.InputMappingModalSearchText.SendKeys(_productName, sendEscape: false);
				Log($"Entered {_productName} into the product mapping modal search box.");

				//click on the suggested global product name
				Model.Pages.SiteProductMappingPage.GetModalGlobalProductSuggestionByName(_productName).Click(false);
				Log($"Clicked the suggested product name: {_productName}");

				//click on the map button
				_page.ButtonMappingModalMap.Click();
				Log("Clicked the map button");

				//assert that the snack bar container appears the correct text
				Assert.AreEqual(snackBarMessage, _page.SnackBarContainer.GetText(false));

				//verify that the global product column for the record now contains the name of the new global category
				Assert.AreEqual(_productName, Model.Pages.SiteProductMappingPage.GetTableGlobalProductDisplayByRow(1).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductMapping_BySelectedGlobalProductSuggestionThenCancel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName }).Result.ProductId.ToString();
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
				var siteProductName = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId).Result.Name;

				//Select all publish and mapping status filter options and click apply filters
				FilterByAllFilters(_page, siteProductName);

				//Click the global product mapping button for the first result record
				OpenPageAndRowOneMappingModal();

				//Type the name of a known global product into the search box
				_page.InputMappingModalSearchText.SendKeys(_productName, sendEscape: false);
				Log($"Entered {_productName} into the product mapping modal search box.");

				//Click on the suggested global product name
				Model.Pages.SiteProductMappingPage.GetModalGlobalProductSuggestionByName(_productName).Click(false);
				Log($"Clicked the suggested product name: {_productName}");

				//Click on the map button
				_page.ButtonMappingModalCancel.ClickAndWaitForPageToLoad();
				Log("Clicked the cancel button");

				//Verify that the global product column for the record is still empty
				AssertRowHasNoGlobalProduct(1);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductDeleteMapping_RemoveButtonFunctionality_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//Setup
				var setupResponse = CreateAndMapSiteProduct();
				var siteProductName = setupResponse.Item1.Result.Name;
				var dialogText = $"Are you sure you want to unmap product {siteProductName.Trim()} ({_sourceSiteProductId}) from global product {_productName}?";

				//Select all publish and mapping status filter options and click apply filters
				FilterByAllFilters(_page, siteProductName);

				//Click the delete mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
				Model.Pages.SiteProductMappingPage.GetSiteProductDeleteMappingButtonByRowNumber(1).Click();
				Log("Click the site product unmapping button for the result record row.");

				//Verify that the modal pops up and the message in it is correct
				Assert.AreEqual(_page.ConfirmationDialogText.GetText(), dialogText);
				Log($"Validated confirmation dialog text. Text: {_page.ConfirmationDialogText.GetText()}");

				//Cleanup
				ProductAdminApiService.DeleteProduct(setupResponse.Item2);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductDeleteMapping_ConfirmDeleteMapping_Succeeds()
		{
			const string snackBarMessage = "Product unmapped successfully";
			ExecuteTimedTest(() =>
			{
				//Setup the data and page
				var productId = SetupDataFilterTableAndOpenRemoveMappingModal();

				//Click the unmap button
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Thread.Sleep(1500);
				Log("Click the unmap button in the confirmation dialog.");

				//Validate that a snack bar message states that the product was unmapped successfully
				Assert.AreEqual(snackBarMessage, _page.SnackBarContainer.GetText());

				//Validate that the site product no longer displays a name in the global product column
				AssertRowHasNoGlobalProduct(1);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void SiteProductDeleteMapping_CancelDeleteMapping_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the data and page
				var productId = SetupDataFilterTableAndOpenRemoveMappingModal();

				//Click the cancel button
				_page.ButtonConfirmationDialogCancel.ClickAndWaitForPageToLoad();
				Log("Click the cancel button in the confirmation dialog.");

				//Validate that the site product continues to display the correct name in the global product column
				Assert.AreEqual(_productName, Model.Pages.SiteProductMappingPage.GetTableGlobalProductDisplayByRow(1).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		private void OpenPageAndRowOneMappingModal()
		{
			OpenPage(_page);

			//click the global product mapping button for the first result record
			BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
			Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(1).Click();
			Log("Click the site product mapping button for the result record row.");
		}

		private Tuple<ResponsePackage<SourceSiteProductDto>, string> CreateAndMapSiteProduct()
		{
			//Create a global product
			var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = _productName }).Result.ProductId;

			//Create a new site product
			IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);
			var getResponse = IntegrationApiService.GetSourceSiteProduct(_sourceSiteProductId);
			var siteProductId = getResponse.Result.ProductCatalogSiteProductId;

			//map the new site product to a known global product
			var mapSaveRequest = RequestUtility.GetSiteProductToProductMapSaveRequest(productId, siteProductId);
			ProductAdminApiService.PostSiteProductToProductMapping(mapSaveRequest);

			return Tuple.Create(getResponse, productId.ToString());
		}

		private string SetupDataFilterTableAndOpenRemoveMappingModal()
		{
			OpenPage(_page);

			var setupResponse = CreateAndMapSiteProduct();
			var siteProductName = setupResponse.Item1.Result.Name;

			//select all publish and mapping status filter options and click apply filters
			FilterByAllFilters(_page, siteProductName);

			//click the delete mapping button for the first result record
			BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
			Model.Pages.SiteProductMappingPage.GetSiteProductDeleteMappingButtonByRowNumber(1).ClickAndWaitForPageToLoad();
			Log("Click the site product unmapping button for the result record row.");

			return setupResponse.Item2;
		}

		private static void AssertRowHasNoGlobalProduct(int rowNumber)
		{
			BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).HoverOver();
			var addMappingButtonDisplayText = Model.Pages.SiteProductMappingPage.GetSiteProductMappingButtonByRowNumber(rowNumber).GetText();
			Assert.AreEqual("ADD MAPPING", addMappingButtonDisplayText.Substring(4, addMappingButtonDisplayText.Length - 4));
		}
	}
}
