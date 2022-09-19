using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.SiteCategoryMappingPage
{
	[TestFixture]
	public class FilterTests : BaseTest
	{
		private Model.Pages.SiteCategoryMappingPage _page;
		private string _siteCategoryId;
		private SourceSiteCategorySingleSaveRequest _siteCategorySaveRequest;
		private string _siteCategoryName;

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.SiteCategoryMappingPage();
			_siteCategoryId = RequestUtility.GetUniqueId();
			_siteCategorySaveRequest = RequestUtility.GetSourceSiteCategorySaveRequest(_siteCategoryId);
			_siteCategoryName = _siteCategorySaveRequest.Name;
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByValidSite_Succeeds()
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
					const string columnName = Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Site;
					var cellText = BasePage.GetTableCellByRowNumberAndColumnName(i, columnName).GetText();
					Log($"Validate that the cell text equals '{siteName}'. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.AreEqual(cellText, siteName);
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByAllSites_Succeeds()
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
		[Category("Category")]
		public void FilterSiteCategories_ByNoSites_Fails()
		{
			const string errorMessageText = "Select at least one site";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

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
				Assert.AreEqual(_page.ErrorMessageSelectSite.GetText(), errorMessageText);
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByMappedStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//select Mapped in the mapping status filter box
				_page.SelectMappingStatus.Click();
				_page.SelectOptionMappingStatusMapped.Check();
				Log("Use the mapping status filter to check mapped category mapping status.");

				//de-select Unmapped in the mapping status filter box
				_page.SelectOptionMappingStatusUnmapped.Uncheck();
				Log("Use the mapping status filter to uncheck unmapped category mapping status.");

				//hit the escape key to get rid of the overlay the filter box brings up, which prevents other buttons from being visible
				_page.SelectOptionMappingStatusUnmapped.SendKeys();
				Log("Send the Escape key to the Ui to remove the overlay container for the mapping status filter box.");

				//click the Apply Filters button and wait for the page to load
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Click the apply filters button and wait for the loading overlay to disappear.");

				//click to sort by Global Category to be sure that any empty/null values are shown first
				_page.TableHeaderCategoryName.ClickAndWaitForPageToLoad();
				Log("Click the Global Category table header to sort the column in ascending order.");

				//get the count of records displayed on the current page in the table
				var itemsInCurrentPage = _page.GetItemsPerPage();

				for (var i = 1; i <= itemsInCurrentPage; i++)
				{
					const string columnName = Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.GlobalCategory;
					var cellText = Model.Pages.SiteCategoryMappingPage.GetTableGlobalCategoryDisplayByRow(i).GetText();
					Log($"Validate that the value is not null. Row: {i}, Column: {columnName}, Text Value: {cellText}");
					Assert.IsNotNull(cellText);
					Assert.IsNotEmpty(cellText);
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByUnmappedStatus_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//select all values for publish status
				_page.SelectPublishStatus.Click();
				Log("Open the select publish status filter box.");
				_page.SelectOptionPublishStatusUnpublished.Check();
				Log("Check the unpublished publish status value.");
				_page.SelectOptionPublishStatusUnpublished.SendKeys();

				//click the Apply Filters button and wait for the page to load
				_page.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				Log("Click the apply filters button and wait for the loading overlay to disappear.");

				//get the count of records displayed on the current page in the table
				var itemsInCurrentPage = _page.GetItemsPerPage();
				Log($"Items per page: {_page.GetItemsPerPage()}");

				for (var i = 1; i <= itemsInCurrentPage; i++)
				{
					BasePage.GetTableCellByRowNumberAndColumnName(i, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
					Assert.IsTrue(BasePage.GetTableCellByRowNumberAndColumnName(i, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.GlobalCategory).GetText().Contains("ADD MAPPING"));
				}
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByAllMappingStatuses_Succeeds()
		{
			const string siteNameTextDisplay = "All";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//select Mapped in the mapping status filter box
				_page.SelectMappingStatus.Click();
				_page.SelectOptionMappingStatusMapped.Check();
				Log("Use the mapping status filter to check mapped category mapping status.");

				//hit the escape key to get rid of the overlay the filter box brings up, which prevents other buttons from being visible
				_page.SelectOptionMappingStatusUnmapped.SendKeys();
				Log("Send the Escape key to the Ui to remove the overlay container for the mapping status filter box.");

				//validate that the filter box contains All text
				Assert.AreEqual(_page.SelectMappingStatus.GetText(), siteNameTextDisplay);
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByNoMappingStatus_Fails()
		{
			const string errorMessageText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//de-select Unmapped in the mapping status filter box
				_page.SelectMappingStatus.Click();
				_page.SelectOptionMappingStatusUnmapped.Uncheck();
				Log("Uncheck unmapped category mapping status.");

				//hit the escape key to get rid of the overlay the filter box brings up, which prevents other buttons from being visible
				_page.SelectOptionMappingStatusUnmapped.SendKeys();
				Log("Remove the overlay container for the mapping status filter box.");

				//an error is present under the drop down stating that at least one option must be selected
				Assert.AreEqual(_page.ErrorMessageSelectMappingStatus.GetText(), errorMessageText);
				Assert.IsFalse(_page.ButtonApplyFilters.IsDisplayed());
			});
		}

		[Test]
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByValidPublishStatus_Succeeds()
		{
			const string expectedPublishStatusValue = "Unpublished";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the publish status filter drop down to select only the unpublished value
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
		[Category("Category")]
		public void FilterSiteCategories_ByAllPublishStatuses_Succeeds()
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
		[Category("Readonly")]
		[Category("Category")]
		public void FilterSiteCategories_ByNoPublishStatuses_Fails()
		{
			const string expectedErrorMessageText = "Select at least one status";
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

				//use the publish status filter drop down to de-select all available options
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

		[Test]
		[Category("Category")]
		public void AutocompleteCategory_ByValidName_Succeeds()
		{
			var categoryName = RequestUtility.GetRandomString(9);
			var searchText = categoryName.Substring(0, categoryName.Length - 1);
			ExecuteTimedTest(() =>
			{
				//Setup
				IntegrationApiService.PostSourceSiteCategory(_siteCategorySaveRequest);
				SetUpPage(_siteCategoryName);
				var categoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = categoryName }).Result.CategoryId.ToString();

				//search for the new global category in the modal search box
				_page.InputMappingModalSearchText.SendKeys(searchText, sendEscape: false);
				Log($"Entered {searchText} into the category mapping modal search box.");

				//Assert that an autocomplete suggestion appears with the name of the category
				Assert.AreEqual(categoryName, BasePage.GetAutocompleteSuggestionByPosition(2).GetText(false));
				Log("Validated the autocomplete suggestion.");

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _siteCategoryId);
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Category")]
		public void AutocompleteCategory_ByBelowMinimumCharacters_Fails()
		{
			var searchText = RequestUtility.GetRandomString(2);
			ExecuteTimedTest(() =>
			{
				//Setup
				IntegrationApiService.PostSourceSiteCategory(_siteCategorySaveRequest);
				SetUpPage(_siteCategoryName);

				//search for the new global category in the modal search box
				_page.InputMappingModalSearchText.SendKeys(searchText, sendEscape: false);
				Log($"Entered {searchText} into the category mapping modal search box.");

				//Assert that the autocomplete box does not appear
				Assert.IsFalse(_page.OptionFirstAutocompleteSuggestion.IsDisplayed());
				Log("Validated the autocomplete suggestion.");

				//Cleanup
				IntegrationApiService.DeleteSourceSiteCategory(RequestUtility.SiteCode, _siteCategoryId);
			});
		}

		private void SetUpPage(string siteCategoryName)
		{
			//Open the page
			OpenPage(_page);

			//Type searchText into the site category name filter box
			_page.InputFilterCategoryName.SendKeys(siteCategoryName);
			Log($"Typed {siteCategoryName} into the site category name filter box.");

			//Click the apply filters button
			_page.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
			Log("Clicked the apply filters button.");

			//Click the site category mapping button in the row for the new site category record in the table
			BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.SiteCategoryMappingPage.ColumnNameSelector.Name).HoverOver();
			Model.Pages.SiteCategoryMappingPage.GetSiteCategoryMappingButtonByRowNumber(1).Click(false);
			Log("Click the site category mapping button for the result record row.");
		}
	}
}
