using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[TestFixture]
	public class CategoryArchiveTests : BaseTest
	{
		private Model.Pages.CategoriesPage _page;
		private string _sourceSiteCategoryId;
		private SourceSiteCategorySingleSaveRequest _sourceSiteCategoryPostRequest;

		public CategoryArchiveTests() : base(nameof(CategoryArchiveTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_sourceSiteCategoryId = RequestUtility.GetUniqueId();
			_page = new Model.Pages.CategoriesPage();
			_sourceSiteCategoryPostRequest = RequestUtility.GetSourceSiteCategorySaveRequest(_sourceSiteCategoryId);
		}

		[Test]
		[Category("Category")]
		public void CategoryArchive_ValidateConfirmationDialog_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var setupResponse = SetupDataAndPage();

				//Click on the archive button for the displayed category
				var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
				tableCell.HoverOver();
				Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, setupResponse.Item1).ClickAndWaitForPageToLoad();
				_page.ArchiveMenuOption.Click();
				Log("Clicked the archive category button.");

				//Validate that the cancel and archive buttons are present
				Assert.AreEqual("CANCEL", _page.ButtonConfirmationDialogCancel.GetText());
				Assert.AreEqual("ARCHIVE", _page.ButtonConfirmationDialogAction.GetText());

				//Validate the text within the confirmation dialog
				var dialogText = $"Are you sure you want to archive {setupResponse.Item1}? Archiving a global category will unmap all site categories related to {setupResponse.Item1}.";
				Assert.AreEqual(dialogText, _page.ConfirmationDialogText.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(setupResponse.Item2);
			});
		}

		[Test]
		[Category("Category")]
		public void CategoryArchive_ValidateCancelButton_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var setupResponse = SetupDataAndPage();

				//Click on the archive button for the displayed category
				Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, setupResponse.Item1).ClickAndWaitForPageToLoad();
				_page.ArchiveMenuOption.Click();
				Log("Clicked the archive category button.");

				//Click the cancel button
				_page.ButtonConfirmationDialogCancel.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Click the cancel button.");

				//Validate that the Status value for the category is still listed as Active
				Assert.AreEqual(nameof(CategoryStatusType.Active), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(setupResponse.Item2);
			});
		}

		[Test]
		[Category("Category")]
		public void CategoryArchive_ValidateArchiveButtonAndUnmapping_Succeeds()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_page);

				//Create a new category
				var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

				//Create a site category and map it to the category
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
				Log($"Created site category. SiteCategoryId: {_sourceSiteCategoryId}.");

				var sourceSiteCategoryId = IntegrationApiService.GetSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId).Result.ProductCatalogSiteCategoryId;
				Log($"Retrieved site category. SourceSiteCategoryId: {sourceSiteCategoryId}");

				var postMapRequest = RequestUtility.GetSiteCategoryToCategoryMapSaveRequest(categoryId, sourceSiteCategoryId);
				CategoryAdminApiService.PostMapSiteCategories(postMapRequest);
				Log($"Map site categories post succeeded. CategoryId: {postMapRequest.CategoryId}, SiteCategoryId: {postMapRequest.SiteCategoryId}.");

				//Filter for the new category in the table and apply filters
				_page.InputFilterCategoryName.SendKeys(categoryName);
				Log($"Typed {categoryName} into the category name box.");
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the apply filters button.");

				//Click on the archive button for the displayed category
				var tableCell = BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status);
				tableCell.HoverOver();
				Model.Pages.CategoriesPage.GetButtonArchiveCategoryByRowNumber(1, categoryName).ClickAndWaitForPageToLoad();
				_page.ArchiveMenuOption.Click();
				Log("Clicked the archive category button.");

				//Click the cancel button
				_page.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Click the archive button.");

				//Validate that the Status value for the category is updated to archived and all count columns are updated to 0
				Assert.AreEqual(nameof(CategoryStatusType.Archived), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.Status).GetText());

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		private Tuple<string, string> SetupDataAndPage()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);

			//Open the page
			OpenPage(_page);

			//Setup data by creating a new category
			var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

			//Filter for the new category in the table and apply filters
			_page.InputFilterCategoryName.SendKeys(categoryName);
			Log($"Typed {categoryName} into the category name box.");
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");

			return Tuple.Create(categoryName, categoryId.ToString());
		}
	}
}
