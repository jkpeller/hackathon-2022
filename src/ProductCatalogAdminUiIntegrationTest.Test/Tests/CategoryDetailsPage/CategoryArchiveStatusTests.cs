using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryDetailsPage
{
    [TestFixture]
    public class CategoryArchiveStatusTests : BaseTest
    {
        private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
        private Model.Pages.CategoriesPage _categoryPage;

        private const string ProductWebsiteUrl ="http://www.testproductwebsiteurl.com/";
        
        public CategoryArchiveStatusTests() : base(nameof(CategoryArchiveStatusTests))
        {
        }
        
        [SetUp]
        public void SetUp()
        {
            _categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
            _categoryPage = new Model.Pages.CategoriesPage();
        }

        [Test]
        [Category("Category")]
        public void CategoryArchiveStatus_VerifyArchivedStatusIsDisplayedWhenCategoryIsDeleted_Succeeds()
        {
            var categoryName = RequestUtility.GetRandomString(8);
            ExecuteTimedTest(() =>
            {
                var categoryId = PostCategoryAndNavigateToDetailsPage(_categoryPage, categoryName);
                
                CategoryAdminApiService.PutCategory(categoryId.ToString(), new CategoryUpdateRequest()
                {
                    CategoryStatusId = (int)CategoryStatusType.Archived,
                    Name = categoryName
                });
                
                BrowserUtility.Refresh();
                BrowserUtility.WaitForPageToLoad();
                
                Assert.IsTrue(_categoryDetailsPage.ArchivedStatusLabel.ExistsInPage());
                
                DeleteCategory(categoryId.ToString());
            });
        }
        
        [Test]
        [Category("Category")]
        public void CategoryArchiveStatus_VerifyArchivedStatusIsNotDisplayedWhenCategoryIsBackToActive_Succeeds()
        {
            var categoryName = RequestUtility.GetRandomString(8);
            ExecuteTimedTest(() =>
            {
                //Create a new category
                var categoryId = PostCategoryAndNavigateToDetailsPage(_categoryPage, categoryName);

                CategoryAdminApiService.PutCategory(categoryId.ToString(), new CategoryUpdateRequest()
                {
                    CategoryStatusId = (int)CategoryStatusType.Archived,
                    Name = categoryName
                });

                CategoryAdminApiService.PutCategory(categoryId.ToString(), new CategoryUpdateRequest()
                {
                    Name = categoryName,
                    CategoryStatusId = (int)CategoryStatusType.Active
                });

                BrowserUtility.Refresh();
                BrowserUtility.WaitForPageToLoad();
                
                Assert.IsFalse(_categoryDetailsPage.ArchivedStatusLabel.ExistsInPage());
                
                DeleteCategory(categoryId.ToString());
            });
        }
        
        [Test]
        [Category("Category")]
        public void CategoryArchiveStatus_VerifyArchivedStatusIsNotDisplayedWhenCategoryProductIsDeleted_Succeeds()
        {
            var categoryName = RequestUtility.GetRandomString(8);
            var categoryName2 = RequestUtility.GetRandomString(8);
            var productName = RequestUtility.GetRandomString(8);

            ExecuteTimedTest(() =>
            {
                var categoryId = PostCategoryAndNavigateToDetailsPage(_categoryPage, categoryName);
                var categoryId2 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest()
                {
                    Name = categoryName2
                }).Result.CategoryId;

                //Create a new category
                var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest()
                {
                    Name = productName,
                    ProductWebsiteUrl = ProductWebsiteUrl
                }).Result.ProductId;
                
                //Assign both categories to Product
                var productCategoryId = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
                {
                    ProductId = productId,
                    CategoryIds = new List<int> { int.Parse(categoryId), categoryId2 }
                })
                    .Result
                    .First(c=>c.CategoryId == int.Parse(categoryId))
                    .ProductCategoryId;
                
                ProductAdminApiService.PutProductCategory(productCategoryId.ToString(),new ProductCategoryUpdateRequest()
                {   
                    StatusTypeId = ProductCategoryStatusType.Archived
                });

                BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId2.ToString());
                BrowserUtility.WaitForPageToLoad();
                
                Assert.IsFalse(_categoryDetailsPage.ArchivedStatusLabel.ExistsInPage());
                
                DeleteCategory(categoryId);
                DeleteCategory(categoryId2.ToString());

                ProductAdminApiService.DeleteProduct(productId.ToString());
            });
        }
    }
}