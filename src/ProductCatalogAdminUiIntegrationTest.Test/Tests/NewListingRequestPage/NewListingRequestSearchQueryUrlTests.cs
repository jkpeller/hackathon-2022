using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;


namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
    [TestFixture]
    public class NewListingRequestSearchQueryUrlTests : BaseTest
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

        public NewListingRequestSearchQueryUrlTests() : base(nameof(NewListingRequestSearchQueryUrlTests))
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
        public void NewListingRequest_SearchUrlListingStatusChagne_BackButtonResetParameters_Succeeds()
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

                //assert that we only have new status in the url
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?listingRequestStatusIds=1&"));

                // go back and confirm that parameters are rest
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?listingRequestStatusIds=1,2&pageNumber=1&pageSize=50&sortColumn=Source&sortDirection=1"));

            });
        }

        [Test]
        public void NewListingRequest_SearchUrlWithHqCountry_BackButtonResetParameters_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //setup a new listing request
                var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

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

                //assert that we have new and approved statuses in the url
                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?countryCodes2={_countryCode}&"));

                // go back and confirm that parameters are rest
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?listingRequestStatusIds=1,2&pageNumber=1&pageSize=50&sortColumn=Source&sortDirection=1"));

            });
        }

        [Test]
        public void NewListingRequest_SearchUrlWithCompanyProductName_BackButtonResetParameters_Succeeds()
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

                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?companyProductName={_companyName}&"));

                // go back and confirm that parameters are rest
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?listingRequestStatusIds=1,2&pageNumber=1&pageSize=50&sortColumn=Source&sortDirection=1"));

            });
        }

        [Test]
        public void NewListingRequest_SearchUrlWithNewAndApprovedStatuses_Succeeds()
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

                //open the select status box
                _page.SelectListingRequestStatus.Click();
                Log("Clicked to open the select listing request status box");

                //select the Approved option 
                _page.ListingRequestFilterStatusApproved.Click();
                Log("Select the Approved status option");
                _page.ListingRequestFilterStatusApproved.SendKeys();

                //click apply filters
                _page.ButtonApplyFilters.ClickAndWaitForPageToLoad();

                //assert that we have new and approved statuses in the url
                var ur = _page.GetCurrentUrl();
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?listingRequestStatusIds=3,1&"));

            });
        }

        [Test]
        public void NewListingRequest_SearchUrlWithHqCountry_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //setup a new listing request
                var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

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

                //assert that we have new and approved statuses in the url
                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?countryCodes2={_countryCode}&"));

            });
        }

        [Test]
        public void NewListingRequest_SearchUrlWithCompanyProductName_Succeeds()
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

                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?companyProductName={_companyName}&"));
            });
        }

    }
}
