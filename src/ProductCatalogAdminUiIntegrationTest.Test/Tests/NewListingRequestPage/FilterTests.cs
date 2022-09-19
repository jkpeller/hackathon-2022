using NUnit.Framework;
using OpenQA.Selenium;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
	[Category("ListingRequest")]
	public class FilterTests : BaseTest
	{
		private Model.Pages.NewListingRequestPage _page;
		private int _categoryId;
		private string _categoryName;
		private string _listingRequestId;
		private string _updatedProductName;
		private string _updatedCompanyName;
		private string _companyName;
		private string _companyWebsiteUrl;
		private string _countryName = "United States of America";
		private string _countryCode = "US";		
		private ListingRequestUpdateRequest _putRequest;

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.NewListingRequestPage();
			_categoryName = RequestUtility.GetRandomString(10);
			_updatedProductName = RequestUtility.GetRandomString(9);
			_updatedCompanyName = RequestUtility.GetRandomString(9);
			_companyName = RequestUtility.GetRandomString(9);
			_companyWebsiteUrl = $"https://www.{_updatedCompanyName}.com/{_companyName}";			

			//open the page
			_page.OpenPage();
		}

		[Test]
		[Category("Readonly")]
		public void FilterNewListingRequest_ValidateDefaultFilters_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				Thread.Sleep(3000);
				_page.SelectListingRequestStatus.Click();			

				//assert that the new and under review options are selected by default				
				Assert.IsTrue(_page.ListingRequestFilterStatusNew.GetText().Contains("New"), "Should SelectListingRequestStatus contain 'New'");				
				Assert.IsTrue(_page.ListingRequestFilterStatusUnderReview.GetText().Contains("Under Review"), "Should SelectListingRequestStatus contain 'Under Review'");
				Assert.IsTrue(string.IsNullOrEmpty(_page.InputCompanyProductNameFilter.GetText()), "Should InputCompanyProductNameFilter be empty");
			});
		}

		[Test]
		[Category("Readonly")]
		public void FilterNewListingRequest_ByAllStatuses_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//open the select status box
				_page.SelectListingRequestStatus.Click();
				Log("Clicked to open the select listing request status box");

				//select the two options that aren't selected by default
				_page.ListingRequestFilterStatusApproved.Click();
				Log("Selected the Approved status option");
				_page.ListingRequestFilterStatusDenied.Click();
				Log("Selected the Denied status option");

				//This step just sends the Escape key to one of the options to close the select box
				_page.ListingRequestFilterStatusApproved.SendKeys();

				//assert the status filter box now contains "All"
				Assert.IsTrue(_page.SelectListingRequestStatus.GetText().Contains("All"));
			});
		}

		[Test]
		[Category("Readonly")]
		public void FilterNewListingRequest_DisplayValidation_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.InputCompanyProductNameFilter.SendKeys("ab", true);

				//open the select status box
				_page.SelectListingRequestStatus.Click();
				Log("Clicked to open the select listing request status box");

				//deselect all values
				_page.ListingRequestFilterStatusNew.Click();
				Log("Deselected the New status option");
				_page.ListingRequestFilterStatusUnderReview.Click();
				Log("Deselected the Under Review status option");
				_page.ListingRequestFilterStatusNew.SendKeys();

				//assert that the apply filters button can't be clicked and an error message is present
				Assert.IsTrue(_page.ErrorCompanyProductNameFilter.ExistsInPage());
				Assert.IsTrue(_page.ErrorListingRequestFilterStatus.ExistsInPage());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisabled());
			});
		}

		[Test]
		public void FilterNewListingRequest_ByNewStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup a new listing request
				SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the select status box
				_page.SelectListingRequestStatus.Click();
				Log("Clicked to open the select listing request status box");

				//deselect the Under Review option so that only New is selected
				_page.ListingRequestFilterStatusUnderReview.Click();
				Log("Deselected the Under Review status option");
				_page.ListingRequestFilterStatusUnderReview.SendKeys();

				//click apply filters
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked the apply filters button");

				//assert that all displayed options have the New value in the status column
				for (var i = 1; i <= _page.GetItemsPerPage(); i++)
				{
					const string columnName = Model.Pages.NewListingRequestPage.ColumnNameSelector.Status;
					var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
					Log($"Validate that the cell text equals '{nameof(ListingRequestStatusType.New)}'. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.AreEqual(nameof(ListingRequestStatusType.New), cellText);
				}
			});
		}

		[Test]
		public void FilterNewListingRequest_ByDeniedStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup a new listing request that's been updated to be denied
				SetupDeniedListingRequest();

				//open the select status box
				_page.SelectListingRequestStatus.Click();
				Log("Clicked to open the select listing request status box");

				//select only the Denied value
				_page.ListingRequestFilterStatusDenied.Click();
				Log("Selected the Denied status option");
				_page.ListingRequestFilterStatusNew.Click();
				Log("Deselected the New status option");
				_page.ListingRequestFilterStatusUnderReview.Click();
				Log("Deselected the Under Review status option");
				_page.ListingRequestFilterStatusNew.SendKeys();

				//click apply filters
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked the apply filters button");

				//assert that all displayed options have the Denied value in the status column
				for (var i = 1; i <= _page.GetItemsPerPage(); i++)
				{
					const string columnName = Model.Pages.NewListingRequestPage.ColumnNameSelector.Status;
					var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
					Log($"Validate that the cell text equals '{nameof(ListingRequestStatusType.Denied)}'. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.AreEqual(nameof(ListingRequestStatusType.Denied), cellText);
				}
			});
		}

		[Test]
		public void FilterNewListingRequest_ByCompanyProductName_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup new listing requests
				var listingRequest1 = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);
				var listingRequest2 = SetupNewListingRequestWithAllValidFields(RequestUtility.GetRandomString(9), $"https://www.{RequestUtility.GetRandomString(9)}.com");

				//add company/product validation
				_page.InputCompanyProductNameFilter.SendKeys(listingRequest1.CompanyName, true);

				//click apply filters
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked the apply filters button");

				var tdCompanyName = Model.Pages.NewListingRequestPage.ColumnNameSelector.Company;

				//assert listing request 1 is present
				var companyName = BasePage.GetTableCellByRowNumberAndColumnName(1, tdCompanyName).GetText();
				Assert.AreEqual(_companyName, companyName);

				//asert listing request 2 is not present
				Assert.IsFalse(BasePage.GetTableCellByRowNumberAndColumnName(2, tdCompanyName).ExistsInPage());
			});
		}

		[Test]
		public void FilterNewListingRequest_ByHqCountryName_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup a new listing request
				var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//add company/product validation
				_page.InputCompanyProductNameFilter.SendKeys(listingRequest.CompanyName, true);

				//open the select HQ Country box
				_page.SelectListingRequestHqCountry.Click();
				Log("Clicked to open the select listing request HQ Country menu");
				_page.SelectListingRequestHqCountryInput.SendKeys(_countryName, sendEscape: false);
				var countryOptionText = Model.Pages.NewListingRequestPage.GetCountryOptionText(_countryName).GetText();
				Log($"Typed {_countryName} into the HQ Country autocomplete input field");
				BasePage.GetCountryOptionByName(_countryName).Click(false);
				Log($"Clicked on the option for {_countryName}.");
				_page.SelectListingRequestHqCountryInput.SendKeys();

				//click apply filters
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked the apply filters button");

				var tdCompanyName = Model.Pages.NewListingRequestPage.ColumnNameSelector.Company;
				var tdCountryCode = Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry;

				//assert listing request is present
				var companyName = BasePage.GetTableCellByRowNumberAndColumnName(1, tdCompanyName).GetText();
				var countryCode = BasePage.GetTableCellByRowNumberAndColumnName(1, tdCountryCode).GetText();
				Assert.AreEqual("(US) United States of America", countryOptionText);
				Assert.AreEqual(_companyName, companyName);
				Assert.AreEqual(_countryCode, countryCode);
			});
		}

		[Test]
		public void FilterNewListingRequest_ByHqCountryCode_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup a new listing request
				var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//add company/product validation
				_page.InputCompanyProductNameFilter.SendKeys(listingRequest.CompanyName, true);

				//open the select HQ Country box
				_page.SelectListingRequestHqCountry.Click();
				Log("Clicked to open the select listing request HQ Country menu");
				_page.SelectListingRequestHqCountryInput.SendKeys(_countryCode, sendEscape: false);
				var countryOptionText = Model.Pages.NewListingRequestPage.GetCountryOptionText(_countryName).GetText();
				Log($"Typed {_countryCode} into the HQ Country autocomplete input field");
				_page.SelectListingRequestHqCountryInput.SendKeys(Keys.Enter);
				Log($"Selected the option for {_countryCode}.");
				_page.SelectListingRequestHqCountryInput.SendKeys();

				//click apply filters
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Clicked the apply filters button");

				var tdCompanyName = Model.Pages.NewListingRequestPage.ColumnNameSelector.Company;
				var tdCountryCode = Model.Pages.NewListingRequestPage.ColumnNameSelector.HqCountry;

				//assert listing request is present
				var companyName = BasePage.GetTableCellByRowNumberAndColumnName(1, tdCompanyName).GetText();
				var countryCode = BasePage.GetTableCellByRowNumberAndColumnName(1, tdCountryCode).GetText();
				Assert.AreEqual("(US) United States of America", countryOptionText);
				Assert.AreEqual(_companyName, companyName);
				Assert.AreEqual(_countryCode, countryCode);				
			});
		}
		
		private void SetupDeniedListingRequest()
		{
			//Setup
			_categoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = _categoryName }).Result.CategoryId;
			_listingRequestId = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl).ListingRequestId.ToString();

			//Attempt to update a listing request with an invalid product short description value
			_putRequest = new ListingRequestUpdateRequest
			{
				StatusTypeId = (int)ListingRequestStatusType.Denied,
				DenialReasonTypeId = ListingRequestDenialReasonType.Other,
				CompanyName = _updatedCompanyName,
				CompanyWebsiteUrl = $"https://www.{_updatedCompanyName}.com",
				CompanyPhoneNumber = RequestUtility.GetRandomString(10),
				CompanyAddress = new AddressRequest
				{
					CountryId = 237
				},
				ProductName = _updatedProductName,
				ProductWebsiteUrl = $"https://{_updatedProductName}.com",
				ProductShortDescription = RequestUtility.GetRandomString(135),
				ProductAcceptedCategoryId = _categoryId
			};

			VendorAdminApiService.PutListingRequest(_listingRequestId, _putRequest);
			Log("Updated listing request successfully");
		}
	}
}
