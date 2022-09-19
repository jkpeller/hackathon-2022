using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[TestFixture]
	public class CategoryMappingTests : BaseTest
	{
		private string _sourceSiteCategoryId;
		private string _categoryName;
		private CategoryInsertRequest _categoryPostRequest;
		private SourceSiteCategorySingleSaveRequest _sourceSiteCategoryPostRequest;
		private string _sourceSiteCategoryName;
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public CategoryMappingTests() : base(nameof(CategoryMappingTests))
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
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}

		[Test]
		[Category("Category")]
		public void CategoryMapping_ValidateReadonlyState_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var categoryId = SetupTest(true, false).Item1;

				//Validate that the site search boxes do not appear in readonly state
				Assert.IsFalse(_categoryDetailsPage.InputSiteLevelSearchCapterra.IsDisplayed());
				Assert.IsFalse(_categoryDetailsPage.InputSiteLevelSearchSoftwareAdvice.IsDisplayed());
				Assert.IsFalse(_categoryDetailsPage.InputSiteLevelSearchGetapp.IsDisplayed());

				//Validate that a chip for the mapped site category is shown correctly, and there is no 'X' button to remove it
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteCategoryName.ToLower()).IsDisplayed());
				Assert.IsFalse(Model.Pages.CategoryDetailsPage.GetSiteCategoryChipRemoveBySiteCategoryName(_sourceSiteCategoryName.ToLower()).IsDisplayed());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void CategoryMapping_ValidateEditableState_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var categoryId = SetupTest(true, true).Item1;

				//Validate that the save and cancel buttons are visible
				Assert.IsTrue(_categoryDetailsPage.ButtonSaveSiteCategories.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonCancelEditSiteCategories.IsDisplayed());

				//Validate that a chip for the mapped site category is shown correctly and an 'X' button is present to remove it
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteCategoryName.ToLower()).IsDisplayed());
				Assert.IsTrue(Model.Pages.CategoryDetailsPage.GetSiteCategoryChipRemoveBySiteCategoryName(_sourceSiteCategoryName.ToLower()).IsDisplayed());

				//Validate that the site search boxes appear
				Assert.IsTrue(_categoryDetailsPage.InputSiteLevelSearchCapterra.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.InputSiteLevelSearchSoftwareAdvice.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.InputSiteLevelSearchGetapp.IsDisplayed());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void CategoryMapping_AddSiteCategoryAndSaveThenRemove_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var setupResponse = SetupTest(false, true);
				var categoryId = setupResponse.Item1;
				var siteCategoryId = setupResponse.Item2;

				//Type the site category name into the capterra search box
				_categoryDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteCategoryName, sendEscape: false);
				Log($"Typed {_sourceSiteCategoryName} into the Capterra site categories search box.");

				//Click on the suggested result
				_categoryDetailsPage.OptionFirstAutocompleteSuggestion.Click();
				Log("Clicked on the first autocomplete suggestion.");

				//Validate that a chip for the mapped site category is shown correctly
				Assert.IsTrue(BasePage.GetCapterraChipByName(_sourceSiteCategoryName.ToLower()).IsDisplayed());

				//Save the site categories
				_categoryDetailsPage.ButtonSaveSiteCategories.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save site categories button.");

				//Click the edit site categories button
				_categoryDetailsPage.ButtonEditSiteCategories.Click();
				Log("Clicked the edit site categories button.");

				//Click the 'X' button to remove the site category mapping
				Model.Pages.CategoryDetailsPage.GetSiteCategoryChipRemoveBySiteCategoryName(_sourceSiteCategoryName.ToLower()).Click();

				//Validate that the site category chip no longer shows up
				Assert.IsFalse(Model.Pages.CategoryDetailsPage.GetSiteCategoryChipRemoveBySiteCategoryName(_sourceSiteCategoryName.ToLower()).IsDisplayed());

				//Get the category from the Api and validate that the site category is still mapped
				var getResponseResult = CategoryAdminApiService.GetCategoryById(categoryId.ToString(), true).Result;
				Assert.AreEqual(siteCategoryId, getResponseResult.SiteCategories.Single().SiteCategoryId);

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		[Category("Category")]
		public void CategoryMapping_AddSiteCategoryAndCancel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup the test
				var categoryId = SetupTest(false, true).CategoryId;

				//Type the site category name into the Capterra search box and click on the suggested result
				_categoryDetailsPage.InputSiteLevelSearchCapterra.SendKeys(_sourceSiteCategoryName, sendEscape: false);
				Log($"Typed {_sourceSiteCategoryName} into the Capterra site categories search box.");

				//Click on the suggested result
				_categoryDetailsPage.OptionFirstAutocompleteSuggestion.Click();
				Log("Clicked on the first autocomplete suggestion.");

				//Cancel the edit site categories
				_categoryDetailsPage.ButtonCancelEditSiteCategories.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the cancel edit site categories button.");

				//Validate that the site category chip does not show up
				Assert.IsFalse(Model.Pages.CategoryDetailsPage.GetSiteCategoryChipRemoveBySiteCategoryName(_sourceSiteCategoryName.ToLower()).IsDisplayed());

				//Get the category by Id from the Api and validate that the site category is not mapped
				var getResponseResult = CategoryAdminApiService.GetCategoryById(categoryId.ToString(), true).Result;
				Assert.IsEmpty(getResponseResult.SiteCategories);

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		private (int CategoryId, int SourceSiteCategoryId) SetupTest(bool mapCategoryToSiteCategory, bool editSiteCategories)
		{
			//Open the page
			OpenPage(_categoriesPage);

			//Setup: create a category, a site category, and map them together
			var categoryId = CategoryAdminApiService.PostCategory(_categoryPostRequest).Result.CategoryId;
			Log($"Create category. CategoryId: {categoryId}");

			IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
			Log($"Created site category. SiteCategoryId: {_sourceSiteCategoryId}.");

			var sourceSiteCategoryId = IntegrationApiService.GetSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId).Result.ProductCatalogSiteCategoryId;
			Log($"Retrieved site category. SourceSiteCategoryId: {sourceSiteCategoryId}");

			if (mapCategoryToSiteCategory)
			{
				var postMapRequest = RequestUtility.GetSiteCategoryToCategoryMapSaveRequest(categoryId, sourceSiteCategoryId);
				CategoryAdminApiService.PostMapSiteCategories(postMapRequest);
				Log($"Map site categories post succeeded. CategoryId: {postMapRequest.CategoryId}, SiteCategoryId: {postMapRequest.SiteCategoryId}.");
			}

			//Filter for the new category in the table and apply filters
			_categoriesPage.InputFilterCategoryName.SendKeys(_categoryName);
			Log($"Typed {_categoryName} into the category name box.");
			_categoriesPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();

			//Click on the 1st table result category name link and switch to the new tab
			Model.Pages.CategoriesPage.GetLinkCategoryNameByRowNumber(1).ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the category name link in row 1.");
			BrowserUtility.SwitchToNewTab();

			if (!editSiteCategories)
				return (categoryId, sourceSiteCategoryId);

			//Click the edit site categories button
			_categoryDetailsPage.ButtonEditSiteCategories.ClickAndWaitForPageToLoad();
			Log("Clicked the edit site categories button.");

			return (categoryId, sourceSiteCategoryId);
		}
	}
}