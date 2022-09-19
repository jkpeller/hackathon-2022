using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteCategoryMappingPage
{
	[TestFixture]
	[NonParallelizable]
	public class TableTests : BaseTest
	{
		private Model.Pages.SiteCategoryMappingPage _page;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteCategoryMappingPage();
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void SiteCategoriesMappingTable_VerifyDefaults_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//all columns are present in the table and contain the correct text
				Assert.Multiple(() =>
				{
					Assert.AreEqual(Model.Pages.SiteCategoryMappingPage.ColumnNameString.CategoryName, _page.TableHeaderCategoryName.GetText());
					Assert.AreEqual(Model.Pages.SiteCategoryMappingPage.ColumnNameString.PublishStatus, _page.TableHeaderPublishStatus.GetText());
					Assert.AreEqual(Model.Pages.SiteCategoryMappingPage.ColumnNameString.Site, _page.TableHeaderSite.GetText());
					Assert.AreEqual(Model.Pages.SiteCategoryMappingPage.ColumnNameString.Url, _page.TableHeaderUrl.GetText());
					Assert.AreEqual(Model.Pages.SiteCategoryMappingPage.ColumnNameString.GlobalCategory, _page.TableHeaderGlobalCategory.GetText());
				});

				//the default items per page is set to 50
				_page.PaginatorDisplayRange.HoverOver();
				Assert.AreEqual(PagingUtility.DefaultPageSize.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void SiteCategoriesMappingTable_ChangeItemsPerPage_Succeeds()
		{
			const int updatedItemsPerPageValue = 10;
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//click items per page drop down
				_page.PaginatorDropDownItemsPerPage.Click();
				Log("Click the table paginator items per page drop down.");

				//click a different value
				BasePage.GetTablePaginatorItemsPerPageOptionByValue(1).ClickAndWaitForPageToLoad();
				Log("Select the value for 10 items per page.");

				//the items per page is set to 10
				_page.PaginatorDisplayRange.HoverOver();
				Assert.AreEqual(updatedItemsPerPageValue.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
				Log("Assert that the items per page drop down now shows 10.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void SiteCategoriesMappingTable_NavigatePages_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				SetUpPage();

				//select to show 10 items per page
				_page.PaginatorDropDownItemsPerPage.Click();
				BasePage.GetTablePaginatorItemsPerPageOptionByValue(1).ClickAndWaitForPageToLoad();
				Log("Updated items per page selection.");

				//validate the paginator range display based on the table default
				Assert.IsTrue(_page.PaginatorDisplayRange.GetText().Contains("1 – 10"));
				Log($"Items per page display reflects showing the 1st page of results. Text: {_page.PaginatorDisplayRange.GetText()}");

				//click the next page button and validate that the page display updates appropriately
				_page.PaginatorButtonNextPage.ClickAndWaitForPageToLoad();
				Log("Click the next page button in the paginator.");
				Assert.IsTrue(_page.PaginatorDisplayRange.GetText().Contains("11 –"));
				Log($"Items per page display reflects showing the 2nd page of results. Text: {_page.PaginatorDisplayRange.GetText()}");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void SiteCategoriesMappingTable_FilterWithNoResults_Succeeds()
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
		public void SiteCategoryMappingTable_SortColumnsAscending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				SetUpPage();

				//Validate that the category name column is sorted
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name);
				Log("Category name column is sorted.");

				//Validate that the global category column is sorted
				_page.TableHeaderGlobalCategory.ClickAndWaitForPageToLoad();
				Log("Clicked the global category column.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.GlobalCategory);
				Log("Global category column is sorted.");

				//Validate that the publish status column is sorted
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the publish status column.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.PublishStatus);
				Log("Publish status column is sorted.");

				//Validate that the site column is sorted
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				Log("Clicked the site column.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Site);
				Log("Site column is sorted.");

				//Validate that the url column is sorted
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				Log("Clicked the url column.");
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Url);
				Log("Url column is sorted.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void SiteCategoryMappingTable_SortColumnsDescending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				SetUpPage();

				//Validate that the category name column is sorted
				_page.TableHeaderCategoryName.ClickAndWaitForPageToLoad();
				Log("Clicked the category name column.");
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name, SortDirectionType.Descending);
				Log("Category name column is sorted.");

				//Validate that the publish status column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the publish status column twice.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.PublishStatus, SortDirectionType.Descending);
				Log("Publish status column is sorted.");

				//Validate that the site column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				Log("Clicked the site column twice.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Site, SortDirectionType.Descending);
				Log("Site column is sorted.");

				//Validate that the url column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				Log("Clicked the url column twice.");
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Url, SortDirectionType.Descending);
				Log("Url column is sorted.");

				//Validate that the global category column is sorted
				_page.ButtonApplyFilters.HoverOver();
				_page.TableHeaderGlobalCategory.ClickAndWaitForPageToLoad();
				_page.TableHeaderGlobalCategory.ClickAndWaitForPageToLoad();
				Log("Clicked the global category column twice.");
				AssertColumnIsSorted(Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.GlobalCategory, SortDirectionType.Descending);
				Log("Global category column is sorted.");
			});
		}

		private void SetUpPage()
		{
			OpenPage(_page);

			//Filter by mapped mapping status
			_page.SelectMappingStatus.Click();
			Log("Clicked into the mapping status filter box.");
			_page.SelectOptionMappingStatusMapped.Check();
			Log("Checked the mapped mapping status value.");
			_page.SelectOptionMappingStatusUnmapped.Uncheck();
			Log("Unchecked the unmapped mapping status value.");
			_page.SelectOptionMappingStatusUnmapped.SendKeys();

			//Filter by all publish statuses
			_page.SelectPublishStatus.Click();
			Log("Clicked into the publish status filter box.");
			_page.SelectOptionPublishStatusUnpublished.Click();
			Log("Checked the unpublished status value.");
			_page.SelectOptionPublishStatusUnpublished.SendKeys();

			//Apply filters
			_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
		}
	}
}
