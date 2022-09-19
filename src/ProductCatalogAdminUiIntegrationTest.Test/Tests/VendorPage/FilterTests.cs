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
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	public class FilterTests : BaseTest
	{
		private Model.Pages.VendorPage _page;
		private readonly string _vendorName;
		private readonly string _vendorWebsiteUrl;
		private const string ArchivedStatus = "Archived";
		private readonly Model.Pages.ProductAddPage _productAddPage;

		public FilterTests() : base(nameof(FilterTests))
		{
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
			_productAddPage = new Model.Pages.ProductAddPage();
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.VendorPage();
		}

		[Test]
		[Category("Vendor")]
		public void AutocompleteVendor_ByValidName_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				OpenPage(_page);
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Search for the vendor name
				_page.InputVendorName.SendKeys(_vendorName, sendEscape: false);
				Log($"Entered {_vendorName} into the vendor name search box.");
				Thread.Sleep(1000);

				//Assert that an autocomplete suggestion appears with the name of the vendor
				Assert.AreEqual(_vendorName, _page.OptionFirstAutocompleteSuggestion.GetText());
				Log("Validated the autocomplete suggestion.");

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AutocompleteVendor_SelectSuggestion_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				OpenPage(_page);
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Search for the vendor name
				_page.InputVendorName.SendKeys(_vendorName, sendEscape: false);
				Log($"Entered {_vendorName} into the vendor name search box.");
				Thread.Sleep(1000);

				//Click the suggestion
				_page.OptionFirstAutocompleteSuggestion.Click();
				Log("Clicked on the first autocomplete suggestion.");

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ByValidName_Succeeds()
		{
			const string expectedProductCount = "0";
			const string expectedCategoryCount = "0";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Search for the vendor name
				_page.InputVendorName.SendKeys(_vendorName, true);
				Log($"Entered {_vendorName} into the vendor name search box.");
				Thread.Sleep(1000);

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked on the apply filters button.");
				Thread.Sleep(5000);

				//Assert that the information in row 1 is correct for the searched-for vendor
				Assert.AreEqual(_vendorName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false));
				Log($"Evaluated the text in the Name column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false)}");

				Assert.AreEqual(expectedProductCount, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Products).GetText(false));
				Log($"Evaluated the text in the Products column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Products).GetText(false)}");

				Assert.AreEqual(expectedCategoryCount, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Categories).GetText(false));
				Log($"Evaluated the text in the Categories column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Categories).GetText(false)}");

				Assert.AreEqual(_vendorWebsiteUrl, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Website).GetText(false));
				Log($"Evaluated the text in the Website column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Website).GetText(false)}");

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ByValidSingleCountry_Succeeds()
		{
			var vendorName1 = $"1{_vendorName}";
			var vendorUrl1 = $"http://{vendorName1}.com";
			var vendorName2 = $"2{_vendorName}";
			var vendorUrl2 = $"http://{vendorName2}.com";
			const string countryName = "United States of America";
			ExecuteTimedTest(() =>
			{
				//Open the page and setup new vendors
				OpenPage(_page);
				var vendor1 = PostVendor(vendorName1, vendorUrl1);
				var vendor2 = PostVendor(vendorName2, vendorUrl2);

				//Search for the vendor name
				_page.InputVendorName.SendKeys(_vendorName);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Search for the country value for both countries
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(countryName, sendEscape: false);
				Log($"Typed {countryName} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(countryName).Click(false);
				Log($"Clicked on the option for {countryName}.");
				_page.InputCountrySearch.SendKeys();

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				BrowserUtility.WaitForPageToLoad();
				Log("Clicked on the apply filters button.");

				//Assert that both vendors are returned
				var row1DisplayedVendorName = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false);
				var row2DisplayedVendorName = BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false);
				Assert.AreEqual(vendorName1, row1DisplayedVendorName);
				Log($"Evaluated the text in the Name column  for row 1. Text: {row1DisplayedVendorName}");
				Assert.AreEqual(vendorName2, row2DisplayedVendorName);
				Log($"Evaluated the text in the Name column  for row 2. Text: {row2DisplayedVendorName}");


				//Cleanup the vendor
				DeleteVendor(vendor1.VendorId);
				DeleteVendor(vendor2.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ByValidCountries_Succeeds()
		{
			var vendorName1 = $"1{_vendorName}";
			var vendorUrl1 = $"http://{vendorName1}.com";
			var vendorName2 = $"2{_vendorName}";
			var vendorUrl2 = $"http://{vendorName2}.com";
			const string countryName1 = "United States of America";
			const string countryName2 = "Afghanistan";
			ExecuteTimedTest(() =>
			{
				//Open the page and setup new vendors
				OpenPage(_page);
				var vendor1 = PostVendor(vendorName1, vendorUrl1);
				var vendor2 =  VendorAdminApiService.PostVendor(new VendorInsertRequest{ Name = vendorName2, WebsiteUrl = vendorUrl2,
					Address = new AddressRequest{ CountryId = 1 } });

				//Search for the vendor name
				_page.InputVendorName.SendKeys(_vendorName);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Search for the country value for both countries
				_page.SelectVendorCountry.Click();
				Log("Clicked into the select Country box.");
				_page.InputCountrySearch.SendKeys(countryName1, true, false);
				Log($"Typed {countryName1} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(countryName1).Click(false);
				Log($"Clicked on the option for {countryName1}.");
				_page.InputCountrySearch.SendKeys(countryName2,true, false);
				Log($"Typed {countryName2} into the Country autocomplete box.");
				BasePage.GetCountryOptionByName(countryName2).Click(false);
				Log($"Clicked on the option for {countryName1}.");
				_page.InputCountrySearch.SendKeys();

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				BrowserUtility.WaitForPageToLoad();
				Log("Clicked on the apply filters button.");

				//Assert that both vendors are returned
				var row1DisplayedVendorName = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false);
				var row2DisplayedVendorName = BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false);
				Assert.AreEqual(vendorName1, row1DisplayedVendorName);
				Log($"Evaluated the text in the Name column  for row 1. Text: {row1DisplayedVendorName}");
				Assert.AreEqual(vendorName2, row2DisplayedVendorName);
				Log($"Evaluated the text in the Name column  for row 2. Text: {row2DisplayedVendorName}");


				//Cleanup the vendor
				DeleteVendor(vendor1.VendorId);
				DeleteVendor(vendor2.Result.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void AutocompleteVendor_ByBelowMinimumCharacters_Fails()
		{
			var searchText = _vendorName.Substring(0, 2);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Search for a vendor name using less than the required number of characters
				_page.InputVendorName.SendKeys(searchText, sendEscape: false);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Assert that the autocomplete box does not appear
				Assert.IsFalse(_page.OptionFirstAutocompleteSuggestion.IsDisplayed());
				Log("Validated the autocomplete suggestion.");

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ByBelowMinimumName_Fails()
		{
			var searchText = _vendorName.Substring(0, 2);
			const string expectedErrorMessage = "You must type at least 3 characters";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Search for a vendor name using less than the required number of characters
				_page.InputVendorName.SendKeys(searchText);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Click the page title to give the message time to load
				_page.PageTitle.ClickAndWaitForPageToLoad();
				Log("Clicked the page title.");

				//Assert that the correct error message is displayed
				Assert.AreEqual(expectedErrorMessage, _page.ErrorMessageMinimumVendorName.GetText());
				Log($"Evaluated the error message. Text: {_page.ErrorMessageMinimumVendorName.GetText()}");

				//Cleanup the vendor
				DeleteVendor(vendor.VendorId);
			});
		}

		//TODO: when we can create a vendor, add a test that creates vendor with > 100-char name and search for 1st 100 chars of it.

		[Test]
		[Category("Vendor")]
		public void SearchVendor_FindCategoryFromFilter_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var category = SetupDataAndPage(c => { });

				//Assert that new category appears in the list
				Assert.AreEqual(category.Item2, Model.Pages.VendorPage.GetCategoryFromFilterByName(category.Item2).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(category.Item1);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ActiveCategoryFromFilter_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var category = SetupDataAndPage(c => { });

				//Assert that new category appears and does not say "Archived" which means that it is Active.
				Assert.That(Model.Pages.VendorPage.GetCategoryFromFilterByName(category.Item2).GetText(), !Contains.Substring($"({ArchivedStatus})").IgnoreCase);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(category.Item1);
			});
		}

		[Test]
		[Category("Vendor")]
		public void SearchVendor_ArchivedCategoryFromFilter_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var category = SetupDataAndPage(c =>
				{
					//Update the category to be archived
					CategoryAdminApiService.PutCategory(c.Item1, new CategoryUpdateRequest { CategoryStatusId = (int)CategoryStatusType.Archived, Name = c.Item2 });
				});

				//Assert that new category appears in the list with the right format and contains "Archived"
				Assert.That(Model.Pages.VendorPage.GetCategoryFromFilterByName(category.Item2).GetText(), Contains.Substring($"({ArchivedStatus})").IgnoreCase);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(category.Item1);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Vendor")]
		public void SearchVendor_NoCategorySelectedFromFilter_Succeeds()
		{
			const string categoryFilterDefaultText = "Category";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//category filter drop down doesn't have any value selected. Default text of the component is "Category"
				Assert.AreEqual(_page.SelectCategory.GetText(), categoryFilterDefaultText);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Vendor")]
		public void SearchVendor_FilterWithNoResults_Succeeds()
		{
			const string noResultsMessageText = "No results were returned";
			var filterText = RequestUtility.GetRandomString(12);
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//type the filter text into the category name filter box and click the apply filters button
				_page.InputVendorName.SendKeys(filterText);
				Log($"Typed {filterText} into the vendor name filter");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button");

				//validate that the no results were returned message is displayed in the table
				Assert.AreEqual(noResultsMessageText, _page.MessageTableNoResults.GetText());
			});
		}

		[Test]
		public void VendorDetailsPage_FilterVendorByActiveStatus_Succeeds()
		{
			string vendorStatus = "Active";
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//Update vendor status to active
				UpdateVendorStatus(vendor.VendorId, vendorStatus);

				//Search for a vendor by name
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName);
				_page.InputVendorName.SendKeys(_vendorName, true);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Search for a vendor by vendor status
				_page.SelectVendorStatus.Click();
				_page.VendorArchivedStatus.Click();
				_page.VendorActiveStatus.SendKeys(sendEscape: true);
				Log($"Filter vendor by active status");

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Thread.Sleep(1500);
				Log("Clicked on the apply filters button");

				//Assert the vendor name on the vendor list result
				Assert.AreEqual(_vendorName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false));
				Log($"Evaluated the text in the Name column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false)}");

				//Assert the vendor status on the vendor list result
				Assert.AreEqual(vendorStatus, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
				Log($"Evaluated the text in the Status column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

				//Cleanup 
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void VendorDetailsPage_SortVendorByVendorStatus_Succeeds()
		{
			string activeVendorStatus = "Active";
			string archivedVendorStatus = "Archived";
			var _vendorName2 = RequestUtility.GetRandomString(9);
			var product1Name = RequestUtility.GetRandomString(10);
			var product2Name = RequestUtility.GetRandomString(10);

			var product1WebsiteUrl = $"https://www.{_vendorName}.com/{product1Name}";
			var product2WebsiteUrl = $"https://www.{_vendorName}.com/{product2Name}";
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				var vendor2 = PostVendor(_vendorName2, _vendorWebsiteUrl);

				//Add a new product to vendor1
				OpenAddProductScreen(vendor.VendorId, true, product1Name, product1WebsiteUrl);

				//Add a new product to vendor2
				OpenAddProductScreen(vendor2.VendorId, true, product2Name, product2WebsiteUrl);

				//create a category
				var (categoryId, categoryName) = CreateCategory();

				//Get product id for vendor1
				var product1Id = VendorAdminApiService.GetProductsByVendorId(vendor.VendorId);
				// Add a category to product1
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = product1Id.Result.Single().ProductId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {product1Name}.");

				//Get the product id for vendor2
				var product2Id = VendorAdminApiService.GetProductsByVendorId(vendor2.VendorId);
				// Add a category to product2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = product2Id.Result.Single().ProductId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {product2Name}.");

				//Update vendor1 status to active
				UpdateVendorStatus(vendor.VendorId, activeVendorStatus);

				//Update vendor2 status to archived
				UpdateVendorStatus(vendor2.VendorId, archivedVendorStatus);

				//Filter vendors by category category name
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName);
				_page.SelectCategory.Click();
				_page.InputCategorySearch.SendKeys(categoryName, true, false);
				_page.InputCategorySearch.SendKeys(Keys.Enter);
				_page.InputCategorySearch.SendKeys(Keys.Tab);
				Log($"Entered {categoryName} in the category input search box.");

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Thread.Sleep(1000);
				Log("Clicked on the apply filters button");

				//Click on the status column to sort by vendor status  
				_page.TableHeaderStatus.ClickAndWaitPageToLoadAndOverlayToDisappear();
				_page.TableHeaderStatus.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked on the status column.");

				//Assert that the vendor with archived status is listed on row 1				
				Assert.AreEqual("Archived", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
				Log($"Evaluated the text in the Name column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

				//Assert that the vendor with active status is listed on row 2
				Assert.AreEqual("Active", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
				Log($"Evaluated the text in the Status column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

				//Cleanup 
				ProductAdminApiService.DeleteProduct(product1Id.Result.Single().ProductId.ToString());
				ProductAdminApiService.DeleteProduct(product2Id.Result.Single().ProductId.ToString());
				DeleteVendor(vendor.VendorId);
				DeleteVendor(vendor2.VendorId);
			});
		}
		[Test]
		public void VendorDetailsPage_FilterVendorByArchivedStatus_Succeeds()
		{
			string vendorStatus = "Archived";
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendor.VendorId);

				//Update vendor status to archived
				UpdateVendorStatus(vendor.VendorId, vendorStatus);

				//Search for a vendor by name
				BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName);
				_page.InputVendorName.SendKeys(_vendorName, true);
				Log($"Entered {_vendorName} into the vendor name search box.");

				//Search for a vendor by vendor status
				_page.SelectVendorStatus.Click();
				_page.VendorActiveStatus.Click();
				_page.VendorActiveStatus.SendKeys(sendEscape: true);
				Log($"Filter vendor by archived status");

				//Click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Thread.Sleep(1500);
				Log("Clicked on the apply filters button");

				//Assert the vendor name on the vendor list result
				Assert.AreEqual(_vendorName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false));
				Log($"Evaluated the text in the Name column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).GetText(false)}");

				//Assert the vendor status on the vendor list result
				Assert.AreEqual(vendorStatus, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
				Log($"Evaluated the text in the Status column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

				//Cleanup 
				DeleteVendor(vendor.VendorId);
			});
		}

		//TODO: when we can create a vendor and product-category association, add a test that creates vendor and associated it with a product for specific category to tes the category filter

		private Tuple<string, string> SetupDataAndPage(Action<Tuple<string, string>> assertionAction)
		{
			//Open the page
			OpenPage(_page);

			//Setup a global category
			var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
			var categoryId = categoryPostResponse.Result.CategoryId;
			var categoryName = categoryPostResponse.Result.Name;

			assertionAction(Tuple.Create(categoryId.ToString(), categoryName));

			BrowserUtility.Refresh();
			BrowserUtility.WaitForPageToLoad();
			Thread.Sleep(5000);

			//Open category filter list
			_page.SelectCategory.Click(false);
			Log("Clicked into the category filter box.");

			return Tuple.Create(categoryId.ToString(), categoryName);
		}

		private void OpenAddProductScreen(string vendorId, bool createProduct = false, string productName = null, string productWebsiteUrl = null)
		{
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId, 3000);

			//Click to edit the vendor
			_page.ButtonAddVendorProduct.ClickAndWaitForPageToLoad();
			Log("Clicked the add product button.");

			if (createProduct)
			{
				//Input the name of the product
				_productAddPage.InputProductName.SendKeys(productName, true);
				Log($"Typed {productName} into the product name input field.");
				Thread.Sleep(3000);

				//Input product website url
				_productAddPage.InputProductWebsite.SendKeys(productWebsiteUrl, true);
				Log($"Typed {productWebsiteUrl} into the product website url input field.");

				//Save the product
				_productAddPage.ButtonSubmitProductForm.ClickAndWaitForPageToLoad();
				Log("Clicked the create product button.");
			}
		}

		private void OpenAddProductScreen(string vendorId)
		{
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId, 3000);

			//Click to edit the vendor
			_page.ButtonAddVendorProduct.ClickAndWaitForPageToLoad();
			Log("Clicked the add product button.");
		}

		private void UpdateVendorStatus(string vendorId, string vendorStatus)
		{
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId);
			_page.ButtonEditVendor.Click();
			_page.DetailsVendorSelectVendorStatus.Click();

			if (vendorStatus.Equals("Active"))
			{
				_page.VendorActiveStatus.Click();
				_page.ButtonSubmitVendorForm.Click();
				Log($"Vendor status was changed to Active");
			}
			else
			{
				_page.VendorArchivedStatus.Click();
				_page.ButtonSubmitVendorForm.Click();
				BrowserUtility.WaitForElementToAppear("vendor-status");
				Assert.AreEqual(vendorStatus.ToUpper(), _page.DetailsVendorStatus.GetText(false));
				Log($"Vendor status was changed to Archived");
			}
		}
	}
}