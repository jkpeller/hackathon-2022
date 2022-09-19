using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[Category("Category")]
	public class CategorySEONamesTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public CategorySEONamesTests() : base(nameof(CategoryEditTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}	


		[Test]
		public void EditCategoryDetails_SEONamesCard_VerifyCapTerraTabElements()
		{
			var categoryName = RequestUtility.GetRandomString(9);
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

				//Assert Capterra SEO Names Elements 
				_categoryDetailsPage.SeoNameCapTerraTab.Click();
				Log($"Click on the SEO Names Capterra tab to assert the elements displayed on the card");
				Thread.Sleep(1500);
				Assert.IsTrue(_categoryDetailsPage.InputEditCapterraSeoName.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonSaveCategoryCapterraSeoNames.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonCancelCategoryCapterraSeoNames.IsDisplayed());
						
				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_VerifyGetAppTabElements()
		{
			var categoryName = RequestUtility.GetRandomString(9);
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

				//Assert GetApp SEO Names Elements 
				_categoryDetailsPage.SeoNameGetAppTab.Click();
				Log($"Click on the SEO Names GetApp tab to assert the elements displayed on the card");
				Thread.Sleep(1000);
				Assert.IsTrue(_categoryDetailsPage.InputEditGetAppSeoName.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonSaveCategoryGetAppSeoNames.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonCancelCategoryGetAppSeoNames.IsDisplayed());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_VerifySoftwareAdviseTabElements()
		{
			var categoryName = RequestUtility.GetRandomString(9);
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

				//Assert GetApp SEO Names Elements 
				_categoryDetailsPage.SeoNameSoftwareAdviseTab.Click();
				Log($"Click on the SEO Names Software Advise tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				Assert.IsTrue(_categoryDetailsPage.InputEditSoftwareAdviseSeoName.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonSaveCategorySoftwareAdviseSeoNames.IsDisplayed());
				Assert.IsTrue(_categoryDetailsPage.ButtonCancelCategorySoftwareAdviseSeoNames.IsDisplayed());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_SaveSeoNamesForCapterra()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(9);
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

				//Assert Capterra SEO Names Elements 
				_categoryDetailsPage.SeoNameCapTerraTab.Click();
				Log($"Click on the SEO Names Capterra tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditCapterraSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditCapterraSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategoryCapterraSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditCapterraSeoName.GetTextValue()} ");

				//Assert SEO Names value
				Assert.AreEqual(_categoryDetailsPage.InputEditCapterraSeoName.GetTextValue(), seoNameValue);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_SaveSeoNamesForGetApp()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(9);
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

				//Assert GetApp SEO Names Elements 
				_categoryDetailsPage.SeoNameGetAppTab.Click();
				Log($"Click on the SEO Names Get App tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditGetAppSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditGetAppSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategoryGetAppSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditGetAppSeoName.GetTextValue()} ");

				//Assert SEO Names value
				Assert.AreEqual(_categoryDetailsPage.InputEditGetAppSeoName.GetTextValue(), seoNameValue);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_SaveSeoNamesForSoftwareAdvise()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(9);
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

				//Assert Software Advise SEO Names Elements 
				_categoryDetailsPage.SeoNameSoftwareAdviseTab.Click();
				Log($"Click on the SEO Names Software Advise tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditSoftwareAdviseSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditSoftwareAdviseSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategorySoftwareAdviseSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditSoftwareAdviseSeoName.GetTextValue()} ");
			
				//Assert SEO Names value
				Assert.AreEqual(_categoryDetailsPage.InputEditSoftwareAdviseSeoName.GetTextValue(), seoNameValue);
	
				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_AttemptSaveSeoNamesAboveMaximumValueForCapterra()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(101);
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

				//Assert Capterra SEO Names Elements 
				_categoryDetailsPage.SeoNameCapTerraTab.Click();
				Log($"Click on the SEO Names Capterra tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditCapterraSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditCapterraSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategoryCapterraSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditCapterraSeoName.GetTextValue()} ");

				//Assert that only 100 characters have been saved in the SEO Names card
				Assert.AreEqual(seoNameValue.Substring(0, seoNameValue.Length - 1), _categoryDetailsPage.InputEditCapterraSeoName.GetTextValue());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_AttemptSaveSeoNamesAboveMaximumValueForGetApp()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(101);
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

				//Assert GetApp SEO Names Elements 
				_categoryDetailsPage.SeoNameGetAppTab.Click();
				Log($"Click on the SEO Names Get App tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditGetAppSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditGetAppSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategoryGetAppSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditGetAppSeoName.GetTextValue()} ");

				//Assert that only 100 characters have been saved in the SEO Names card
				Assert.AreEqual(seoNameValue.Substring(0, seoNameValue.Length - 1), _categoryDetailsPage.InputEditGetAppSeoName.GetTextValue());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void EditCategoryDetails_SEONamesCard_AttemptSaveSeoNamesAboveMaximumValueSoftwareAdvise()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var seoNameValue = RequestUtility.GetRandomString(101);
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

				//Assert Software Advise SEO Names Elements 
				_categoryDetailsPage.SeoNameSoftwareAdviseTab.Click();
				Log($"Click on the SEO Names Software Advise tab to assert the elements displayed on the cards");
				Thread.Sleep(1000);
				_categoryDetailsPage.InputEditSoftwareAdviseSeoName.SendKeys(clear: true);
				_categoryDetailsPage.InputEditSoftwareAdviseSeoName.SendKeys(seoNameValue);
				Log($"SEO Name value typed on the card was { seoNameValue } ");
				_categoryDetailsPage.ButtonSaveCategorySoftwareAdviseSeoNames.Click();
				Thread.Sleep(1000);
				Log($"SEO Name value saved on the card was { _categoryDetailsPage.InputEditSoftwareAdviseSeoName.GetTextValue()} ");

				//Assert that only 100 characters have been saved in the SEO Names card
				Assert.AreEqual(seoNameValue.Substring(0, seoNameValue.Length - 1), _categoryDetailsPage.InputEditSoftwareAdviseSeoName.GetTextValue());

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
