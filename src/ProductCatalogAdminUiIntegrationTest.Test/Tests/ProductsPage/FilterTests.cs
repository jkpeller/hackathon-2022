using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Request.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.DevTools.V85.Network;
using OpenQA.Selenium;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductsPage
{
	[TestFixture]
	[Category("Product")]
	public class FilterTests : BaseTest
	{
		private Model.Pages.ProductsPage _page;
		private string _productName;

		private const string NoResultsMessageText = "No results were returned";

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.ProductsPage();
			_productName = RequestUtility.GetRandomString(9);
		}

		[Test]
		public void FilterProducts_ByValidProductName_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup by creating a product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = _productName }).Result.ProductId;

				//Type a valid product name into the product name filter box
				_page.InputFilterProductName.SendKeys(_productName);
				Log($"{_productName} was typed into the product name filter box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Apply filters button was clicked.");

				//Assert that the returned product name equals the product created for this test
				Assert.AreEqual(_productName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Name).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void FilterProducts_ByAboveMaximumCharacters_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(101);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup by creating a product
				var saveProductName = productName.Substring(0, productName.Length - 1);
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = saveProductName }).Result.ProductId;
				BrowserUtility.Refresh();
				Thread.Sleep(5000);

				//Type more than what is allowed into the product name filter box
				_page.InputFilterProductName.SendKeys(productName);
				Log($"Typed {productName} into the product name filter box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Assert that the the product is returned because the search cut off the last character of the search
				Assert.AreEqual(saveProductName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Name).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Readonly")]
		[Ignore("For product search there is no minimum of characters allowed")]
		public void FilterProducts_ByBelowMinimumCharacters_Fails()
		{
			const string expectedProductNameErrorText = "You must type at least 3 characters";
			var searchText = RequestUtility.GetRandomString(2);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Type 2 characters into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText);       
				Log($"Typed {searchText} into the product name filter box.");
				_page.PageTitle.Click();
				Log("Clicked the page title.");

				//Assert that the error message is correctly displayed below the filter and that the apply filters button is disabled
				Assert.AreEqual(expectedProductNameErrorText, _page.ErrorMessageInputProductName.GetText());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}
		
		[Test]
		[Category("Readonly")]
		public void FilterProducts_ProductSearchAllowedAfterEnteringOneCharacter_Succeeds()
		{
			var searchText = RequestUtility.GetRandomString(1);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Type 1 character in the product name filter box
				_page.InputFilterProductName.SendKeys("U");	
				Log($"Typed one character in the product name filter box.");
				_page.InputFilterProductName.SendKeys(Keys.Tab);
				Log("Click tab key");

				//Assert that the apply filters button is displayed
				Assert.IsTrue(_page.ButtonApplyFilters.IsDisplayed());
			});
		}
		
		[Test]
		public void FilterProducts_ProductContainsMatchingStringForTextFilterGreaterThanTwo_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var keyName = RequestUtility.GetRandomString(9);
			var searchText = keyName.Substring(0, 3);
			
			var productName1 = $"{keyName}_{partialName}";
			var productName2 = $"A_{keyName}_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName1}).Result.ProductId.ToString();
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = productName2 }).Result.ProductId.ToString();
				Thread.Sleep(500);

				//Search for the created product
				_page.InputFilterProductName.SendKeys(searchText);
				Log($"Typed {keyName} into the product filter box.");
				Thread.Sleep(1000);

				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button");

				var productNames = _page.TableBodyProductNames.ToArray();
				CollectionAssert.IsNotEmpty(productNames);
				Assert.IsTrue(productNames.All(c => c.Contains(searchText)));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
			});
		}
		
		[Test]
		public void FilterProducts_ProductStartsWithMatchingStringForTextFilterLessOrEqualThanTwo_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var keyName = RequestUtility.GetRandomString(9);
			var searchText = keyName.Substring(0, 2);
			
			var productName1 = $"{keyName}_{partialName}";
			var productName2 = $"A_{keyName}_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

                //Setup
                var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName1 }).Result.ProductId.ToString();
                var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName2 }).Result.ProductId.ToString();
                Thread.Sleep(500);

                //Search for the created product
                _page.InputFilterProductName.SendKeys(searchText);
				Log($"Typed {searchText} into the product filter box.");
				Thread.Sleep(1000);

				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button");
				
				var productNames = _page.TableBodyProductNames.ToArray();
				CollectionAssert.IsNotEmpty(productNames);
				Assert.IsTrue(productNames.All(c => c.StartsWith(searchText, StringComparison.InvariantCultureIgnoreCase)));

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId1);
                ProductAdminApiService.DeleteProduct(productId2);
            });
		}

		[Test]
		public void AutocompleteProducts_ByValidProductName_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(9);
			var searchText = productName.Substring(0, productName.Length - 1);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName }).Result.ProductId.ToString();
				BrowserUtility.Refresh();

				//Type searchText into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText, sendEscape: false);	
				Log($"Typed {searchText} into the product name filter box.");
				Thread.Sleep(1000);

				//Assert that an autocomplete suggestion appears with the name of the product
				_page.ButtonApplyFilters.HoverOver();
				Assert.AreEqual(productName, BasePage.GetAutocompleteSuggestionByPosition(1).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void AutocompleteProducts_SelectSuggestion_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(9);
			var searchText = productName.Substring(0, productName.Length - 1);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = productName }).Result.ProductId.ToString();
				Thread.Sleep(500);

				//Type searchText into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText, sendEscape: false);	
				Log($"Typed {searchText} into the product name filter box.");
				Thread.Sleep(1000);

				//Click on the suggestion and then apply filters
				_page.ButtonApplyFilters.HoverOver();
				BasePage.GetAutocompleteSuggestionByPosition(1).Click();
				Log("Clicked on the autocomplete suggestion.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked on the apply filters button.");

				//Validate that the returned result is correct
				Assert.AreEqual(productName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Name).GetText());
				Log("Validate the returned result.");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Readonly")]
		public void SearchProduct_FilterWithNoResults_Succeeds()
		{
			var filterText = RequestUtility.GetRandomString(12);
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//type the filter text into the category name filter box and click the apply filters button
				_page.InputFilterProductName.SendKeys(filterText);
				Log($"Typed {filterText} into the vendor name filter");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button");

				//validate that the no results were returned message is displayed in the table
				Assert.AreEqual(NoResultsMessageText, _page.MessageTableNoResults.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		public void AutocompleteProducts_ByBelowMinimumCharacters_Fails()
		{
			var searchText = RequestUtility.GetRandomString(1);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Type 1 characters into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText);	
				Log($"Typed {searchText} into the product name filter box.");

				//Assert that the autocomplete box does not appear
				Assert.IsFalse(_page.OptionFirstAutocompleteSuggestion.IsDisplayed());
			});
		}
		
		[Test]
		[Category("Readonly")]
		public void AutocompleteProducts_AfterEnteringTwoCharacters_Succeeds()
		{
			var searchText = RequestUtility.GetRandomString(2);
			var partialName = RequestUtility.GetRandomString(9);
			var productName1 = $"{searchText}_{partialName}";

			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);
				
				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName1}).Result.ProductId.ToString();

				//Type 2 characters into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText, sendEscape: false);	
				Log($"Typed {searchText} into the product name filter box.");
				Thread.Sleep(1000);
				
				//Assert that the autocomplete box does not appear
				Assert.IsTrue(_page.OptionFirstAutocompleteSuggestion.IsDisplayed());
				
				ProductAdminApiService.DeleteProduct(productId1);
			});
		}
		
		[Test]
		public void AutocompleteProducts_ProductContainsMatchingStringForTextFilterGreaterThanTwo_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var keyName = RequestUtility.GetRandomString(9);
			var searchText = keyName.Substring(0, 3);
			
			var productName1 = $"{keyName}_{partialName}";
			var productName2 = $"A_{keyName}_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName1}).Result.ProductId.ToString();
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = productName2 }).Result.ProductId.ToString();
				Thread.Sleep(500);

				//Search for the created product
				_page.InputFilterProductName.SendKeys(searchText, sendEscape: false);
				_page.InputFilterProductName.HoverOver();
				Log($"Typed {searchText} into the product filter box.");
				Thread.Sleep(1000);

				var productNames = _page.AutocompleteSuggestionNames.ToArray();
				CollectionAssert.IsNotEmpty(productNames);
				Assert.IsTrue(productNames.All(c => c.Contains(searchText)));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
			});
		}
		
		[Test]
		public void AutocompleteProducts_ProductStartsWithMatchingStringForTextFilterLessOrEqualThanTwo_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var keyName = RequestUtility.GetRandomString(9);
			var searchText = keyName.Substring(0, 2);
			
			var productName1 = $"{keyName}_{partialName}";
			var productName2 = $"A_{keyName}_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName1 }).Result.ProductId.ToString();
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = productName2 }).Result.ProductId.ToString();
				Thread.Sleep(500);

				//Search for the created product
				_page.InputFilterProductName.SendKeys(searchText, sendEscape: false);
				Log($"Typed {searchText} into the product filter box.");
				Thread.Sleep(1000);
				
				var productNames = _page.AutocompleteSuggestionNames.ToArray();
				CollectionAssert.IsNotEmpty(productNames);
				Assert.IsTrue(productNames.All(c => c.ToLower().StartsWith(searchText.ToLower())));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
			});
		}

		[Test]
		public void FilterProducts_ByActiveStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = _productName }).Result.ProductId.ToString();
				BrowserUtility.Refresh();

				//Search for the created product
				_page.InputFilterProductName.SendKeys(_productName);
				Log($"Typed {_productName} into the product filter box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Validate that the product displays active in the status column
				Assert.AreEqual(nameof(CategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void FilterProducts_ByArchivedStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = _productName}).Result.ProductId.ToString();
				ProductAdminApiService.PutProduct(productId, new ProductUpdateRequest{ Name = _productName, ProductStatusId = (int)CategoryStatusType.Archived});
				Thread.Sleep(500);

				//Search for the created product
				_page.InputFilterProductName.SendKeys(_productName);
				Log($"Typed {_productName} into the product filter box.");
				Thread.Sleep(1000);

				//Filter only on the archived status filter
				_page.SelectProductStatus.Click();
				Log("Clicked into the status filter box.");
				_page.SelectStatusActive.Uncheck();
				Log("Deselected the active status value.");
				_page.SelectStatusArchived.Check();
				Log("Selected the archived status value.");
				_page.SelectStatusArchived.SendKeys();

				//Apply filters
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Validate that the product displays archived in the status column
				Assert.AreEqual(nameof(CategoryStatusType.Archived), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void FilterProducts_ByBothStatuses_Succeeds()
		{
			var partialName = RequestUtility.GetRandomString(9);
			var productName1 = $"Z_{partialName}";
			var productName2 = $"A_{partialName}";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName1}).Result.ProductId.ToString();
				ProductAdminApiService.PutProduct(productId1, new ProductUpdateRequest{ Name = productName1, ProductStatusId = (int)CategoryStatusType.Archived});
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest{ Name = productName2 }).Result.ProductId.ToString();
				Thread.Sleep(500);

				//Search for the created product
				_page.InputFilterProductName.SendKeys(partialName);
				Log($"Typed {partialName} into the product filter box.");
				Thread.Sleep(1000);

				//Filter on both status filter values
				_page.SelectProductStatus.Click();
				Log("Clicked into the status filter box.");
				_page.SelectStatusArchived.Check();
				Log("Selected the archived status value.");
				_page.SelectStatusArchived.SendKeys();

				//Apply filters
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Click to sort by the status column so we can guarantee the order
				_page.TableHeaderStatus.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the status column.");

				//Validate that both products display the correct status values
				Assert.AreEqual(nameof(CategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Status).GetText());
				Assert.AreEqual(nameof(CategoryStatusType.Archived), BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.ProductsPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
			});
		}

		[Test]
		[Category("Readonly")]
		public void FilterProducts_ByNoStatus_Fails()
		{
			const string expectedErrorText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Filter only neither status values
				_page.SelectProductStatus.Click();
				Log("Clicked into the status filter box.");
				_page.SelectStatusActive.Uncheck();
				Log("Deselected the active status value.");
				_page.SelectStatusArchived.SendKeys();

				//Validate that an error message displays with the correct text
				Assert.AreEqual(expectedErrorText, _page.ErrorMessageSelectProductStatus.GetText());
			});
		}

		[Test]
		public void FilterProducts_ByValidProductIntegrationId_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup by creating a product
				var product = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = _productName }).Result;

				//Navigate to the page for the new product using a valid productIntegrationId query parameter
				NavigateToProductsPageWithProductIntegrationId(product.ProductIntegrationId.ToString());

				//Assert only the single matching product is returned
				Assert.AreEqual(product.Name, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.ProductsPage.ColumnNameSelector.Name).GetText());
				Assert.IsNull(BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.ProductsPage.ColumnNameSelector.Name).GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
			});
		}

		[Test]
		[Category("Readonly")]
		public void FilterProducts_ByInvalidProductIntegrationId_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Navigate to the page using an invalid productIntegrationId query parameter
				NavigateToProductsPageWithProductIntegrationId(RequestUtility.GetRandomString(3));

				//validate that the no results were returned message is displayed in the table
				Assert.AreEqual(NoResultsMessageText, _page.MessageTableNoResults.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		public void FilterProducts_ByNonExistingProductIntegrationId_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Navigate to the page using a non existing productIntegrationId query parameter
				NavigateToProductsPageWithProductIntegrationId(Guid.NewGuid().ToString());

				//validate that the no results were returned message is displayed in the table
				Assert.AreEqual(NoResultsMessageText, _page.MessageTableNoResults.GetText());
			});
		}

		[Test]
		public void FilterProducts_ValidateVendorLink_Succeeds()
		{
			var vendorName = RequestUtility.GetRandomString(8);
			var vendorWebsiteUrl = $"https://{RequestUtility.GetRandomString(8)}.com";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Post vendor
				var vendor = PostVendor(vendorName, vendorWebsiteUrl);

				//Post product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest { Name = _productName, VendorId = int.Parse(vendor.VendorId)}).Result.ProductId;

				//Type a valid product name into the product name filter box
				_page.InputFilterProductName.SendKeys(_productName);
				Log($"{_productName} was typed into the product name filter box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Apply filters button was clicked.");

				//Assert that the returned product has correct vendor url on the vendor name column
				Assert.AreEqual($"{BrowserUtility.BaseUri}vendors/{vendor.VendorId}", Model.Pages.ProductsPage.GetVendorLinkByRowNumber(1).GetHref());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
			});
		}

		private static void NavigateToProductsPageWithProductIntegrationId(string productIntegrationId)
		{
			BrowserUtility.WebDriver.Navigate().GoToUrl($"{BrowserUtility.BaseUri}/products?productIntegrationId={productIntegrationId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(1000);
		}
	}
}