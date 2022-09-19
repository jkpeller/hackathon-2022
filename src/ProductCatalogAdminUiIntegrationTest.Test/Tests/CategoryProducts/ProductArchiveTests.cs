using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryProducts
{
    [TestFixture]
    [Category("Category")]
    public class ProductArchiveTests: BaseTest
    {
        private Model.Pages.CategoryDetailsPage _categoryDetailsPage;

        public ProductArchiveTests() : base(nameof(FilterTests))
        {
        }

        [SetUp]
        public void SetUp()
        {
            _categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
        }

        [Test]
		public void ProductArchive_ArchivedProductHaveToShowArchivedProductCategoryStatus()
		{
			var archiveStatusName = "Removed";
			ExecuteTimedTest(() =>
			{
				//Open category page
				OpenPage(_categoryDetailsPage);

				var productToDeleteName = RequestUtility.GetRandomString(9);
				var otherProductName = RequestUtility.GetRandomString(9);

				//Create Product
				var productToDeletedId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productToDeleteName
				}).Result.ProductId;

				var otherProductId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = otherProductName
				}).Result.ProductId;

				//Create category
				var categoryName = RequestUtility.GetRandomString(9);
				var otherCategoryName = RequestUtility.GetRandomString(9);

				var categoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest()
				{
					Name = categoryName
				}).Result.CategoryId;

				var otherCategoryId = CategoryAdminApiService.PostCategory(new CategoryInsertRequest()
				{
					Name = otherCategoryName
				}).Result.CategoryId;

				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest()
				{
					CategoryIds = new List<int>(){ categoryId, otherCategoryId },
					ProductId = productToDeletedId
				});
				
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest()
				{
					CategoryIds = new List<int>(){ categoryId },
					ProductId = otherProductId
				});

				//Navigate to category detail page
				Log($"Navigate to the category detail page for category Id {categoryId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
				
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitForPageToLoad();

				Log("Check new 2 products added.");
				Assert.AreEqual(2, Model.Pages.CategoryDetailsPage.GetRowCount());
				
				var productCategories =  ProductAdminApiService.GetProductCategories(productToDeletedId.ToString()).Result;
				var productCategoryId = productCategories.Single(c => c.CategoryId == categoryId).ProductCategoryId;
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});
				
				//Filter products by removed category status
				_categoryDetailsPage.SelectCategoryStatus.Click();
				Log("Clicked into the status filter box.");
				_categoryDetailsPage.SelectCategoryStatusActive.Uncheck();
				
				Log("Check that the Removed option exists in the Category Status select.");
				Assert.IsTrue(_categoryDetailsPage.SelectCategoryStatusArchived.ExistsInPage());
				
				Log("Deselected the active status value.");
				_categoryDetailsPage.SelectCategoryStatusArchived.Check();
				Log("Selected the archived status value.");
				_categoryDetailsPage.SelectCategoryStatus.SendKeys();
				
				_categoryDetailsPage.ButtonApplyFilters.ClickAndWaitForPageToLoad();
				
				Log("Check that the removed product can be filtered out.");
				Assert.AreEqual(1, Model.Pages.CategoryDetailsPage.GetRowCount());

				Log("Check that the Cateogry Status of the selected product-category changed to Removed");
				Assert.AreEqual(archiveStatusName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText());
				Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.CategoryStatus).GetText()}");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productToDeletedId.ToString());
				ProductAdminApiService.DeleteProduct(otherProductId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
    }
}