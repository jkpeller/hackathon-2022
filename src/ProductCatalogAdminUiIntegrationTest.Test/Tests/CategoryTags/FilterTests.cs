using System.Collections.Generic;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using OpenQA.Selenium;
using System.Threading;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryTags
{
	[TestFixture]
	[Category("Category")]
	public class FilterTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;

		public FilterTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}
		
        [Test]
        public void CategoryTags_FilterForUnassociatedTagNoResults_Success()
        {
            string expectedResults = "No results were returned";
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);
				
                var (categoryId, categoryName) = CreateCategory();
                var (tagId, tagName) = CreateTag(TagType.CategoryTag);

                //Associate tag with category
                var categoryTag = CategoryAdminApiService.PostCategoryTag(categoryId.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tagId
                });

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

                // Update the category tag to remove tag association
				CategoryAdminApiService.PutCategoryTag(categoryId.ToString(), new CategoryTagUpdateRequest
                {
					// empty list
					TagIds = new List<int>()
                    {
                    }
                });

                // Filter by new tag
                _categoryDetailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
                _categoryDetailsPage.SelectCategoryTagFilter.SendKeys();
                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");
				
				// Assert that no Categories are associated with this tag
                Assert.AreEqual(expectedResults, _categoryDetailsPage.MessageTableNoResults.GetText());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteTag(tagId.ToString());
            });
        }

        [Test]
        public void CategoryTags_FilterForTagAssociatedWithOneCategory_Success()
        {
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                var (categoryId, categoryName) = CreateCategory();
                var (tagId, tagName) = CreateTag(TagType.CategoryTag);

                //Associate tag with category
                var categoryTag = CategoryAdminApiService.PostCategoryTag(categoryId.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tagId
                });

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

                // Filter by new tag
                _categoryDetailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tagName).Click();
                _categoryDetailsPage.SelectCategoryTagFilter.SendKeys();
                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                // Assert that 1 Category is associated with this tag
                Assert.AreEqual("1", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(categoryId.ToString());
                CategoryAdminApiService.DeleteTag(tagId.ToString());
            });
        }


        [Test]
        public void CategoryTags_FilterForMultipleTagsAssociatedWithMultipleCategories_Success()
        {
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                var (category1Id, category1Name) = CreateCategory("001");
                var (category2Id, category2Name) = CreateCategory("002");
                var (category3Id, category3Name) = CreateCategory("003");

                var (tag1Id, tag1Name) = CreateTag(TagType.CategoryTag);
                var (tag2Id, tag2Name) = CreateTag(TagType.CategoryTag);

                //Associate tag with category
                var category1Tag = CategoryAdminApiService.PostCategoryTag(category1Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag1Id
                });
                var category2Tag = CategoryAdminApiService.PostCategoryTag(category1Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag2Id
                });
                var category3Tag = CategoryAdminApiService.PostCategoryTag(category2Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag1Id
                });
                var category4Tag = CategoryAdminApiService.PostCategoryTag(category3Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag2Id
                });

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

                // Filter by new tag
                _categoryDetailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tag1Name).Click();
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tag2Name).Click();
                _categoryDetailsPage.SelectCategoryTagFilter.SendKeys();
                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                // Assert that 3 Categories are returned and then verify number of tags associated with each
                Assert.AreEqual(3, Model.Pages.CategoriesPage.GetRowCountByCategoryTagsColumn());
                Assert.AreEqual("2", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());
                Assert.AreEqual("1", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());
                Assert.AreEqual("1", BasePage.GetTableCellByRowNumberAndColumnName(3, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());
                
                //Cleanup
                CategoryAdminApiService.DeleteCategory(category1Id.ToString());
                CategoryAdminApiService.DeleteCategory(category2Id.ToString());
                CategoryAdminApiService.DeleteCategory(category3Id.ToString());
                CategoryAdminApiService.DeleteTag(tag1Id.ToString());
                CategoryAdminApiService.DeleteTag(tag2Id.ToString());
            });
        }


        [Test]
        public void CategoryTags_FilterForTagAssociatedWithMultipleCategories_Success()
        {
            ExecuteTimedTest(() =>
            {
                //Open category page
                OpenPage(_categoriesPage);

                var (category1Id, category1Name) = CreateCategory("001");
                var (category2Id, category2Name) = CreateCategory("002");

                var (tag1Id, tag1Name) = CreateTag(TagType.CategoryTag);
                var (tag2Id, tag2Name) = CreateTag(TagType.CategoryTag);
                var (tag3Id, tag3Name) = CreateTag(TagType.CategoryTag);

                //Associate tag with category
                var category1Tag = CategoryAdminApiService.PostCategoryTag(category1Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag1Id
                });
                var category2Tag = CategoryAdminApiService.PostCategoryTag(category1Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag2Id
                });
                var category3Tag = CategoryAdminApiService.PostCategoryTag(category2Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag3Id
                });
                var category4Tag = CategoryAdminApiService.PostCategoryTag(category2Id.ToString(), new CategoryTagInsertRequest
                {
                    TagId = tag2Id
                });

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName);

                // Filter by new tag
                _categoryDetailsPage.SelectCategoryTagFilter.Click();
                Log("Clicked into the tag filter box.");
                Model.Pages.CategoryDetailsPage.GetCategoryTagFilterOptionByTagName(tag2Name).Click();
                _categoryDetailsPage.SelectCategoryTagFilter.SendKeys();
                //Apply filters
                _categoryDetailsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
                Log("Clicked the apply filters button.");

                // Assert that 2 Categories are returned and then verify number of tag associated with each
                Assert.AreEqual(2, Model.Pages.CategoriesPage.GetRowCountByCategoryTagsColumn());
                Assert.AreEqual("2", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());
                Assert.AreEqual("2", BasePage.GetTableCellByRowNumberAndColumnName(2, Model.Pages.CategoriesPage.ColumnNameSelector.CategoryTags).GetText());

                //Cleanup
                CategoryAdminApiService.DeleteCategory(category1Id.ToString());
                CategoryAdminApiService.DeleteCategory(category2Id.ToString());
                CategoryAdminApiService.DeleteTag(tag1Id.ToString());
                CategoryAdminApiService.DeleteTag(tag2Id.ToString());
                CategoryAdminApiService.DeleteTag(tag3Id.ToString());
            });
        }

    }
}