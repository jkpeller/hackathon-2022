using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductsPage
{
	[TestFixture]
	public class TableTests : BaseTest
	{
		private Model.Pages.ProductsPage _page;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.ProductsPage();
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void GlobalProductCatalogTable_VerifyDefaults_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();
				Log($"Open the page. Page: {_page.GetType()}");

				//all columns are present in the table and contain the correct text
				Assert.Multiple(() =>
				{
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.ProductName, _page.TableHeaderProductName.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.Status, _page.TableHeaderStatus.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.Updated, _page.TableHeaderModifiedOnUtc.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.CapterraCount, _page.TableHeaderCapterra.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.SoftwareAdviceCount, _page.TableHeaderSoftwareAdvice.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.GetappCount, _page.TableHeaderGetapp.GetText());
					Assert.AreEqual(Model.Pages.ProductsPage.ColumnNameString.TotalCount, _page.TableHeaderTotalProductCount.GetText());
				});

				//the default items per page is set to 50
				_page.PaginatorDisplayRange.Click();
				Assert.AreEqual(PagingUtility.DefaultPageSize.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void GlobalProductCatalogTable_ChangeItemsPerPage_Succeeds()
		{
			const int updatedItemsPerPageValue = 10;
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();
				Log($"Open the page. Page: {_page.GetType()}");

				//click items per page drop down
				_page.PaginatorDropDownItemsPerPage.Click();
				Log("Click the table paginator items per page drop down.");

				//click a different value
				BasePage.GetTablePaginatorItemsPerPageOptionByValue(1).ClickAndWaitForPageToLoad();
				Log("Select the value for 10 items per page.");

				//the items per page is set to 10
				_page.PaginatorDisplayRange.Click();
				Assert.AreEqual(updatedItemsPerPageValue.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
				Log("Assert that the items per page drop down now shows 10.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void GlobalProductCatalogTable_NavigatePages_Succeeds()
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
		[Category("Product")]
		public void GlobalProductTable_SortColumnsAscending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();

				//TODO: Selenium is trimming leading spaces. We need a solution for that before we can validate ascending order sorting by Product Name

				//Validate that the publish status column is sorted
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the status column.");
				AssertColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Status);
				Log("Status column is sorted.");

				//Validate that the modified on utc column is sorted
				_page.TableHeaderModifiedOnUtc.ClickAndWaitForPageToLoad();
				Log("Clicked the modified on utc column.");
				AssertColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Updated);
				Log("Modified on utc column is sorted.");

				//Validate that the capterra column is sorted
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				Log("Clicked the capterra column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Capterra);
				Log("Capterra column is sorted.");

				//Validate that the software advice column is sorted
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				Log("Clicked the software advice column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.SoftwareAdvice);
				Log("Software Advice column is sorted.");

				//Validate that the getapp column is sorted
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				Log("Clicked the getapp column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Getapp);
				Log("Getapp column is sorted.");

				//Validate that the total column is sorted
				_page.TableHeaderTotalProductCount.ClickAndWaitForPageToLoad();
				Log("Clicked the total column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Total);
				Log("Total column is sorted.");
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		[Ignore("Sorting cannot be tested due to postgres db collation")]
		public void GlobalCategoryTable_SortColumnsDescending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();

				//Validate that the product name column is sorted
				_page.TableHeaderProductName.ClickAndWaitForPageToLoad();
				Log("Clicked the name column.");
				AssertColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Name, SortDirectionType.Descending);
				Log("Category name column is sorted.");

				//Validate that the publish status column is sorted
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the status column.");
				AssertColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Status, SortDirectionType.Descending);
				Log("Status column is sorted.");

				//Validate that the capterra column is sorted
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				_page.TableHeaderCapterra.ClickAndWaitForPageToLoad();
				Log("Clicked the capterra column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Capterra, SortDirectionType.Descending);
				Log("Capterra column is sorted.");

				//Validate that the software advice column is sorted
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				_page.TableHeaderSoftwareAdvice.ClickAndWaitForPageToLoad();
				Log("Clicked the software advice column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.SoftwareAdvice, SortDirectionType.Descending);
				Log("Software Advice column is sorted.");

				//Validate that the getapp column is sorted
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				_page.TableHeaderGetapp.ClickAndWaitForPageToLoad();
				Log("Clicked the getapp column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Getapp, SortDirectionType.Descending);
				Log("Getapp column is sorted.");

				//Validate that the total column is sorted
				_page.TableHeaderTotalProductCount.ClickAndWaitForPageToLoad();
				_page.TableHeaderTotalProductCount.ClickAndWaitForPageToLoad();
				Log("Clicked the total column.");
				AssertNumericColumnIsSorted(Model.Pages.ProductsPage.ColumnNameSelector.Total, SortDirectionType.Descending);
				Log("Total column is sorted.");
			});
		}

		private void OpenPageAndFilterAllStatuses()
		{
			OpenPage(_page);

			//Filter on all status values and apply filters
			_page.SelectProductStatus.Click();
			Log("Clicked into the status filter box.");
			_page.SelectStatusArchived.Check();
			Log("Selected the archived status value.");
			_page.SelectStatusArchived.SendKeys();
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");
		}
	}
}
