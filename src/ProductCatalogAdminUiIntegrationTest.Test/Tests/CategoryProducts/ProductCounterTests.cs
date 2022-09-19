using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryProducts
{
    [Category("Category")]
    public class ProductCounterTests : BaseTest
    {
        private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
        private Model.Pages.CategoriesPage _categoriesPage;

        public ProductCounterTests() : base(nameof(ProductCounterTests))
        {
        }

        [SetUp]
        public void SetUp()
        {
            _categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
            _categoriesPage = new Model.Pages.CategoriesPage();
        }

        [Test]
		public void CategoryProducts_ProductCountIncreaseWhenAnArchivedProductIsSetBackToActive_Succeeds()
		{
			var productName1 = RequestUtility.GetRandomString(9);
			var productName2 = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product1
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName1,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product1 { productName1 } created.");

				//Create product2
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName2,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product2 { productName2 } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();
				var (category2Id, category2Name) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId, category2Id }
				}).Result.Where(pc => pc.CategoryId == category2Id).FirstOrDefault().ProductCategoryId;
				Log($"{categoryName} is assigned to {productName1}.");

				//Insert product-category relationship 2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId2,
					CategoryIds = new List<int> { categoryId, category2Id }
				});
				Log($"{categoryName} is assigned to {productName2}.");
				
				NavigateToCategoryDetailsPage(category2Id.ToString());
				var productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");
				
				//Verified product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = false,
					StatusTypeId = ProductCategoryStatusType.Archived
				});
				Log($"{categoryName} is removed for {productName1}.");
				
				BrowserUtility.Refresh();
				BrowserUtility.WaitForPageToLoad();
				productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "1");

				//Unverified product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active
				});
				Log($"{categoryName} is unverified for {productName1}.");
				
				BrowserUtility.Refresh();
				BrowserUtility.WaitForPageToLoad();
				productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1.ToString());
				ProductAdminApiService.DeleteProduct(productId2.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void CategoryProducts_ProductCountStaysSameWhenTryToRemovePrimaryCategory_Succeeds()
		{	
			var productName1 = RequestUtility.GetRandomString(9);
			var productName2 = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product1
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName1,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product1 { productName1 } created.");

				//Create product2
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName2,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product2 { productName2 } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId }
				}).Result.Select(pc => pc.ProductCategoryId).SingleOrDefault();
				Log($"{categoryName} is assigned to {productName1}.");

				//Insert product-category relationship 2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId2,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {productName2}.");
				
				NavigateToCategoryDetailsPage(categoryId.ToString());
				var productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");
				
				//Verified product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});
				Log($"{categoryName} is removed for {productName1}.");
				
				BrowserUtility.Refresh();
				BrowserUtility.WaitForPageToLoad();
				productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1.ToString());
				ProductAdminApiService.DeleteProduct(productId2.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
		
		[Test]
		public void CategoryProducts_ProductCountDoesNotChangeWhenAnUnverifiedProductIsSetToVerified_Succeeds()
		{
			var productName1 = RequestUtility.GetRandomString(9);
			var productName2 = RequestUtility.GetRandomString(9);
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_categoriesPage);

				//Create product1
				var productId1 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName1,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product1 { productName1 } created.");

				//Create product2
				var productId2 = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = productName2,
					ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/"
				}).Result.ProductId;
				Log($"Product2 { productName2 } created.");

				//Create category
				var (categoryId, categoryName) = CreateCategory();

				//Insert product-category relationship 1
				var productCategory1 = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId1,
					CategoryIds = new List<int> { categoryId }
				}).Result.Select(pc => pc.ProductCategoryId).SingleOrDefault();
				Log($"{categoryName} is assigned to {productName1}.");

				//Insert product-category relationship 2
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId2,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"{categoryName} is assigned to {productName2}.");
				
				NavigateToCategoryDetailsPage(categoryId.ToString());
				var productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");
				
				//Verified product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});
				Log($"{categoryName} is verified for {productName1}.");
				
				BrowserUtility.Refresh();
				BrowserUtility.WaitForPageToLoad();
				productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");

				//Unverified product-category 1
				ProductAdminApiService.PutProductCategory(productCategory1.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Unverified
				});
				Log($"{categoryName} is unverified for {productName1}.");
				
				BrowserUtility.Refresh();
				BrowserUtility.WaitForPageToLoad();
				productCount = _categoryDetailsPage.ProductCount.GetText();
				Assert.AreEqual(productCount, "2");

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1.ToString());
				ProductAdminApiService.DeleteProduct(productId2.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}
    }
}