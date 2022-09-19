using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
	[TestFixture]
	public class NewListingRequestTableTests : BaseTest
	{
		private Model.Pages.NewListingRequestPage _page;
		private string _companyName;
		private string _companyWebsiteUrl;

		public NewListingRequestTableTests() : base(nameof(NewListingRequestTableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.NewListingRequestPage();
			_companyName = RequestUtility.GetRandomString(10);
			_companyWebsiteUrl = $"https://test.com/{_companyName}";
		}

		[Test]
		[Category("Readonly")]
		[Category("ListingRequest")]
		public void NewListingRequestTable_ValidateColumnOrder_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();

				//validate that the columns are shown in the correct order
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Status, BasePage.GetTableColumnByColumnNumber(1).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Received, BasePage.GetTableColumnByColumnNumber(2).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Company, BasePage.GetTableColumnByColumnNumber(3).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Product, BasePage.GetTableColumnByColumnNumber(4).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Category, BasePage.GetTableColumnByColumnNumber(5).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.HqCountry, BasePage.GetTableColumnByColumnNumber(6).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Source, BasePage.GetTableColumnByColumnNumber(7).GetText());
				Assert.AreEqual(Model.Pages.NewListingRequestPage.ColumnNameString.Updated, BasePage.GetTableColumnByColumnNumber(8).GetText());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateRequestInformation_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();

				//set up listing request
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, isCreateVendor: true);

				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				
				//validate that the request is the first result record in the table
				Assert.AreEqual("New", BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());
				Assert.AreEqual(result.CompanyName, BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Company).GetText());
				Assert.AreEqual(result.ProductName, BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Product).GetText());
				Assert.AreEqual(result.ProductProposedCategoryName, BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Category).GetText());
				Assert.AreEqual(result.CompanyCountryCode, BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry).GetText());
				Assert.AreEqual("Sugar User", BasePage.GetTableCellByRowNumberAndColumnName
					(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Source).GetText());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateDefaultSort_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the page
				_page.OpenPage();

				//set up listing requests
				var companyPrefix = RequestUtility.GetRandomString(5);
				var companyName1 = $"{companyPrefix}{RequestUtility.GetRandomString(5)}";
				var companyName2 = $"{companyPrefix}{RequestUtility.GetRandomString(5)}";
				var companyName3 = $"{companyPrefix}{RequestUtility.GetRandomString(5)}";
				var companyWebsiteUrl1 = $"https://{companyName1}.com";
				var companyWebsiteUrl2 = $"https://{companyName2}.com";
				var companyWebsiteUrl3 = $"https://{companyName3}.com";
				var company1 = SetupNewListingRequestWithAllValidFields(companyName1, companyWebsiteUrl1);
				var company2 = SetupNewListingRequestWithAllValidFields(companyName2, companyWebsiteUrl2, isCreateVendor: true);
				var company3 = SetupNewListingRequestWithAllValidFields(companyName3, companyWebsiteUrl3, isCreateVendor: true);
				
				//filter to the companies
				_page.InputCompanyProductNameFilter.SendKeys(companyPrefix);
				_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();

				//validate that the requests are in the correct order in the table, Sugar User first followed by all other sources and received descending
				Assert.AreEqual("New", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());
				Assert.AreEqual(company3.CompanyName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Company).GetText());
				Assert.AreEqual(company3.ProductName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Product).GetText());
				Assert.AreEqual(company3.ProductProposedCategoryName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Category).GetText());
				Assert.AreEqual(company3.CompanyCountryCode, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry).GetText());
				Assert.AreEqual("Sugar User", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.NewListingRequestPage.ColumnNameSelector.Source).GetText());

				Assert.AreEqual("New", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());
				Assert.AreEqual(company2.CompanyName, BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.Company).GetText());
				Assert.AreEqual(company2.ProductName, BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.Product).GetText());
				Assert.AreEqual(company2.ProductProposedCategoryName, BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.Category).GetText());
				Assert.AreEqual(company2.CompanyCountryCode, BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry).GetText());
				Assert.AreEqual("Sugar User", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.NewListingRequestPage.ColumnNameSelector.Source).GetText());

				Assert.AreEqual("New", BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.Status).GetText());
				Assert.AreEqual(company1.CompanyName, BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.Company).GetText());
				Assert.AreEqual(company1.ProductName, BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.Product).GetText());
				Assert.AreEqual(company1.ProductProposedCategoryName, BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.Category).GetText());
				Assert.AreEqual(company1.CompanyCountryCode, BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry).GetText());
				Assert.AreEqual("Web", BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.NewListingRequestPage.ColumnNameSelector.Source).GetText());
			});
		}


		[Test]
		[Category("ListingRequest")]
		public void NewListingRequestTable_SortColumnsAscending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();
				//Validate that the publish status column is sorted
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the status column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Status);
				Log("Status column is sorted.");

				//Validate that the Received column is sorted
				_page.TableHeaderReceived.ClickAndWaitForPageToLoad();
				_page.TableHeaderReceived.ClickAndWaitForPageToLoad();
				Log("Clicked the Received column.");
				AssertColumnDateTypeIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Received);
				Log("Receivedcolumn is sorted.");

				//Validate that the Company column is sorted
				_page.TableHeaderVendor.ClickAndWaitForPageToLoad();
				Log("Clicked the Company column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Company);
				Log("Company column is sorted.");

				//Validate that the Product column is sorted
				_page.TableHeaderProduct.ClickAndWaitForPageToLoad();
				Log("Clicked the Product column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Product);
				Log("Product column is sorted.");

				//Validate that the Category column is sorted
				_page.TableHeaderCategory.ClickAndWaitForPageToLoad();
				Log("Clicked the Category column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Category);
				Log("Category column is sorted.");

				//Validate that the HqCountry column is sorted
				_page.TableHeaderHqCountry.ClickAndWaitForPageToLoad();
				Log("Clicked the HqCountry column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry);
				Log("HqCountry column is sorted.");

				//Validate that the Source column is sorted
				_page.TableHeaderSource.ClickAndWaitForPageToLoad();
				Log("Clicked the Source column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Source);
				Log("Source column is sorted.");

				//Validate that the Updated column is sorted
				_page.TableHeaderUpdated.ClickAndWaitForPageToLoad();
				_page.TableHeaderUpdated.ClickAndWaitForPageToLoad();
				Log("Clicked the Updated column.");
				AssertColumnDateTypeIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Updated);
				Log("Source Updated is sorted.");

			});
		}

		[Test]
		[Category("ListingRequest")]
		public void NewListingRequestTable_SortColumnsDescending_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPageAndFilterAllStatuses();
				//Validate that the Status name column is sorted
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				_page.TableHeaderStatus.ClickAndWaitForPageToLoad();
				Log("Clicked the Status column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Status, SortDirectionType.Descending);
				Log("Name Status is sorted.");

				//Validate that the Received status column is sorted
				_page.TableHeaderReceived.ClickAndWaitForPageToLoad();
				Log("Clicked the Received column.");
				AssertColumnDateTypeIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Received, SortDirectionType.Descending);
				Log("Received column is sorted.");

				//Validate that the Company column is sorted
				_page.TableHeaderVendor.ClickAndWaitForPageToLoad();
				_page.TableHeaderVendor.ClickAndWaitForPageToLoad();
				Log("Clicked the Company column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Company, SortDirectionType.Descending);
				Log("Company column is sorted.");

				//Validate that the Product column is sorted
				_page.TableHeaderProduct.ClickAndWaitForPageToLoad();
				_page.TableHeaderProduct.ClickAndWaitForPageToLoad();
				Log("Clicked the Product column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Product, SortDirectionType.Descending);
				Log("Software Product is sorted.");

				//Validate that the Category column is sorted
				_page.TableHeaderCategory.ClickAndWaitForPageToLoad();
				_page.TableHeaderCategory.ClickAndWaitForPageToLoad();
				Log("Clicked the Category column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Category, SortDirectionType.Descending);
				Log("Category column is sorted.");

				//Validate that the HqCountry column is sorted
				_page.TableHeaderHqCountry.ClickAndWaitForPageToLoad();
				_page.TableHeaderHqCountry.ClickAndWaitForPageToLoad();
				Log("Clicked the HqCountry column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry, SortDirectionType.Descending);
				Log("HqCountry column is sorted.");

				//Validate that the Source column is sorted
				_page.TableHeaderSource.ClickAndWaitForPageToLoad();
				_page.TableHeaderSource.ClickAndWaitForPageToLoad();
				Log("Clicked the Source column.");
				AssertColumnIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Source, SortDirectionType.Descending);
				Log("Source column is sorted.");

				//Validate that the Updated column is sorted
				_page.TableHeaderUpdated.ClickAndWaitForPageToLoad();
				Log("Clicked the Updated column.");
				AssertColumnDateTypeIsSorted(Model.Pages.NewListingRequestPage.ColumnNameSelector.Updated, SortDirectionType.Descending);
				Log("Updated column is sorted.");
			});
		}
		private void OpenPageAndFilterAllStatuses()
		{
			OpenPage(_page);

			//Filter on all status values and apply filters
			_page.SelectListingRequestStatus.Click();
			Log("Clicked into the status filter box.");

			_page.SelectStatusApproved.Check();
			Log("Selected the Approved status value.");
			_page.SelectStatusDenied.Check();
			Log("Selected the Denied status value.");
			_page.SelectStatusNew.SendKeys();
			Log("Selected the New status value.");

			Log("Selected the Under review status value.");
			_page.SelectStatusUnderReview.SendKeys();

			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");
		}


	}
}
