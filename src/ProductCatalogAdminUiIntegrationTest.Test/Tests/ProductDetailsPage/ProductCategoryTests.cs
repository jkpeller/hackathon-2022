using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	public class ProductCategoryTests : BaseTest
	{
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";

		public ProductCategoryTests() : base(nameof(ProductCategoryTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
		}

		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerifyValuesOnTable()
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product				
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category1
				var category1 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
					{Name = $"Architecture{RequestUtility.GetRandomString(9)}"});
				var category1Name = category1.Result.Name;
				var category1Id = category1.Result.CategoryId;

				//Publish category1 to Site
				CategoryAdminApiService.PutCategoryPublishStatusById(category1Id.ToString(),
					new CategoryPublishStatusUpdateRequest
					{
						PublishStatusTypeId = PublishStatusType.Published,
						SiteId = SiteType.Capterra
					});

				//Create category2
				var category2 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
					{Name = $"Project{RequestUtility.GetRandomString(9)}"});
				var category2Name = category2.Result.Name;
				var category2Id = category2.Result.CategoryId;

				//Assign both categories to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> {category1Id, category2Id}
				});

				//Get ProductCategoryId of productId and category2 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == category2Id).ProductCategoryId;

				//Set ProductCategoryId Status to archived
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});

				//Navigates to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assert
				Assert.AreEqual(category1Name,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.That(
					Model.Pages.ProductDetailsPage.GetSiteIconFromCategoriesTableByRowNumber(1, "capterra")
						.GetImageSrc(), Does.Contain(Model.Shared.BasePage.ColorIcon)); //published
				Assert.That(
					Model.Pages.ProductDetailsPage.GetSiteIconFromCategoriesTableByRowNumber(1, "getapp").GetImageSrc(),
					Does.Contain(Model.Shared.BasePage.GrayIcon)); //not published
				Assert.That(
					Model.Pages.ProductDetailsPage.GetSiteIconFromCategoriesTableByRowNumber(1, "software-advice")
						.GetImageSrc(), Does.Contain(Model.Shared.BasePage.GrayIcon)); //not published
				Assert.AreEqual(category2Name,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(ColorBlackRgba,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(2)
						.GetCssValue(ColorCssPropertyName));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(category1Id.ToString());
				CategoryAdminApiService.DeleteCategory(category2Id.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerifyChangedByColumnHasToBeEqualToUserFullNameOnAddCategory()
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category1
				var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
					{Name = $"Architecture{RequestUtility.GetRandomString(9)}"});
				var categoryName = category.Result.Name;
				var categoryId = category.Result.CategoryId;

				//Assign both categories to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> {categoryId}
				});

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				var userName = $"{_productDetailsPage.UserDataFirstName} {_productDetailsPage.UserDataLastName}";
				Log($"The username logged in is {userName}");
				Log($"Text displayed in the categoryName column is {Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText()}");
				Log($"Text username displayed in the Changed By column is {Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(1).GetText()}");

				//Assert
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(userName,
					Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(1).GetText());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}	

		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerifyVerificationStatusOfVerifiedWhenVerifyOptionIsSelected()
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;
								
				var categoryName = RequestUtility.GetRandomString(9);
				var(categoryId,_) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				var userName = $"{_productDetailsPage.UserDataFirstName} {_productDetailsPage.UserDataLastName}";
				var activeCategoryStatusName = "Active";
				var verifiedCategoryVerificationStatusName = "Verified";

				//Assert
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(activeCategoryStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(1).GetText());

				//Get ProductCategoryId of productId and category1 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				//Set ProductCategoryid to active
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified,
					Primary = true
				}) ;

				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Asserts
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(userName,
					Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(activeCategoryStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(verifiedCategoryVerificationStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText());
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}
		
		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerificationStatusSetToArchivedWhenRemovedOptionIsSelected()
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				var categoryName = "_" + RequestUtility.GetRandomString(9);
				var (categoryId, _) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				// Create category2
				var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				{ Name = RequestUtility.GetRandomString(9) });
				var categoryName2 = category.Result.Name;
				var categoryId2 = category.Result.CategoryId;

				//Assign Category2 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId, categoryId2 }
				});
				Log($"category 2 {categoryName} was added to Product {_productName}");

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				var userName = $"{_productDetailsPage.UserDataFirstName} {_productDetailsPage.UserDataLastName}";
				var removedCategoryStatusName = "Removed";
				var rejectedCategoryVerificationStatusName = "Rejected";

				//Get ProductCategoryId of productId and category1 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				//Get ProductCategoryId of productId and category2 relationship
				var productCategoryId2 = productCategories.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

				//Set ProductCategoryId2 status to Active
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
				   StatusTypeId = ProductCategoryStatusType.Active,
				   VerificationTypeId = ProductCategoryVerificationType.Verified,
				   Primary = true	
				});

				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Set ProductCategoryId2 status to Archived
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});

				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assert
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(userName,
					Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(removedCategoryStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(rejectedCategoryVerificationStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(2).GetText());

				//Validate that remove category core features
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var productFeatureIds = productFeatures.Result.SelectMany(r => r.Features.Select(fe => fe.FeatureId)).ToArray();

				var categoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var coreFeatureIds = categoryFeatures.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);

				var hasCoreFeatures = coreFeatureIds.All(id => !productFeatureIds.Contains(id));
				Assert.True(hasCoreFeatures);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerificationStatusIsSetToVerifyForCategoryWithAllCoreFeaturesSeleted()
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

                // Create category1
                var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
                { Name = "_" + RequestUtility.GetRandomString(9) });
                var categoryName = category.Result.Name;
                var categoryId = category.Result.CategoryId;

                // Create Category Core Feature
                var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
                {
                    Name = RequestUtility.GetRandomString(10),
                    Definition = RequestUtility.GetRandomString(10)
                }).Result;

                // Add Core Feature to Category1
                CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
                    new CategoryFeatureInsertRequest
                    {
                        FeatureId = feature.FeatureId,
                        FeatureTypeId = FeatureType.Core
                    });

                //Navigate to product detail page
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

                //Create category2
                category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
                { Name = RequestUtility.GetRandomString(9) });
                var categoryName2 = category.Result.Name;
                var categoryId2 = category.Result.CategoryId;

                // Add Core Feature to Category2
                CategoryAdminApiService.PostCategoryFeature(categoryId2.ToString(),
                new CategoryFeatureInsertRequest
                {
                    FeatureId = feature.FeatureId,
                    FeatureTypeId = FeatureType.Core
                });

                //Assign Category1 to Product
                ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
                {
                    ProductId = productId,
                    CategoryIds = new List<int> { categoryId, categoryId2 }
                });
                Log($"category 1 with core features {categoryName} was added to Product {_productName}");

                //Refresh the page
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//get ProductCategoryId of productId and category1 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				// Set ProductCategoryId1 Status to verified
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});
			
				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Get ProductCategoryId of category2				
				var productCategoryId2 = productCategories.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

				//set ProductCategoryId2 status to Verified
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});	

				// Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert Category Verification status for category2
				Assert.AreEqual("Verified", Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(2).GetText());

				// Assert that category2 core features are selected on the the product
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var productFeatureIds = productFeatures.Result.SelectMany(r => r.Features.Select(fe => fe.FeatureId)).ToArray();

				var categoryFeatures2 = CategoryAdminApiService.GetCategoryFeaturesById(categoryId2.ToString());
				var coreFeatureIds2 = categoryFeatures2.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);
				var hasCoreFeatures2 = coreFeatureIds2.All(id => productFeatureIds.Contains(id));
				Assert.True(hasCoreFeatures2);

				//Assert that core features for both categories are the same
				var categoryFeatures1 = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var coreFeatureIds1 = categoryFeatures1.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);
				Assert.AreEqual(coreFeatureIds1, coreFeatureIds2);
				
				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}
		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_VerificationStatusIsSetToVerifiedWhenAllCoreFeaturesCategoryIsSetBackToActive()	
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category1
				var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				{ Name = "_" + RequestUtility.GetRandomString(9) });
				var categoryName = category.Result.Name;
				var categoryId = category.Result.CategoryId;

				// Create Category Core Feature
				var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
				{
					Name = RequestUtility.GetRandomString(10),
					Definition = RequestUtility.GetRandomString(10)
				}).Result;

				// Add Core Feature to Category1
				CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
					new CategoryFeatureInsertRequest
					{
						FeatureId = feature.FeatureId,
						FeatureTypeId = FeatureType.Core
					});

				//Create category2
				category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				{ Name = RequestUtility.GetRandomString(9) });
				var categoryName2 = category.Result.Name;
				var categoryId2 = category.Result.CategoryId;

				// Add Core Feature to Category2
				CategoryAdminApiService.PostCategoryFeature(categoryId2.ToString(),
				new CategoryFeatureInsertRequest
				{
					FeatureId = feature.FeatureId,
					FeatureTypeId = FeatureType.Core
				});

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId, categoryId2 }
				});
				Log($"category 1 {categoryName} with Core Features was added to Product {_productName}");

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Get ProductCategoryId of productId and category1 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				// Set ProductCategoryId Status to Verified
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				// Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Get ProductCategoryId of productId and category2 relationship
				var productCategoryId2 = productCategories.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

				//Set ProductCategory Status to Verified
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				// Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert Category Verification Status for category2
				Assert.AreEqual("Verified", Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(2).GetText());

				//Set ProductCategoryId2 Status to Archived
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived	
				});

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());   
				
				//Assert Verification status for category2
				Assert.AreEqual("Removed", Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(2).GetText());

				//Set ProductCategoryId2 status back to active
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active
				});

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assert Verification status for category2
				Assert.AreEqual("Active", Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(2).GetText());

				//Assert that category2 core features are selected on the the product
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var productFeatureIds = productFeatures.Result.SelectMany(r => r.Features.Select(fe => fe.FeatureId)).ToArray();

				var categoryFeatures2 = CategoryAdminApiService.GetCategoryFeaturesById(categoryId2.ToString());
				var coreFeatureIds2 = categoryFeatures2.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);
				var hasCoreFeatures2 = coreFeatureIds2.All(id => productFeatureIds.Contains(id));
				Assert.True(hasCoreFeatures2);

				//Assert that core features for both categories are the same
				var categoryFeatures1 = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var coreFeatureIds1 = categoryFeatures1.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);
				Assert.AreEqual(coreFeatureIds1, coreFeatureIds2);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Category("Product")]
		public void ProductCategoryVerify_ArchivedVerificationStatusIsSetBackToActive()		
		{
			ExecuteTimedTest(() =>
			{
				//Open UI to get tokens
				_productDetailsPage.OpenPage();

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category1
				var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
					{Name = "_" + RequestUtility.GetRandomString(9)});
				var categoryName = category.Result.Name;
				var categoryId = category.Result.CategoryId;

				//Create category2
				category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				    { Name = RequestUtility.GetRandomString(9) });
				var categoryName2 = category.Result.Name;
				var categoryId2 = category.Result.CategoryId;

				//Create Category Core Feature
				var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
					{
						Name = RequestUtility.GetRandomString(10),
						Definition = RequestUtility.GetRandomString(10)
					})
					.Result;

				CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
					new CategoryFeatureInsertRequest
					{
						FeatureId = feature.FeatureId,
						FeatureTypeId = FeatureType.Core
					});
				
				//Assign both categories to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> {categoryId, categoryId2}
				});

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

                var userName = $"{_productDetailsPage.UserDataFirstName} {_productDetailsPage.UserDataLastName}";
                var removedCategoryStatusName = "Removed";
                var rejectedCategoryVerificationStatusName = "Rejected";

				//Get ProductCategoryId of productId and category1 relationship
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				//Get ProductCategoryId of productId and category2 relationship
				var productCategoryId2 = productCategories.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

				//Set ProductCategoryId2 status as primary
				ProductAdminApiService.PutProductCategory(productCategoryId2.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Unverified,
					Primary = true
				});

				//Set ProductCategoryId status to archived
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});

				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assert
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(userName,
					Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(removedCategoryStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(2).GetText());
				Assert.AreEqual(rejectedCategoryVerificationStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(2).GetText());

				var activeCategoryStatusName = "Active";
				var verifiedCategoryVerificationStatusName = "Verified";

				//Set ProductCategoryId status back to active
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active
				});

				//Refresh product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				
				//Assert
				Assert.AreEqual(categoryName,
					Model.Pages.ProductDetailsPage.GetCategoryNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(userName,
					Model.Pages.ProductDetailsPage.GetChangedByFullNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(activeCategoryStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryStatusNameFromCategoriesTableByRowNumber(1).GetText());
				Assert.AreEqual(verifiedCategoryVerificationStatusName,
					Model.Pages.ProductDetailsPage.GetCategoryVerificationStatusNameFromCategoriesTableByRowNumber(1).GetText());
								
				// Validate that add all category core features
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var productFeatureIds = productFeatures.Result.SelectMany(r => r.Features.Select(fe => fe.FeatureId)).ToArray();

				var categoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var coreFeatureIds = categoryFeatures.Result.Where(f => f.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId);

				var hasCoreFeatures = coreFeatureIds.All(id => productFeatureIds.Contains(id));
				Assert.True(hasCoreFeatures);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}
	}
}
