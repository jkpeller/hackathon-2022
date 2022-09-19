using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteProductMappingPage
{
	[TestFixture]
	[Ignore("Mapping capabilities were disabled on GCT-1315")]
	public class TableTests : BaseTest
	{
		private Model.Pages.SiteProductMappingPage _page;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteProductMappingPage();
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void SiteProductsMappingTable_VerifyDefaults_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//all columns are present in the table and contain the correct text
				Assert.Multiple(() =>
				{
					Assert.AreEqual(Model.Pages.SiteProductMappingPage.ColumnNameString.ProductName, _page.TableHeaderProductName.GetText());
					Assert.AreEqual(Model.Pages.SiteProductMappingPage.ColumnNameString.PublishStatus, _page.TableHeaderPublishStatus.GetText());
					Assert.AreEqual(Model.Pages.SiteProductMappingPage.ColumnNameString.Site, _page.TableHeaderSite.GetText());
					Assert.AreEqual(Model.Pages.SiteProductMappingPage.ColumnNameString.Url, _page.TableHeaderUrl.GetText());
					Assert.AreEqual(Model.Pages.SiteProductMappingPage.ColumnNameString.GlobalProduct, _page.TableHeaderGlobalProduct.GetText());
				});

				//the default items per page is set to 50
				_page.PaginatorDisplayRange.HoverOver();
				Assert.AreEqual(PagingUtility.DefaultPageSize.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void SiteProductsMappingTable_ChangeItemsPerPage_Succeeds()
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
		public void SiteProductsMappingTable_NavigatePages_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//select to show 10 items per page
				_page.PaginatorDropDownItemsPerPage.Click();
				BasePage.GetTablePaginatorItemsPerPageOptionByValue(1).ClickAndWaitForPageToLoad();
				Log("Updated items per page selection.");

				//validate the paginator range display based on the table default
				Assert.IsTrue(_page.PaginatorDisplayRange.GetText().Contains("1 – 10"));
				Log("Items per page display reflects showing the 1st page of results.");

				//click the next page button and validate that the page display updates appropriately
				_page.PaginatorButtonNextPage.ClickAndWaitForPageToLoad();
				Log("Click the next page button in the paginator.");
				Assert.IsTrue(_page.PaginatorDisplayRange.GetText().Contains("11 –"));
				Log("Items per page display reflects showing the 2nd page of results.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void SiteProductsMappingTable_SortColumnsAscending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Apply filters button clicked.");

				//TODO: Selenium is trimming leading spaces. We need a solution for that before we can validate ascending order sorting by Product Name

				//click the publish status column and validate it sorts correctly
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				AssertColumnIsSorted(Model.Pages.SiteProductMappingPage.ColumnNameSelector.PublishStatus);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.PublishStatus} sorted correctly.");

				//click the site column and validate it sorts correctly
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				AssertColumnIsSorted(Model.Pages.SiteProductMappingPage.ColumnNameSelector.Site);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.Site} sorted correctly.");

				//click the url column and validate it sorts correctly
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteProductMappingPage.ColumnNameSelector.Url);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.Url} sorted correctly.");

				//TODO: Selenium is trimming leading spaces. We need a solution for that before we can validate ascending order sorting by Global Product
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void SiteProductsMappingTable_SortColumnsDescending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//filter on the mapped mapping status value only in the mapping status filter drop down so the Global Product column is populated
				_page.SelectMappingStatus.Click();
				Log("Mapping status filter drop down opened.");
				_page.SelectOptionMappingStatusUnmapped.Uncheck();
				Log("Unmapped mapping status value was deselected");
				_page.SelectOptionMappingStatusMapped.Check();
				Log("Mapped mapping status value was selected");
				_page.SelectOptionMappingStatusMapped.SendKeys(hoverOver: false);

				//click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Apply filters button clicked.");

				//click the product name column and validate it sorts correctly
				_page.TableHeaderProductName.ClickAndWaitForPageToLoad();
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName, SortDirectionType.Descending);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName} sorted correctly.");

				//click the global product column twice and validate it sorts correctly
				_page.TableHeaderGlobalProduct.ClickAndWaitForPageToLoad();
				_page.TableHeaderGlobalProduct.ClickAndWaitForPageToLoad();
				AssertColumnIsSorted(Model.Pages.SiteProductMappingPage.ColumnNameSelector.GlobalProduct, SortDirectionType.Descending);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.GlobalProduct} sorted correctly.");

				//click the publish status column twice and validate it sorts correctly
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				_page.TableHeaderPublishStatus.ClickAndWaitForPageToLoad();
				AssertColumnIsSorted(Model.Pages.SiteProductMappingPage.ColumnNameSelector.PublishStatus, SortDirectionType.Descending);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.PublishStatus} sorted correctly.");

				//click the site column twice and validate it sorts correctly
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				_page.TableHeaderSite.ClickAndWaitForPageToLoad();
				AssertColumnIsSorted(Model.Pages.SiteProductMappingPage.ColumnNameSelector.Site, SortDirectionType.Descending);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.Site} sorted correctly.");

				//click the url column twice and validate it sorts correctly
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				_page.TableHeaderUrl.ClickAndWaitForPageToLoad();
				AssertColumnIsSortedWithoutDashes(Model.Pages.SiteProductMappingPage.ColumnNameSelector.Url, SortDirectionType.Descending);
				Log($"{Model.Pages.SiteProductMappingPage.ColumnNameSelector.Url} sorted correctly.");
			});
		}

		//TODO - when we have elastic search so we can search on site products mapping page, add the no results test in here
	}
}
