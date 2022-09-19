using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
    internal class VendorPageSearchQueryUrlTests : BaseTest
    {
        private Model.Pages.VendorPage _page;
        private readonly string _vendorName;
        private readonly string _vendorWebsiteUrl;
        private string _categoryName;
        private string _updatedProductName;
        private string _updatedCompanyName;
        private string _companyName;
        private string _companyWebsiteUrl;
        private string _countryName = "United States of America";
        private const string _archivedStatus = "Archived";

        public VendorPageSearchQueryUrlTests() : base(nameof(VendorPageSearchQueryUrlTests))
        {
            _vendorName = RequestUtility.GetRandomString(9);
            _vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
        }

        [SetUp]
        public void SetUp()
        {
            _page = new Model.Pages.VendorPage();
            _categoryName = RequestUtility.GetRandomString(10);
            _updatedProductName = RequestUtility.GetRandomString(9);
            _updatedCompanyName = RequestUtility.GetRandomString(9);
            _companyName = RequestUtility.GetRandomString(9);
            _companyWebsiteUrl = $"https://www.{_updatedCompanyName}.com/{_companyName}";

            //open the page
            _page.OpenPage();
        }

        [Test]
        public void VendorPage_SearchUrlVendorStatusChange_BackButtonResetParameters_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //Search for a vendor by vendor status
                _page.SelectVendorStatus.Click();
                _page.VendorActiveStatus.Click();                 
                _page.VendorArchivedStatus.SendKeys(sendEscape: true);
                Log($"Filter vendor by active status");

                //Click the apply filters button
                _page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
                Thread.Sleep(1500);
                Log("Clicked on the apply filters button");

                //assert that we only have new status in the url
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?vendorStatusIds=3&"));
                //Assert the vendor status on the vendor list result
                 Assert.AreEqual("Archived", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
                Log($"Evaluated the text in the Status column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

                // go back and confirm that parameters are reset
                _page.ClickBrowserBackButton();
                Thread.Sleep(3000);
                Assert.AreEqual("Active", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false));
                Log($"Evaluated the text in the Status column. Text: {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Status).GetText(false)}");

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?vendorStatusIds=4,3&"));
            });
        }

        [Test]
        public void VendorPage_SearchUrlWithHqCountry_BackButtonResetParameters_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //Search for the country value for both countries
                _page.SelectVendorCountry.Click();
                Log("Clicked into the select Country box.");
                _page.InputCountrySearch.SendKeys(_countryName, sendEscape: false);
                Log($"Typed {_countryName} into the Country autocomplete box.");
                BasePage.GetCountryOptionByName(_countryName).Click(false);
                Log($"Clicked on the option for {_countryName}.");
                _page.InputCountrySearch.SendKeys();

                //Click the apply filters button
                _page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
                BrowserUtility.WaitForPageToLoad();

                //assert that we have new and approved statuses in the url
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?countries=237&"));

                // go back and confirm that parameters are rest
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?vendorStatusIds=4,3&pageNumber=1&pageSize=50&sortColumn=Name&sortDirection=1"));

            });
        }

        [Test]
        public void VendorPage_SearchUrlWithVendorName_BackButtonResetParameters_Succeeds()
        {
            var vendorName1 = $"{_vendorName}";
            var vendorUrl1 = $"http://{vendorName1}.com";
            var vendorName2 = RequestUtility.GetRandomString(9);
            var vendorUrl2 = $"http://{vendorName2}.com";

            ExecuteTimedTest(() =>
            {
                //Open the page and setup new vendors
                var vendor1 = PostVendor(vendorName1, vendorUrl1);
                var vendor2 = PostVendor(vendorName2, vendorUrl2);

                //Search for the vendor name
                _page.InputVendorName.SendKeys(_vendorName);
                Log($"Entered {_vendorName} into the vendor name search box.");

                //click apply filters
                _page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
                Log("Clicked the apply filters button");

                var tdVendorName = Model.Pages.VendorPage.ColumnNameSelector.Name;

                //assert listing request 1 is present
                var vendorName = BasePage.GetTableCellByRowNumberAndColumnName(1, tdVendorName).GetText();
                Assert.AreEqual(_vendorName, vendorName);

                //asert listing request 2 is not present
                Assert.IsFalse(BasePage.GetTableCellByRowNumberAndColumnName(2, tdVendorName).ExistsInPage());

                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?vendorName={_vendorName}&"));

                // go back and confirm that parameters are reset
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?vendorStatusIds=4,3&pageNumber=1&pageSize=50&sortColumn=Name&sortDirection=1"));

                //Cleanup the vendor
                DeleteVendor(vendor1.VendorId);
                DeleteVendor(vendor2.VendorId);

            });
        }

        [Test]
        public void VendorPage_SearchUrlWithCategoryName_BackButtonResetParameters_Succeeds()
        {
            var product1Name = RequestUtility.GetRandomString(10);
            var vendorName = RequestUtility.GetRandomString(10);
            var ProductWebsiteUrl = $"https://wwww.{ product1Name}.com";

            ExecuteTimedTest(() =>
            {
                var vendor = PostVendor(vendorName, _vendorWebsiteUrl);

                //create a product associated with the vendor created above
                 var product1 = PostProduct(product1Name, ProductWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));
  
                //Create category1
                var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
                { Name = RequestUtility.GetRandomString(9) });
                var categoryName = category.Result.Name;
                var categoryId = category.Result.CategoryId;

                //Assign both categories to Product
                ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
                {
                    ProductId = Convert.ToInt32(product1.ProductId),
                    CategoryIds = new List<int> { categoryId }
                });

                //Refresh the Vendor Page
                BrowserUtility.Refresh();
                BrowserUtility.WaitForPageToLoad();

                //open the select status box
                _page.SelectCategory.Click();
                Log("Clicked to open the select cagegory box");
                _page.InputCategorySearch.SendKeys(categoryName, sendEscape: false);
                Log($"Typed {categoryName} into the Category autocomplete box.");
                BasePage.GetCategoryOptionByName(categoryName).Click(false);
                Log($"Clicked on the option for {categoryName}.");
                _page.InputCategorySearch.SendKeys();

                //Click the apply filters button
                _page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
                BrowserUtility.WaitForPageToLoad();                        
                Assert.IsTrue(BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.VendorPage.ColumnNameSelector.Name).ExistsInPage());

                //assert that we have new and approved statuses in the url
                var ur = _page.GetCurrentUrl();
                Assert.IsTrue(_page.GetCurrentUrl().Contains($"?categories={categoryId}&"));

                // go back and confirm that parameters are reset
                _page.ClickBrowserBackButton();

                //assert that we reset url parameters to a default value
                Assert.IsTrue(_page.GetCurrentUrl().Contains("?vendorStatusIds=4,3&pageNumber=1&pageSize=50&sortColumn=Name&sortDirection=1"));


                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
                ProductAdminApiService.DeleteProduct(product1.ProductId.ToString());               
            });
        }

        private Tuple<string, string> SetupDataAndPage(Action<Tuple<string, string>> assertionAction)
        {
            //Open the page
            OpenPage(_page);

            //Setup a global category
            var categoryPostResponse = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = RequestUtility.GetRandomString(9) });
            var categoryId = categoryPostResponse.Result.CategoryId;
            var categoryName = categoryPostResponse.Result.Name;

            assertionAction(Tuple.Create(categoryId.ToString(), categoryName));

            BrowserUtility.Refresh();
            BrowserUtility.WaitForPageToLoad();
            Thread.Sleep(5000);

            //Open category filter list
            _page.SelectCategory.Click(false);
            Log("Clicked into the category filter box.");

            return Tuple.Create(categoryId.ToString(), categoryName);
        }
    }
}
