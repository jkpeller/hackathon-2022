using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteProductMappingPage
{
	[TestFixture]
	[Ignore("Mapping capabilities were disabled on GCT-1315")]
	public class FilterTests : BaseTest
	{
		private Model.Pages.SiteProductMappingPage _page;
		private string _sourceSiteProductId;
		private SourceSiteProductSaveRequest _sourceSiteProductSaveRequest;

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteProductMappingPage();
			_sourceSiteProductId = RequestUtility.GetUniqueId();
			_sourceSiteProductSaveRequest = RequestUtility.GetSourceSiteProductSaveRequest(_sourceSiteProductId);
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByValidSite_Succeeds()
		{
			var siteName = SiteType.Capterra.ToString();
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//filter on the Capterra site name value only in the site name filter drop down
				_page.SelectSite.Click();
				Log("Site name filter drop down opened.");
				_page.SelectOptionSiteGetapp.Uncheck();
				Log("Getapp site name value was deselected");
				_page.SelectOptionSiteSoftwareAdvice.Uncheck();
				Log("Software Advice site name value was deselected");
				_page.SelectOptionSiteSoftwareAdvice.SendKeys();

				//click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Apply filters button clicked.");

				//get the count of records displayed on the current page in the table
				var itemsInCurrentPage = _page.GetItemsPerPage();

				for (var i = 1; i <= itemsInCurrentPage; i++)
				{
					const string columnName = Model.Pages.SiteProductMappingPage.ColumnNameSelector.Site;
					var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
					Log($"Validate that the cell text equals '{siteName}'. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.AreEqual(cellText, siteName);
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByAllSites_Succeeds()
		{
			const string siteNameTextDisplay = "All";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//site name filter drop down contains contains "All" when all available sites are selected
				Assert.AreEqual(_page.SelectSite.GetText(), siteNameTextDisplay);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByValidMappingStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//filter on the mapped mapping status value only in the site name filter drop down
				_page.SelectMappingStatus.Click();
				Log("Mapping status filter drop down opened.");
				_page.SelectOptionMappingStatusUnmapped.Uncheck();
				Log("Unmapped mapping status value was deselected");
				_page.SelectOptionMappingStatusMapped.Check();
				Log("Mapped mapping status value was selected");
				_page.SelectOptionMappingStatusMapped.SendKeys();

				//click the apply filters button
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Apply filters button clicked.");

				//get the count of records displayed on the current page in the table
				var itemsInCurrentPage = _page.GetItemsPerPage();

				for (var i = 1; i <= itemsInCurrentPage; i++)
				{
					const string columnName = Model.Pages.SiteProductMappingPage.ColumnNameSelector.GlobalProduct;
					var cellText = Model.Pages.SiteProductMappingPage.GetTableGlobalProductContainerByRow(i).GetText();
					Log($"Validate that the cell text is not null or empty. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.IsNotNull(cellText);
					Assert.IsNotEmpty(cellText);
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByAllMappingStatuses_Succeeds()
		{
			const string expectedSiteNameTextDisplay = "All";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the mapping status filter drop down to select all options 
				_page.SelectMappingStatus.Click();
				Log("Mapping status filter drop down opened.");
				_page.SelectOptionMappingStatusMapped.Check();
				Log("Unmapped mapping status value was selected");
				_page.SelectOptionMappingStatusMapped.SendKeys();

				//site name filter drop down contains contains "All" when all available sites are selected
				Assert.AreEqual(expectedSiteNameTextDisplay, _page.SelectMappingStatus.GetText());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByValidPublishStatus_Succeeds()
		{
			const string expectedPublishStatusValue = "Unpublished";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the publish status filter drop down to select only the Unpublished value
				_page.SelectPublishStatus.Click();
				Log("Publish status filter drop down clicked.");
				_page.SelectOptionPublishStatusPublished.Uncheck();
				Log("Published publish status value deselected.");
				_page.SelectOptionPublishStatusUnpublished.Check();
				Log("Unpublished publish status value selected.");
				_page.SelectOptionPublishStatusUnpublished.SendKeys();

				//click the apply filters button, as the default filtering should be by the published publish status value
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Apply filters button clicked.");

				//get the count of records displayed on the current page in the table
				var itemsInCurrentPage = _page.GetItemsPerPage();

				for (var i = 1; i <= itemsInCurrentPage; i++)
				{
					const string columnName = Model.Pages.SiteProductMappingPage.ColumnNameSelector.PublishStatus;
					var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
					Log($"Validate that the cell text equals '{expectedPublishStatusValue}'. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.AreEqual(expectedPublishStatusValue, cellText);
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByAllPublishStatuses_Succeeds()
		{
			const string expectedPublishStatusSelectedDisplay = "All";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the publish status filter drop down to select all options 
				_page.SelectPublishStatus.Click();
				Log("Publish status filter drop down clicked.");
				_page.SelectOptionPublishStatusPublished.Check();
				Log("Published publish status value selected.");
				_page.SelectOptionPublishStatusUnpublished.Check();
				Log("Unpublished publish status value selected.");
				_page.SelectOptionPublishStatusUnpublished.SendKeys();

				//publish status filter drop down contains contains "All" when all available options are selected
				Assert.AreEqual(expectedPublishStatusSelectedDisplay, _page.SelectPublishStatus.GetText());
			});
		}

		[Test]
		[Category("Product")]
		public void FilterSiteProducts_ByValidProductName_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup data by creating a site product
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				//select all publish and mapping status filter options and click apply filters
				OpenPageAndFilterByProductNameAndAllFilters(_page, _sourceSiteProductSaveRequest.Name);

				//assert that the searched-for site product shows up in the table
				Assert.AreEqual(_sourceSiteProductSaveRequest.Name.Trim(), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).GetTextWithoutTrailingId());

				//cleanup the source site product
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Product")]
		public void FilterSiteProducts_ValidateShareableUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//setup data by creating a site product
				IntegrationApiService.PostSourceSiteProduct(_sourceSiteProductSaveRequest);

				//select all publish and mapping status filter options and click apply filters
				OpenPageAndFilterByProductNameAndAllFilters(_page, _sourceSiteProductSaveRequest.Name);

				//refresh the page in its current state when the query parameters should be in place
				Log($"Url value before refresh: {BrowserUtility.WebDriver.Url}");
				BrowserUtility.Refresh();
				Log($"Url value after refresh: {BrowserUtility.WebDriver.Url}");

				//validate that only the new site product shows up, meaning that the filters were applied in the shared url
				Assert.IsTrue(BrowserUtility.WebDriver.Url.Contains(_sourceSiteProductSaveRequest.Name));
				Assert.AreEqual(_sourceSiteProductSaveRequest.Name, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).GetTextWithoutTrailingId());

				//cleanup the source site product
				IntegrationApiService.DeleteSourceSiteProduct(_sourceSiteProductId);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByNoMappingStatuses_Fails()
		{
			const string expectedErrorMessageText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the mapping status filter drop down to de-select all available options
				_page.SelectMappingStatus.Click();
				Log("Mapping status filter drop down opened.");
				_page.SelectOptionMappingStatusUnmapped.Uncheck();
				Log("Unmapped mapping status value was deselected");
				_page.SelectOptionMappingStatusUnmapped.SendKeys();

				//an error is present under the drop down stating that at least one option must be selected
				Assert.AreEqual(expectedErrorMessageText, _page.ErrorMessageSelectMappingStatus.GetText());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByBelowMinimumProductName_Fails()
		{
			const string expectedProductNameErrorText = "You must type at least 3 characters";
			var searchText = RequestUtility.GetRandomString(2);
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//type 2 characters into the product name filter box
				_page.InputFilterProductName.SendKeys(searchText);			
				_page.PageTitle.Click();

				//assert that the error message is correctly displayed below the filter and that the apply filters button is disabled
				Assert.AreEqual(expectedProductNameErrorText, _page.ErrorMessageInputProductName.GetText());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		[Test]
		[Category("Product")]
		public void FilterSiteProducts_ByAboveMaximumProductName_Fails()
		{
			var siteProductId = RequestUtility.GetRandomString(50);
			var sourceSiteProductSaveRequest = RequestUtility.GetMaxLengthSourceSiteProductSaveRequest(siteProductId);
			var searchText = $"{sourceSiteProductSaveRequest.Name}_{RequestUtility.GetRandomString(5)}";
			ExecuteTimedTest(() =>
			{
				//setup data by creating a site product
				IntegrationApiService.PostSourceSiteProduct(sourceSiteProductSaveRequest);

				//select all publish and mapping status filter options and click apply filters
				OpenPageAndFilterByProductNameAndAllFilters(_page, searchText);

				//validate that the characters after the 100-character site product name were not entered so that the created site product was still returned
				Assert.AreEqual(sourceSiteProductSaveRequest.Name.Trim(), BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteProductMappingPage.ColumnNameSelector.ProductName).GetTextWithoutTrailingId());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByNoSites_Fails()
		{
			const string expectedErrorMessageText = "Select at least one site";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				Assert.IsFalse(_page.ButtonApplyFilters.IsDisabled());

				//use the site name filter drop down to de-select all available options
				_page.SelectSite.Click();
				Log("Site name filter drop down opened.");
				_page.SelectOptionSiteCapterra.Uncheck();
				Log("Capterra site name value was deselected");
				_page.SelectOptionSiteGetapp.Uncheck();
				Log("Getapp site name value was deselected");
				_page.SelectOptionSiteSoftwareAdvice.Uncheck();
				Log("Software Advice site name value was deselected");
				_page.SelectOptionSiteSoftwareAdvice.SendKeys();

				//an error is present under the drop down stating that at least one site filter option must be selected
				Assert.AreEqual(expectedErrorMessageText, _page.ErrorMessageSelectSite.GetText());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Product")]
		public void FilterSiteProducts_ByNoPublishStatuses_Fails()
		{
			const string expectedErrorMessageText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the mapping status filter drop down to de-select all available options
				_page.SelectPublishStatus.Click();
				Log("Publish status filter drop down clicked.");
				_page.SelectOptionPublishStatusPublished.Uncheck();
				Log("Published publish status value deselected.");
				_page.SelectOptionPublishStatusUnpublished.Uncheck();
				Log("Unpublished publish status value deselected.");
				_page.SelectOptionPublishStatusUnpublished.SendKeys();

				//an error is present under the drop down stating that at least one option must be selected
				Assert.AreEqual(expectedErrorMessageText, _page.ErrorMessageSelectPublishStatus.GetText());
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		private void OpenPageAndFilterByProductNameAndAllFilters(BasePage page, string siteProductName)
		{
			OpenPage(page);

			//type the name of the new site product in the product name filter box
			page.InputFilterProductName.SendKeys(siteProductName);

			//select all options from the mapping status filter box
			page.SelectMappingStatus.Click();
			Log("Mapping status filter drop down opened.");
			page.SelectOptionMappingStatusMapped.Check();
			Log("Mapped mapping status value was selected");
			page.SelectOptionMappingStatusMapped.SendKeys();

			//select all options from the publish status filter box
			page.SelectPublishStatus.Click();
			Log("Publish status filter drop down clicked.");
			page.SelectOptionPublishStatusUnpublished.Check();
			Log("Unpublished publish status value selected.");
			page.SelectOptionPublishStatusUnpublished.SendKeys();

			//click the Apply Filters button and wait for the page to load
			page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
			Log("Click the apply filters button and wait for the loading overlay to disappear.");
		}
	}
}
