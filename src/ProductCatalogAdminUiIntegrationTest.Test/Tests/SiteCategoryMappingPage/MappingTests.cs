using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteCategoryMappingPage
{
	[TestFixture]
	public class MappingTests : BaseTest
	{
		private string _sourceSiteCategoryId;
		private string _categoryName;
		private CategoryInsertRequest _categoryPostRequest;
		private SourceSiteCategorySingleSaveRequest _sourceSiteCategoryPostRequest;
		private string _sourceSiteCategoryName;
		private Model.Pages.SiteCategoryMappingPage _page;

		public MappingTests() : base(nameof(MappingTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_sourceSiteCategoryId = RequestUtility.GetUniqueId();
			_categoryName = RequestUtility.GetRandomString(9);
			_categoryPostRequest = RequestUtility.GetCategoryInsertRequest(_categoryName);
			_sourceSiteCategoryPostRequest = RequestUtility.GetSourceSiteCategorySaveRequest(_sourceSiteCategoryId);
			_sourceSiteCategoryName = _sourceSiteCategoryPostRequest.Name;
			_page = new Model.Pages.SiteCategoryMappingPage();
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryMapping_ToValidGlobalCategory_Succeeds()
		{
			const string expectedMappingButtonText = "SAVE MAPPING";
			ExecuteTimedTest(() =>
			{
				//setup data by creating a new source site category and global category
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);

				//Open the page, filter by new site category name, all mapping status and publish status values, then click apply filters
				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);
				var categoryId = CategoryAdminApiService.PostCategory(_categoryPostRequest).Result.CategoryId;

				//Click in the result row to open the mapping modal and type in the new category name value
				OpenMappingModalAndSearchForCategory(_page, _categoryName);

				//click the suggested name that matches the new global category name
				Model.Pages.SiteCategoryMappingPage.GetModalGlobalCategorySuggestionByName(_categoryName.ToLower()).Click();
				Log($"Click the suggestion with text '{_categoryName}' from the category mapping modal search box.");

				//Assert that the map button is correct
				Assert.AreEqual(expectedMappingButtonText, _page.ButtonMappingModalMap.GetText());

				//click the map button
				_page.ButtonMappingModalMap.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Click the map button.");

				//verify that the global category column for the record now contains the name of the new global category
				Assert.AreEqual(_categoryName, Model.Pages.SiteCategoryMappingPage.GetTableGlobalCategoryDisplayByRow(1).GetText());

				//cleanup the global and site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryMapping_CancelMapping_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup data by creating a new source site category and global category
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);

				//Open the page, filter by new site category name, all mapping status and publish status values, then click apply filters
				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);

				var categoryId = CategoryAdminApiService.PostCategory(_categoryPostRequest).Result.CategoryId;

				//Click in the result row to open the mapping modal and type in the new category name value
				OpenMappingModalAndSearchForCategory(_page, _categoryName);

				//click the suggested name that matches the new global category name
				Model.Pages.SiteCategoryMappingPage.GetModalGlobalCategorySuggestionByName(_categoryName.ToLower()).Click();
				Log($"Click the suggestion with text '{_categoryName}' from the category mapping modal search box.");

				//click the cancel button
				_page.ButtonMappingModalCancel.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Click the map button.");

				//verify that the global category column for the record contains no text because it is still unmapped
				Assert.IsTrue(BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.GlobalCategory).GetText().Contains("ADD MAPPING"));

				//cleanup the global and site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryMapping_AddNewGlobalCategory_Succeeds()
		{
			const string snackBarSuccessText = "Global category created";
			ExecuteTimedTest(() =>
			{
				//setup data by creating a new source site category
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
				
				//Open the page, filter by new site category name, all mapping status and publish status values, then click apply filters
				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);

				//Click in the result row to open the mapping modal and type in the new category name value
				OpenMappingModalAndSearchForCategory(_page, _categoryName);

				//click the add it as a global category button
				_page.SpanAddGlobalCategory.Click();
				Thread.Sleep(500);
				Log("Click the add global category button inside the category mapping modal search box.");

				//verify that the snack bar message appears and contains the correct text
				Assert.AreEqual(_page.SnackBarContainer.GetText(), snackBarSuccessText);
				Log("Snack bar message text was correct.");

				//cleanup the global and site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				var categoryId = CategoryAdminApiService.GetCategoriesByPartialName(_categoryName).Result.First().CategoryId;
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategory_AddAndMapToNewGlobalCategory_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup data by creating a new source site category
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
				
				//Open the page, filter by new site category name, all mapping status and publish status values, then click apply filters
				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);

				//Click in the result row to open the mapping modal and type in the new category name value
				OpenMappingModalAndSearchForCategory(_page, _categoryName);

				//click the add it as a global category button
				_page.SpanAddGlobalCategory.Click();
				Log("Click the add global category button inside the category mapping modal search box.");

				//click the map button
				_page.ButtonMappingModalMap.ClickAndWaitForPageToLoad();
				Log("Click the map button.");

				//verify that the global category column for the record now contains the name of the new global category
				Assert.AreEqual(_categoryName, Model.Pages.SiteCategoryMappingPage.GetTableGlobalCategoryDisplayByRow(1).GetText());
				
				//cleanup the global and site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				var categoryId = CategoryAdminApiService.GetCategoriesByPartialName(_categoryName).Result.First().CategoryId;
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryDeleteMapping_ValidateDialog_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//select all publish and mapping status filter options and click apply filters
				var setupResponse = OpenPageAndFilterBySiteCategoryAndAllFilters(_sourceSiteCategoryName);

				//click the delete mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryDeleteMappingButtonByRowNumber(1).Click();
				Log("Click the site category unmapping button for the result record row.");

				//verify that the modal pops up and the message in it is correct
				var dialogText = $"Are you sure you want to remove the {_categoryName} global category from the {_sourceSiteCategoryName.Trim()} category?";
				Assert.AreEqual(dialogText, _page.ConfirmationDialogText.GetText());

				//cleanup the source site category and category
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(setupResponse.Item2);
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryDeleteMapping_CancelDelete_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//select all publish and mapping status filter options and click apply filters
				var setupResponse = OpenPageAndFilterBySiteCategoryAndAllFilters(_sourceSiteCategoryName);

				//click the delete mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryDeleteMappingButtonByRowNumber(1).Click();
				Log("Click the site category unmapping button for the result record row.");

				//click the cancel button
				_page.ButtonConfirmationDialogCancel.ClickAndWaitForPageToLoad();
				Log("Clicked the cancel button.");

				//verify that the global category column for the record now contains the name of the new global category
				Assert.AreEqual(_categoryName, Model.Pages.SiteCategoryMappingPage.GetTableGlobalCategoryDisplayByRow(1).GetText());

				//cleanup the source site category and category
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(setupResponse.Item2);
			});
		}

		[Test]
		[Category("Category")]
		public void SiteCategoryDeleteMapping_ConfirmDelete_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//select all publish and mapping status filter options and click apply filters
				var setupResponse = OpenPageAndFilterBySiteCategoryAndAllFilters(_sourceSiteCategoryName);

				//click the delete mapping button for the first result record
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryDeleteMappingButtonByRowNumber(1).ClickAndWaitForPageToLoad();
				Log("Click the site category unmapping button for the result record row.");

				//click the remove button button
				_page.ButtonConfirmationDialogAction.ClickAndWaitForPageToLoad();
				Log("Clicked the remove button.");

				//verify that the add mapping button is now visible for that row
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Assert.IsTrue(Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).IsDisplayed());

				//cleanup the source site category and category
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(setupResponse.Item2);
			});
		}

		private void OpenPageAndFilterBySiteCategoryNameAndAllFilters(BasePage page, string sourceSiteCategoryName)
		{
			OpenPage(page);

			//type the name of the new site category in the category name filter box
			page.InputFilterCategoryName.SendKeys(sourceSiteCategoryName);

			//select all options from the mapping status filter box
			page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened.");
			page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			page.SelectOptionMappingStatusMapped.SendKeys(hoverOver: false);

			//select all options from the publish status filter box
			page.SelectPublishStatus.Click(false);
			Log("Publish status filter drop down clicked.");
			page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			page.SelectOptionPublishStatusUnpublished.SendKeys(hoverOver: false);

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
		}

		private void OpenMappingModalAndSearchForCategory(Model.Pages.SiteCategoryMappingPage page, string categoryName)
		{
			//click the site category mapping button in the row for the new site category record in the table
			BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
			Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).Click();
			Log("Click the site category mapping button for the result record row.");

			//search for the new global category in the modal search box
			page.InputMappingModalSearchText.SendKeys(categoryName, sendEscape: false);
			Thread.Sleep(500);
			Log($"Entered {categoryName} into the category mapping modal search box.");	
		}

		private Tuple<int, string> OpenPageAndFilterBySiteCategoryAndAllFilters(string siteCategoryName)
		{
			OpenPage(_page);

			//setup data by creating a new site category
			IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
			var siteCategoryId = IntegrationApiService.GetSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId).Result.ProductCatalogSiteCategoryId;

			//create a category
			var categoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = _categoryName }).Result.CategoryId;

			//map the new site product to a known global product
			var mapSaveRequest = RequestUtility.GetSiteCategoryToCategoryMapSaveRequest(categoryId, siteCategoryId);
			CategoryAdminApiService.PostMapSiteCategories(mapSaveRequest);

			//Filter on the site product name
			_page.InputFilterCategoryName.SendKeys(siteCategoryName);

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

			return Tuple.Create(siteCategoryId, categoryId.ToString());
		}
	}
}
