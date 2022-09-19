using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
	[Category("Category")]
	public class CategorySitePublishStatusTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;
		private readonly List<string> _siteNames = new List<string> { "capterra", "getapp", "software-advice" };
		private readonly string IncompleteStatusName = nameof(PublishStatusType.Incomplete);
		private readonly string PublishStatusName = nameof(PublishStatusType.Published);
		private readonly string UnpublishStatusName = nameof(PublishStatusType.Unpublished);

		public CategorySitePublishStatusTests() : base(nameof(CategorySitePublishStatusTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
			OpenPage(_categoriesPage);
		}

		[Test]
		public void CategorySitePublishStatus_ValidateDefaultIncompleteStatusForNewCategory_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();

				//Validate that each site is defaulted to Incomplete status
				foreach (var siteName in _siteNames)
				{
					Assert.AreEqual(IncompleteStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {IncompleteStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void CategorySitePublishStatus_PublishCategory_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();
				
				//Publish the category for each site
				ClickPublishButtonForEachSite();

				//Validate that each site status display was updated
				foreach (var siteName in _siteNames)
				{
					Assert.AreEqual(PublishStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {PublishStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void CategorySitePublishStatus_UnpublishCategory_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();

				//Publish the category for each site
				ClickPublishButtonForEachSite();

				//Unpublish the category for each site
				ClickPublishButtonForEachSite();

				//Validate that each site shows the correct status
				foreach (var siteName in _siteNames)
				{
					Assert.AreEqual(UnpublishStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {UnpublishStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void CategorySitePublishStatus_PublishCategoryCancelled_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();
				
				foreach (var siteName in _siteNames)
				{
					//Click the publish button for the site card
					BasePage.GetSitePublishButtonBySite(siteName).Click();

					//Cancel publish action
					_categoryDetailsPage.ButtonConfirmationDialogCancel.Click();

					Assert.AreEqual(IncompleteStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {IncompleteStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void CategorySitePublishStatus_UnpublishCategoryCancelled_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();

				ClickPublishButtonForEachSite();

				foreach (var siteName in _siteNames)
				{
					//Click to unpublish
					BasePage.GetSitePublishButtonBySite(siteName).Click();

					//Cancel the unpublish action
					_categoryDetailsPage.ButtonConfirmationDialogCancel.Click();

					Assert.AreEqual(PublishStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {PublishStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		public void CategorySitePublishStatus_PublishCategoryCancelledFromBeingUnpublished_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var categoryId = SetupDataAndPage();

				//Publish the category for each site
				ClickPublishButtonForEachSite();

				//Unpublish the category for each site
				ClickPublishButtonForEachSite();

				foreach (var siteName in _siteNames)
				{
					//Click to publish
					BasePage.GetSitePublishButtonBySite(siteName).Click();

					//Cancel the publish action
					_categoryDetailsPage.ButtonConfirmationDialogCancel.Click();

					Assert.AreEqual(UnpublishStatusName,
						BasePage.GetSitePublishStatusBySite(siteName).GetText(),
						$"Publish status for {siteName} should've been {UnpublishStatusName}");
				}

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		private string SetupDataAndPage()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var categoryPostRequest = RequestUtility.GetCategoryInsertRequest(categoryName);

			//Setup data by creating a new category
			var categoryId = CategoryAdminApiService.PostCategory(categoryPostRequest).Result.CategoryId.ToString();

			//Navigate to the page for the new category
			BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId, 5000);

			return categoryId;
		}

		private void ClickPublishButtonForEachSite()
		{
			foreach (var siteName in _siteNames)
			{
				//Click the publish button for the site card
				BasePage.GetSitePublishButtonBySite(siteName).Click();

				//Confirm the action
				_categoryDetailsPage.ButtonConfirmationDialogAction.Click();

				//Wait for action to complete
				Thread.Sleep(100);
				BrowserUtility.WaitForElementToDisappear($"mat-progress-bar-publish-status-{siteName}");
			}
		}
	}
}
