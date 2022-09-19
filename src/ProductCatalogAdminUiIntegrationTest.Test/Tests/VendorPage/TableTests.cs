using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	public class TableTests : BaseTest
	{
		private Model.Pages.VendorPage _page;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.VendorPage();
		}

		[Test]
		[Category("Readonly")]
		[Category("Vendor")]
		public void VendorTable_VerifyDefaults_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();
				Log($"Open the page. Page: {_page.GetType()}");

				//all columns are present in the table and contain the correct text
				Assert.Multiple(() =>
				{
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.Name, _page.TableHeaderName.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.Products, _page.TableHeaderProducts.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.Categories, _page.TableHeaderCategories.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.Website, _page.TableHeaderWebsite.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.Status, _page.TableHeaderStatus.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.ModifiedOnUtc, _page.TableHeaderModifiedOnUtc.GetText());
					Assert.AreEqual(Model.Pages.VendorPage.ColumnNameString.CreatedOnUtc, _page.TableHeaderCreatedOnUtc.GetText());
				});

				//the default items per page is set to 50
				_page.PaginatorDropDownItemsPerPage.HoverOver();
				Assert.AreEqual(PagingUtility.DefaultPageSize.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Vendor")]
		public void VendorTable_ChangeItemsPerPage_Succeeds()
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
				_page.PaginatorDropDownItemsPerPage.HoverOver();
				Assert.AreEqual(updatedItemsPerPageValue.ToString(), _page.PaginatorDropDownItemsPerPage.GetText());
				Log("Assert that the items per page drop down now shows 10.");
			});
		}

		//TODO: when we can create enough vendors to populate more than one page in the UI, test the paging
	}
}
