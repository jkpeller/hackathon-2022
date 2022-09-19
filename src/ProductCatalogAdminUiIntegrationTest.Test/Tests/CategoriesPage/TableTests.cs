using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoriesPage
{
	[TestFixture]
	public class TableTests : BaseTest
	{
		private Model.Pages.CategoriesPage _page;
        private Model.Pages.CategoryDetailsPage _detailsPage;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.CategoriesPage();
            _detailsPage = new Model.Pages.CategoryDetailsPage();
        }

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void CategoriesTable_FilterWithNoResults_Succeeds()
		{
			const string noResultsMessageText = "No results were returned";
			var filterText = RequestUtility.GetUniqueId();
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//type the filter text into the category name filter box and click the apply filters button
				_page.InputFilterCategoryName.SendKeys(filterText);
				Log($"Typed {filterText} into the category name filter");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button");

				//validate that the no results were returned message is displayed in the table
				Assert.AreEqual(noResultsMessageText, _page.MessageTableNoResults.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void CategoriesTable_SortColumnsAscending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();

				//Validate that the category name column is sorted
				AssertColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Name);
				Log("Category name column is sorted.");

				//Validate that the publish status column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the status column.");
				AssertColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Status);
				Log("Status column is sorted.");

				//Validate that the capterra column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				Log("Clicked the capterra column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Capterra);
				Log("Capterra column is sorted.");

				//Validate that the software advice column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				Log("Clicked the software advice column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.SoftwareAdvice);
				Log("Software Advice column is sorted.");

				//Validate that the getapp column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				Log("Clicked the getapp column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Getapp);
				Log("Getapp column is sorted.");

				//Validate that the total column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderTotalCategoryCount.ClickAndWaitForPageToLoad();
				Log("Clicked the total column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Total);
				Log("Total column is sorted.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void CategoriesTable_SortColumnsDescending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();

				//Validate that the category name column is sorted
				_page.TableHeaderCategoryName.ClickAndWaitForPageToLoad();
				_page.TableHeaderCategoryName.ClickAndWaitForPageToLoad();
				Log("Clicked the name column.");
				AssertColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Name, SortDirectionType.Descending);
				Log("Category name column is sorted.");

				//Validate that the publish status column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the status column.");
				AssertColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Status, SortDirectionType.Descending);
				Log("Status column is sorted.");

				//Validate that the capterra column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				Log("Clicked the capterra column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Capterra, SortDirectionType.Descending);
				Log("Capterra column is sorted.");

				//Validate that the software advice column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				Log("Clicked the software advice column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.SoftwareAdvice, SortDirectionType.Descending);
				Log("Software Advice column is sorted.");

				//Validate that the getapp column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				Log("Clicked the getapp column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Getapp, SortDirectionType.Descending);
				Log("Getapp column is sorted.");

				//Validate that the total column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderTotalCategoryCount.ClickAndWaitForPageToLoad();
				_page.TableHeaderTotalCategoryCount.ClickAndWaitForPageToLoad();
				Log("Clicked the total column.");
				AssertNumericColumnIsSorted(Model.Pages.CategoriesPage.ColumnNameSelector.Total, SortDirectionType.Descending);
				Log("Total column is sorted.");
			});
		}

		[Test]
		[Category("Category")]
		public void ReactivateCategory_ValidateConfirmationDialog_Succeeds()
		{
			const string expectedDialogActionText = "ACTIVATE";
			ExecuteTimedTest(() =>
			{
				var setupResponse = SetupDataAndPage();

				//Click on the unarchive button for the displayed category
				var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
				tableCell.HoverOver();
				Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, setupResponse.Item2).ClickAndWaitForPageToLoad();
				_page.UnarchiveMenuOption.Click();
				Log("Clicked the archive category button.");

				//Validate that the cancel and activate buttons are present and correct
				Assert.IsTrue(_page.ButtonConfirmationDialogCancel.IsDisplayed());
				Log("The cancel button was displayed.");
				Assert.AreEqual(expectedDialogActionText, _page.ButtonConfirmationDialogAction.GetText());
				Log($"Text retrieved from activate button was correct. Text: {_page.ButtonConfirmationDialogAction.GetText()}");

				//Validate that the dialog contains the correct text
				var confirmationText = $"Are you sure you want to activate {setupResponse.Item2}?";
				Assert.AreEqual(confirmationText, _page.ConfirmationDialogText.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(setupResponse.Item1);
			});
		}

		[Test]
		[Category("Category")]
		public void ReactivateCategory_CancelUnarchive_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var setupResponse = SetupDataAndPage();

				//Click the cancel button
				_page.ButtonConfirmationDialogCancel.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the cancel button.");

				//Assert that the Status value for the record is still shown as Archived
				Assert.AreEqual(nameof(CategoryStatusType.Archived), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(setupResponse.Item1);
			});
		}

		[Test]
		[Category("Category")]
		public void ReactivateCategory_ConfirmUnarchive_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var setupResponse = SetupDataAndPage();

				//Click on the unarchive button for the displayed category
				var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
				tableCell.HoverOver();
				Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, setupResponse.Item2).ClickAndWaitForPageToLoad();
				_page.UnarchiveMenuOption.Click();
				Log("Clicked the archive category button.");

				//Click the cancel button
				_page.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the activate button.");

				//Assert that the Status value for the record is now shown as Active
				Assert.AreEqual(nameof(CategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(setupResponse.Item1);
			});
		}

		[Test]
		[Category("Category")]
		public void CategoriesTable_ValidateUpdatedJustNow_Succeeds()
		{
			const string expectedText = "Just now";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Setup a category
				var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
				var categoryName = categoryPostResponse.Result.Name;

				//Type the category name into the text filter box
				_page.InputFilterCategoryName.SendKeys(categoryName, true);
				Log($"Typed {categoryName} into the category filter text box.");

				//Click apply filters
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Validate that the Updated column contains the correct text
				Assert.AreEqual(expectedText, Model.Pages.CategoriesPage.GetCategoryUpdatedByRowNumber(1).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryPostResponse.Result.CategoryId.ToString());
			});
		}

        [Test]
        [Category("Category")]
        public void CategoriesTable_ValidateUrlContainsCategoryName_Succeeds()
        {
            string expectedText1 = "categoryName";
            string expectedText2 = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
            {
                //Open the page
                OpenPage(_page);

                //Setup a category
                var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = expectedText2 });
                var categoryName = categoryPostResponse.Result.Name;

                //Type the category name into the text filter box
                _page.InputFilterCategoryName.SendKeys(categoryName, true);
                Log($"Typed {categoryName} into the category filter text box.");

                //Click apply filters
                _page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Validate that the Updated column contains the correct text
                string searchUrl = BrowserUtility.WebDriver.Url;
				Assert.True(searchUrl.Contains(expectedText1));
                Assert.True(searchUrl.Contains(expectedText2));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryPostResponse.Result.CategoryId.ToString());
            });
        }

        [Test]
        [Category("Category")]
        public void CategoriesTable_ValidateUrlContainsStatuses_Succeeds()
        {
            string expectedText1 = "categoryStatusIds";
            string expectedText2 = "1,2";
            ExecuteTimedTest(() =>
            {
                //Open the page
                OpenPage(_page);

                //Setup a category
                var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
                var categoryName = categoryPostResponse.Result.Name;

				//Click to filter by archived categories only
                _page.SelectCategoryStatus.Click();
                Log("Clicked to open the category status filter box.");
                _page.SelectStatusActive.Check();
                Log("Checked the active status option.");
                _page.SelectStatusArchived.Check();
                Log("Checked the archived status option.");
                _page.SelectStatusArchived.SendKeys();

				//Click apply filters
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Validate that the Updated column contains the correct text
                string searchUrl = BrowserUtility.WebDriver.Url;
                Assert.True(searchUrl.Contains(expectedText1));
                Assert.True(searchUrl.Contains(expectedText2));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryPostResponse.Result.CategoryId.ToString());
            });
        }

        [Test]
        [Category("Category")]
        public void CategoriesTable_ValidateUrlContainsTagIds_Succeeds()
        {
            string expectedText1 = "tagIds";
            string expectedText2;

            ExecuteTimedTest(() =>
            {
                //Open the page
                OpenPage(_page);

				//Setup a category
				var (categoryId, categoryName) = CreateCategory();
                var (tagId, tagName) = CreateTag(TagType.CategoryTag);
                var (tagId2, tagName2) = CreateTag(TagType.CategoryTag);

                int compareResult = String.Compare(tagName, tagName2, StringComparison.Ordinal);
                expectedText2 = compareResult < 0 ? tagId + "," + tagId2 : tagId2 + "," + tagId;

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

				// Filter by new tag
				_detailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName2).Click();
                _detailsPage.SelectCategoryTagFilter.SendKeys();

				//Click apply filters
                _page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                //Validate that the Updated column contains the correct text
                string searchUrl = BrowserUtility.WebDriver.Url;
                Assert.True(searchUrl.Contains(expectedText1));
                Assert.True(searchUrl.Contains(expectedText2));

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
                CategoryAdminApiService.DeleteTag(tagId.ToString());
                CategoryAdminApiService.DeleteTag(tagId2.ToString());
            });
        }
		
        [Test]
        [Category("Category")]
        public void CategoriesTable_ValidateRestorePreviousSearchUrlWhenBackButtonClicked_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                OpenPage(_page);

                //Setup a category and 2 tags
                var (categoryId, categoryName) = CreateCategory();
                var (tagId, tagName) = CreateTag(TagType.CategoryTag);
                var (tagId2, tagName2) = CreateTag(TagType.CategoryTag);

				// Refresh page in order to see newly created tags
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

                // Select objects for the filter
				//Type the category name into the text filter box
				_page.InputFilterCategoryName.SendKeys(categoryName, true);
                Log($"Typed {categoryName} into the category filter text box.");

				//Click to filter by archived categories only
				_page.SelectCategoryStatus.Click();
                Log("Clicked to open the category status filter box.");
                _page.SelectStatusActive.Check();
                Log("Checked the active status option.");
                _page.SelectStatusArchived.Check();
                Log("Checked the archived status option.");
                _page.SelectStatusArchived.SendKeys();

				// Filter by new tag
				_detailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName2).Click();
                _detailsPage.SelectCategoryTagFilter.SendKeys();

                //Click apply filters
                _page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

				// Store the Url
                string searchUrl = BrowserUtility.WebDriver.Url;

				// Navigate to other page
                BrowserUtility.WebDriver.Navigate().GoToUrl("https://www.google.com");
                BrowserUtility.WaitForPageToLoad();
                BrowserUtility.WaitForOverlayToDisappear();
                Thread.Sleep(5000);

				// Navigate back
				BrowserUtility.NavigateBack();

				// Assert current Url is equal to previous Url
				Assert.AreEqual(searchUrl, BrowserUtility.WebDriver.Url);

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
                CategoryAdminApiService.DeleteTag(tagId.ToString());
                CategoryAdminApiService.DeleteTag(tagId2.ToString());
			});
        }

		[Test]
		[Category("Category")]
		public void CategoriesTable_ValidateDefaultFilterRestoredWhenParametersRemoved_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

                string defaultFilterUrl = BrowserUtility.WebDriver.Url;

				//Setup a category and 2 tags
				var (categoryId, categoryName) = CreateCategory();
				var (tagId, tagName) = CreateTag(TagType.CategoryTag);
				var (tagId2, tagName2) = CreateTag(TagType.CategoryTag);

                // Refresh page in order to see newly created tags
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

				// Select objects for the filter
				//Type the category name into the text filter box
				_page.InputFilterCategoryName.SendKeys(categoryName, true);
				Log($"Typed {categoryName} into the category filter text box.");

				//Click to filter by archived categories only
				_page.SelectCategoryStatus.Click();
				Log("Clicked to open the category status filter box.");
				_page.SelectStatusActive.Check();
				Log("Checked the active status option.");
				_page.SelectStatusArchived.Check();
				Log("Checked the archived status option.");
				_page.SelectStatusArchived.SendKeys();

				// Filter by new tag
				_detailsPage.SelectCategoryTagFilter.Click();
				Log("Clicked into the tag filter box.");
				Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
				Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName2).Click();
				_detailsPage.SelectCategoryTagFilter.SendKeys();

				//Click apply filters
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

                string searchUrl = BrowserUtility.WebDriver.Url;
                // Assert current search Url is different than default
				Assert.AreNotEqual(defaultFilterUrl, searchUrl);

                string urlWithQueryParamsRemoved = searchUrl.Split('?')[0];

				BrowserUtility.WebDriver.Navigate().GoToUrl(urlWithQueryParamsRemoved);
				BrowserUtility.WaitForPageToLoad();
				BrowserUtility.WaitForOverlayToDisappear();
                Thread.Sleep(5000);

				Assert.AreEqual(defaultFilterUrl, BrowserUtility.WebDriver.Url);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteTag(tagId.ToString());
				CategoryAdminApiService.DeleteTag(tagId2.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void CategoriesTable_ProductAndFeatureCountDisplayed_Succeeds()
		{
            const string expectedProductCount = "2";
            const string expectedFeatureCount = "1";
			const int featureId = 339749;
			ExecuteTimedTest(() =>
			{
                OpenPage(_page);

                // Setup a category
                var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
                var categoryName = categoryPostResponse.Result.Name;
                var categoryId = categoryPostResponse.Result.CategoryId;

				// Add Feature to the Category
                var categoryFeaturePostResponse = CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
                    new CategoryFeatureInsertRequest
                    {
                        FeatureId = featureId,
						FeatureTypeId = FeatureType.Core
                    });
                var categoryFeatureId = categoryFeaturePostResponse.Result.CategoryFeatureId;

				// Add a product to the Category
                string prodName1 = RequestUtility.GetRandomString(9);
                string prodName2 = RequestUtility.GetRandomString(9);
				var productPostResponse1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
                {
					Name = prodName1,
					ProductWebsiteUrl = $"https://www.{prodName1}.com",
                    CategoryIds = new List<int>
                    {
                        categoryPostResponse.Result.CategoryId
					}
                });
                var productPostResponse2 = ProductAdminApiService.PostProduct(new ProductInsertRequest
                {
                    Name = prodName2,
                    ProductWebsiteUrl = $"https://www.{prodName2}.com",
                    CategoryIds = new List<int>
                    {
                        categoryPostResponse.Result.CategoryId
                    }
                });

				// Type the category name into the text filter box
				_page.InputFilterCategoryName.SendKeys(categoryName, true);
                Log($"Typed {categoryName} into the category filter text box.");
                // Click apply filters
                _page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

				Assert.AreEqual(expectedProductCount, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.ProductCount).GetText(false));
                Assert.AreEqual(expectedFeatureCount, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.FeatureCount).GetText(false));

                // Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productPostResponse1.Result.ProductId.ToString());
                ProductAdminApiService.DeleteProduct(productPostResponse2.Result.ProductId.ToString());
				CategoryAdminApiService.DeleteCategoryFeature(categoryFeatureId.ToString());
            });
}

		private Tuple<string, string> SetupDataAndPage()
		{
			//Open the page
			OpenPage(_page);

			//Setup a category
			var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
			var categoryId = categoryPostResponse.Result.CategoryId;
			var categoryName = categoryPostResponse.Result.Name;

			//Update the category to be archived
			CategoryAdminApiService.PutCategory(categoryId.ToString(), new CategoryUpdateRequest { CategoryStatusId = (int)CategoryStatusType.Archived, Name = categoryName });

			//Type the category name into the text filter box
			_page.InputFilterCategoryName.SendKeys(categoryName, true);
			Log($"Typed {categoryName} into the category filter text box.");

			//Click to filter by archived categories only
			_page.SelectCategoryStatus.Click();
			Log("Clicked to open the category status filter box.");
			_page.SelectStatusActive.Uncheck();
			Log("Unchecked the active status option.");
			_page.SelectStatusArchived.Check();
			Log("Checked the archived status option.");
			_page.SelectStatusArchived.SendKeys();

			//Click apply filters
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");

			//Click the unarchive button for the row returned by the filters
			var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
			tableCell.HoverOver();
			Model.Pages.CategoriesPage.GetButtonUnarchiveCategoryByRowNumber(1).ClickAndWaitForPageToLoad();
			Log("Clicked the unarchive button for row 1.");

			return Tuple.Create(categoryId.ToString(), categoryName);
		}

		private void OpenPageAndFilterAllStatuses()
		{
			OpenPage(_page);

			//Filter on all status values and apply filters
			_page.SelectCategoryStatus.Click();
			Log("Clicked into the status filter box.");
			_page.SelectStatusArchived.Check();
			Log("Selected the archived status value.");
			_page.SelectStatusArchived.SendKeys();
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");
		}
	}
}
