using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteCategoryMappingPage
{
	[TestFixture]
	public class LayoutTests : BaseTest
	{
		private Model.Pages.SiteCategoryMappingPage _page;
		private string _sourceSiteCategoryId;
		private SourceSiteCategorySingleSaveRequest _sourceSiteCategoryPostRequest;
		private string _sourceSiteCategoryName;
		private string _categoryName;
		private CategoryInsertRequest _categoryPostRequest;

		public LayoutTests() : base(nameof(LayoutTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteCategoryMappingPage();
			_sourceSiteCategoryId = RequestUtility.GetUniqueId();
			_sourceSiteCategoryPostRequest = RequestUtility.GetSourceSiteCategorySaveRequest(_sourceSiteCategoryId);
			_sourceSiteCategoryName = _sourceSiteCategoryPostRequest.Name;
			_categoryName = RequestUtility.GetRandomString(9);
			_categoryPostRequest = RequestUtility.GetCategoryInsertRequest(_categoryName);
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void ValidateLayout_PageTitleAndNavLink_Succeeds()
		{
			const string expectedTitle = "Category Mapping";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Assert that the name of the page is correct
				Assert.AreEqual(expectedTitle, _page.PageTitle.GetText());
				Log($"Validated the page title. {_page.PageTitle.GetText()}");

				//Assert that the page name is correct in the nav bar
				Assert.IsTrue(_page.LinkSideNavCategoryMapping.GetHref().Contains("/site-categories"));
			});
		}

		[Test]
		[Category("Category")]
		public void ValidateLayout_SourceSiteCategoryIdInCategoryColumn_Succeeds()
		{
			var categoryText = $"{_sourceSiteCategoryName} ({_sourceSiteCategoryId})";
			ExecuteTimedTest(() =>
			{
				//Setup
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);

				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);
				Log("Opened the page and filters on the new category.");

				//Assert that the returned record contains the correct name and id
				Assert.AreEqual(categoryText, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).GetText());

				//cleanup the site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void ValidateLayout_LocationOfGlobalCategoryColumn_Succeeds()
		{
			const string expectedColumnName = "Global Category";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Assert that the global category column is the second one in the table
				Assert.AreEqual(expectedColumnName, BasePage.GetTableColumnByColumnNumber(2).GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void ValidateLayout_MappingDialogTitle_Succeeds()
		{
			const string expectedDialogTitleText = "Assign a Global Category";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//Click to add a mapping for any row in the table
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).Click();
				Log("Click the site category mapping button for the result record row.");

				//Assert that the returned record contains the correct name and id
				Assert.AreEqual(expectedDialogTitleText, _page.TitleMappingDialog.GetText());
			});
		}

		[Test]
		[Category("Category")]
		public void ValidateLayout_MappingDialogSiteAndCategoryDisplay_Succeeds()
		{
			const string expectedCategorySiteText = "Capterra";
			var categoryNameText = $"{_sourceSiteCategoryName}";
			var categoryIdText = $"({_sourceSiteCategoryId})";
			ExecuteTimedTest(() =>
			{
				//Setup
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);

				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);

				//Click to add a mapping for any row in the table
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).Click();
				Log("Click the site category mapping button for the result record row.");

				//Assert that the site name and category name/id are shown correctly
				Assert.AreEqual(expectedCategorySiteText, _page.DivMappingDialogSiteName.GetText());
				Assert.AreEqual(categoryNameText, _page.SpanMappingDialogCategoryName.GetText());
				Assert.AreEqual(categoryIdText, _page.SpanMappingDialogCategoryId.GetText());

				//cleanup the site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
			});
		}

		[Test]
		[Category("Category")]
		public void ValidateLayout_MappingDialogGlobalDisplay_Succeeds()
		{
			const string expectedGlobalSectionText = "Global (UPC)";
			ExecuteTimedTest(() =>
			{
				//Setup
				IntegrationApiService.PostSourceSiteCategory(_sourceSiteCategoryPostRequest);
				OpenPageAndFilterBySiteCategoryNameAndAllFilters(_page, _sourceSiteCategoryName);
				var categoryId = CategoryAdminApiService.PostCategory(_categoryPostRequest).Result.CategoryId;

				//Click to add a mapping for any row in the table
				BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
				Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).Click();
				Log("Click the site category mapping button for the result record row.");

				//Search for the category
				_page.InputMappingDialogGlobalCategorySearch.SendKeys(_categoryName, sendEscape: false);
				Log($"Typed {_categoryName} into the global category search box.");

				//Click on the category suggestion
				Model.Pages.SiteCategoryMappingPage.GetModalGlobalCategorySuggestionByName(_categoryName.ToLower()).Click(false);
				Log($"Click the suggestion with text '{_categoryName}' from the category mapping modal search box.");

				//Assert that the site name and category name/id are shown correctly
				Assert.AreEqual(expectedGlobalSectionText, _page.DivMappingDialogGlobalHeader.GetText());
				Assert.AreEqual(_categoryName, _page.SpanMappingDialogGlobalCategoryName.GetText().Substring(0, 9));

				//Cleanup the global and site category 
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _sourceSiteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		private void OpenPageAndFilterBySiteCategoryNameAndAllFilters(BasePage page, string sourceSiteCategoryName)
		{
			OpenPage(page);

			//type the name of the new site category in the category name filter box
			page.InputFilterCategoryName.SendKeys(sourceSiteCategoryName);

			//select all options from the mapping status filter box
			page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened");
			page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			page.SelectOptionMappingStatusMapped.SendKeys(hoverOver: false);
			Log("Escaped out of the mapping status box");

			//select all options from the publish status filter box
			page.SelectPublishStatus.Click(false);
			Log("Publish status filter drop down clicked.");
			page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			page.SelectOptionPublishStatusUnpublished.SendKeys(hoverOver: false);
			Log("Escaped out of the publish status filter box");

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
		}
	}
}
