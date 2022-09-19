using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[Category("Category")]
	public class CategoryEditTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public CategoryEditTests() : base(nameof(CategoryEditTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}

		[Test]
		public void EditCategoryDetails_ByValidName_Succeeds()
		{
			const string expectedLabelText = "Global Category Name";
			var updatedCategoryName = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage(updatedCategoryName);

				//Click the Save button
				_categoryDetailsPage.ButtonSaveCategoryChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global category button.");

				//Assert that the name has updated and that the label is correct
				Assert.AreEqual(expectedLabelText, _categoryDetailsPage.LabelCategoryName.GetText());
				Assert.AreEqual(updatedCategoryName, _categoryDetailsPage.DisplayCategoryName.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditCategoryDetails_ByAboveMaximumNameValue_Succeeds()
		{
			var updatedCategoryName = RequestUtility.GetRandomString(101);
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage(updatedCategoryName);

				//Type in a new name for the category
				_categoryDetailsPage.InputEditCategoryName.SendKeys(updatedCategoryName, true);
				Log($"Typed {updatedCategoryName} into the edit category name box.");
				Thread.Sleep(3000);

				//Click the Save button
				_categoryDetailsPage.ButtonSaveCategoryChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global category button.");
				Thread.Sleep(3000);

				//Assert that the name has updated to only the first 100 characters of the above maximum value name
				Assert.AreEqual(updatedCategoryName.Substring(0, updatedCategoryName.Length - 1), _categoryDetailsPage.DisplayCategoryName.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditCategoryDetails_ByEmptyName_Fails()
		{
			const string updatedCategoryName = " ";
			const string expectedErrorText = "Category Name is required";
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage(updatedCategoryName);

				//Backspace out the text currently in the box
				_categoryDetailsPage.InputEditCategoryName.SendKeys(Keys.Backspace);
				Log("Typed blank space into the edit category name box.");
				Thread.Sleep(500);

				//Assert that the minimum length validation error is present
				Assert.AreEqual(expectedErrorText, _categoryDetailsPage.ErrorMessageRequiredCategoryName.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditCategoryDetails_ByByBelowMinimumName_Fails()
		{
			var updatedCategoryName = RequestUtility.GetRandomString(2);
			const string expectedErrorText = "You must type at least 3 characters";
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage(updatedCategoryName);

				//Assert that the minimum length validation error is present
				Assert.AreEqual(expectedErrorText, _categoryDetailsPage.ErrorMessageMinimumCategoryName.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditCategoryDetails_ByOnlySpacesName_Fails()
		{
			var emptySpaceCategoryName = new string(' ', 3);
			const string expectedErrorText = "You must type at least 3 characters";
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage(emptySpaceCategoryName);

				//Assert that the minimum length validation error is present
				Assert.AreEqual(expectedErrorText, _categoryDetailsPage.ErrorMessageMinimumCategoryName.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void EditCategoryDetails_VerifyCategoryDefinitionIsDisplayedInEditMode_Success()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Setup data by creating a new category
				var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);
				var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

				//Navigate to the page for the new category
				BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryId}");
				BrowserUtility.WaitForPageToLoad();
				BrowserUtility.WaitForOverlayToDisappear();
				Log($"Navigate to the new category page {BrowserUtility.WebDriver.Url}");
				Thread.Sleep(1000);

				//Assert that the category definition elements have been added and defaults to read only mode
				Assert.IsTrue(_categoryDetailsPage.ButtonEditCategory.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.DisplayCategoryDefinition.IsDisplayed());

				//Attempt to type a category definition in the category definition field
				_categoryDetailsPage.DisplayCategoryDefinition.SendKeys("type definition");
				Log("Attempt to type a category definition in the category definition field");

				//Assert that Category Definition field cannot be edited in read only mode
				Assert.IsEmpty(_categoryDetailsPage.DisplayCategoryDefinition.GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_ByEditingValidCategoryDefinition_Success()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryDefinition = RequestUtility.GetRandomString(5);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Setup data by creating a new category
				var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);
				var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

				//Navigate to the page for the new category
				BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryId}");
				BrowserUtility.WaitForPageToLoad();
				BrowserUtility.WaitForOverlayToDisappear();
				Log($"Navigate to the new category page {BrowserUtility.WebDriver.Url}");
				Thread.Sleep(1000);

				//Type a category definition that does not exceeds the 500 characters
				_categoryDetailsPage.ButtonEditCategory.Click();
				Log($"Type Category Definition:  { categoryDefinition }");
				_categoryDetailsPage.InputEditCategoryDefinition.SendKeys(categoryDefinition);
				Log("Click on the edit category button and type a category definition");

				//Click on Save Category button
				_categoryDetailsPage.ButtonSaveCategoryChanges.Click();
				Log("Click on the Save button to save category changes");
				Thread.Sleep(2000);

				//Assert the category definition value saved on the Category Details card
				Log($"Saved Category Definition:  { _categoryDetailsPage.DisplayCategoryDefinition.GetText()}");
				Assert.AreEqual(categoryDefinition, _categoryDetailsPage.DisplayCategoryDefinition.GetText().ToString());
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_ByExceedingCategoryDefinitionAboveMaximumLimits_Success()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryDefinition = RequestUtility.GetRandomString(501);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Setup data by creating a new category
				var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);
				var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

				//Navigate to the page for the new category
				BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryId}");
				BrowserUtility.WaitForPageToLoad();
				BrowserUtility.WaitForOverlayToDisappear();
				Log($"Navigate to the new category page { BrowserUtility.WebDriver.Url }");
				Thread.Sleep(1000);

				// Attempt to type a definition in the definition detail field
				_categoryDetailsPage.ButtonEditCategory.Click();
				_categoryDetailsPage.InputEditCategoryDefinition.SendKeys(categoryDefinition);
				Log("Click on the edit category button and type a 501 characters category definition");

				_categoryDetailsPage.ButtonSaveCategoryChanges.Click();
				Log("Click on the Save button to save category changes");
				Thread.Sleep(2000);

				//Assert that only 500 characters have been saved in the category defintion field
				Log($"Saved Category Definition:  { _categoryDetailsPage.DisplayCategoryDefinition.GetText()}");
				Assert.AreEqual(categoryDefinition.Substring(0,categoryDefinition.Length - 1), _categoryDetailsPage.DisplayCategoryDefinition.GetText().ToString());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}	

		private string SetupDataAndPage(string updatedCategoryName)
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);

			//Open the page
			OpenPage(_categoriesPage);

			//Setup data by creating a new category
			var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId;

			//Navigate to the page for the new category
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"/{BrowserUtility.CategoryPageName}/{categoryId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(1000);

			//Click the edit button for the category
			_categoryDetailsPage.ButtonEditCategory.Click();
			Log("Clicked the edit global category button.");

			//Type in a new name for the category
			_categoryDetailsPage.InputEditCategoryName.SendKeys(updatedCategoryName, true, false);
			Log($"Typed {updatedCategoryName} into the edit category name box.");
			Thread.Sleep(1000);

			return categoryId.ToString();
		}
	}
}
