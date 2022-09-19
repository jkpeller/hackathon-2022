using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoryProducts
{
	[TestFixture]
	[Category("Category")]
	public class CategoryFitTests : BaseTest
	{
		private Model.Pages.CategoryDetailsPage _categoryDetailsPage;
		private Model.Pages.CategoriesPage _categoriesPage;
		private string _productName = RequestUtility.GetRandomString(9);
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";

		public CategoryFitTests() : base(nameof(FilterTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_categoryDetailsPage = new Model.Pages.CategoryDetailsPage();
			_categoriesPage = new Model.Pages.CategoriesPage();
		}

		[Test]
		public void CategoryFitVerification_MissingCoreFeaturesAreAddedToProductWhenCategoryIsSetVerified()
		{
			var verifiedCategoryVerificationStatusName = "Verified";
			ExecuteTimedTest(() =>
			{
				//Open category page
				OpenPage(_categoriesPage);

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category with core features
				var categoryName = RequestUtility.GetRandomString(9);
				var (categoryId, _) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				//Get category core features
				var getCategoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var categoryCoreFeatureName = getCategoryFeatures.Result.Where(x => x.FeatureTypeId == FeatureType.Core).Select(cf => cf.Name).Single();

				//Navigate to category detail page
				Log($"Navigate to the category detail page for category Id {categoryId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

				// On the category card, select the verify option				
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				// Refresh the category detail page and assert that verification status is set to Verified
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
				Assert.AreEqual(verifiedCategoryVerificationStatusName, BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Navigate to the product detail page to assert that the core feature from category was added to the products
				Log($"Navigate to the product detail page for productId {productId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert that the category core feature was added to the product and it appears selected
				var getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var selectedCoreFeature = getProductFeatures.Result.Single().Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(selectedCoreFeature);

				// Assert that the category core feature appears selected on the product detail page.
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(categoryCoreFeatureName);
				Assert.IsTrue(checkbox.IsSelected());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void CategoryFitVerification_CategoryVerificationIsSetToVerifiedForProductWithAllCoreFeatureSelected()
		{
			ExecuteTimedTest(() =>
			{
				//Open category page
				OpenPage(_categoriesPage);

				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category1 with core features
				var categoryName = RequestUtility.GetRandomString(9);
				var (categoryId, _) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				//Get category core features 
				var getCategoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var categoryCoreFeatureName = getCategoryFeatures.Result.Where(x => x.FeatureTypeId == FeatureType.Core).Select(cf => cf.Name).Single();

				//Get category featureId
				int categoryCoreFeatureId = getCategoryFeatures.Result.Where(x => x.FeatureTypeId == FeatureType.Core).Select(cf => cf.FeatureId).Single();

				//Get product categories to verify category1
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;

				// Select Verify menu option for category1 in order to show the core feature selected 
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				//Navigates to category detail page of category1
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

				// Assert that category1 is in Verified status after the  Verify option is selected
				Assert.AreEqual("Verified", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category1 verification status after selecting the Verify option is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Assert that category1 core feature was added to the product and it appears selected
				var getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var category1CoreFeatureSelectionStatus = getProductFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(category1CoreFeatureSelectionStatus);

				//Create category2
				var category2 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				{ Name = "_" + RequestUtility.GetRandomString(9) });
				var categoryName2 = category2.Result.Name;
				var categoryId2 = category2.Result.CategoryId;

				// Add Core Feature to Category2
				CategoryAdminApiService.PostCategoryFeature(categoryId2.ToString(),
				new CategoryFeatureInsertRequest
				{
					FeatureId = categoryCoreFeatureId,
					FeatureTypeId = FeatureType.Core
				});

				//Assign category2 to product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId2 }
				});
				Log($"category2 with core features { categoryName2 } was added to Product { _productName }");

				//Navigates to category detail page of category2
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId2.ToString());

				//Assert that category2 is in unverified verification status after the category2 is added to the product
				Assert.AreEqual("Unverified", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category2 verification before selecting the Verify option is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Assert that the category2 core feature is added to the product and it appears selected
				getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var category2CoreFeatureSelectionStatus = getProductFeatures.Result.Single(x => x.CategoryId == categoryId2).Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(category2CoreFeatureSelectionStatus);

				//Get product categories to perform the product verification status on category2
				productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

				// Select Verify menu option for category1 in order to show the core feature selected 
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				//Navigates to category detail page of category2
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId2.ToString());

				//Assert that category2 is in verified verification status after the Verify option is selected
				Assert.AreEqual("Verified", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category verification status after selecting Verify option s {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Navigate to the product detail page to assert that the core feature from category was added to the products
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert that the category core features were added to the product and they appear selected
			    getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				category1CoreFeatureSelectionStatus = getProductFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(category1CoreFeatureSelectionStatus);

				category2CoreFeatureSelectionStatus = getProductFeatures.Result.Single(x => x.CategoryId == categoryId2).Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(category2CoreFeatureSelectionStatus);

				// Assert that the category core feature appears selected on the product detail page.
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(categoryCoreFeatureName);
				Assert.IsTrue(checkbox.IsSelected());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void CategoryFitVerification_CategoryVerificationStatusIsSetToRejectedWhenCategoryIsRemoved()
		{
			ExecuteTimedTest(() =>
			{
				//Open category page
				OpenPage(_categoriesPage);

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category with core features
				var categoryName = RequestUtility.GetRandomString(9);
				var (categoryId, _) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				//Get category core features
				var getCategoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var categoryCoreFeatureName = getCategoryFeatures.Result.Where(res => res.FeatureTypeId == FeatureType.Core).Select(cf => cf.Name).Single();

				//Navigate to category detail page
				Log($"Navigate to the category detail page for category Id {categoryId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

                // On the category card, select the verify option
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
                var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
                ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Rejected
				});

				// Refresh the category detail page and assert that verification status is set to Rejected
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
				Assert.AreEqual("Rejected", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Navigate to the product detail page to assert that the core feature(s) are not displayed. 
				Log($"Navigate to the product detail page for productId {productId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert that the category core feature was not added to the product and it appears unselected
				var getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var selectedCoreFeature = getProductFeatures.Result.Single().Features.Exists(res => res.FeatureName.Equals(categoryCoreFeatureName) && res.IsSelected);
				Assert.IsFalse(selectedCoreFeature);

				// Assert that the category core feature appears selected on the product detail page.
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(categoryCoreFeatureName);
				Assert.IsFalse(checkbox.IsSelected());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void CategoryFitVerification_AllProductCoreFeaturesAreAddedWhenCategoryIsSetBackToActive()
		{
			ExecuteTimedTest(() =>
			{
				//Open category page
				OpenPage(_categoriesPage);

				//Create Product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
				{
					Name = _productName,
					ProductWebsiteUrl = ProductWebsiteUrl
				}).Result.ProductId;

				//Create category with core features
				var categoryName = RequestUtility.GetRandomString(9);
				var (categoryId, _) = CreateCategoryProductWithCoreFeature(productId, categoryName);

				//Get category core features
				var getCategoryFeatures = CategoryAdminApiService.GetCategoryFeaturesById(categoryId.ToString());
				var categoryCoreFeatureName = getCategoryFeatures.Result.Where(x => x.FeatureTypeId == FeatureType.Core).Select(cf => cf.Name).Single();

				//Navigate to category detail page
				Log($"Navigate to the category detail page for category Id {categoryId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

				// On the category card, select the remove option				
				var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Rejected
				});

				// Refresh the category detail page and assert that verification status is set to Rejected
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
				Assert.AreEqual("Rejected", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");


				// On the category card, select the verify option				
				productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
				productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					Primary = true,
					StatusTypeId = ProductCategoryStatusType.Active,
					VerificationTypeId = ProductCategoryVerificationType.Verified
				});

				// Refresh the category detail page and assert that verification status is set to Verified
				BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
				Assert.AreEqual("Verified", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
				Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

				// Navigate to the product detail page to assert that the core feature(s) is selected
				Log($"Navigate to the product detail page for productId {productId.ToString()}");
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				// Assert that the category core feature was added to the product and it appears selected
				var getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var selectedCoreFeature = getProductFeatures.Result.Single().Features.Exists(x => x.FeatureName.Equals(categoryCoreFeatureName) && x.IsSelected);
				Assert.IsTrue(selectedCoreFeature);

				// Assert that the category core feature appears selected on the product detail page.
				var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(categoryCoreFeatureName);
				Assert.IsTrue(checkbox.IsSelected());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
			});
		}

		[Test]
		public void CategoryFitVerification_VerificationStatusTurnsToVerifiedWhenCategoryIsSetToActiveForProductsWithCoreFeaturesSelected()
		{
			ExecuteTimedTest(() =>
			{
   				   //Open category page
					OpenPage(_categoriesPage);

					//Create Product
					var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest
					{
						Name = _productName,
						ProductWebsiteUrl = ProductWebsiteUrl
					}).Result.ProductId;

					//Create category
					var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
					{ Name = RequestUtility.GetRandomString(9) });
					var categoryName = category.Result.Name;
					var categoryId = category.Result.CategoryId;

					// Create Category Core Feature1
					var feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
					{
						Name = RequestUtility.GetRandomString(10),
						Definition = RequestUtility.GetRandomString(10)
					}).Result;

					// Add Core Feature1 to Category
					CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
						new CategoryFeatureInsertRequest
						{
							FeatureId = feature.FeatureId,
							FeatureTypeId = FeatureType.Core
						});

					// Create Category Core Feature2
					feature = CategoryAdminApiService.PostFeature(new FeatureInsertRequest
					{
						Name = RequestUtility.GetRandomString(10),
						Definition = RequestUtility.GetRandomString(10)
					}).Result;

					// Add Core Feature2 to Category
					CategoryAdminApiService.PostCategoryFeature(categoryId.ToString(),
						new CategoryFeatureInsertRequest
						{
							FeatureId = feature.FeatureId,
							FeatureTypeId = FeatureType.Core
						});

					//Assign Category to Product
					ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
					{
						ProductId = productId,
						CategoryIds = new List<int> { categoryId }
					});
					Log($"category 1 {categoryName} with Core Features was added to Product {_productName}");


					//Navigate to category detail page
					Log($"Navigate to the category detail page for category Id {categoryId.ToString()}");
					BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());

					// On the category card, select the remove option				
					var productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
					var productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
					ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
					{
						Primary = true,
						StatusTypeId = ProductCategoryStatusType.Active,
						VerificationTypeId = ProductCategoryVerificationType.Rejected
					});

					// Refresh the category detail page and assert that verification status is set to Rejected
					BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
					Assert.AreEqual("Rejected", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
					Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

					// On the category card, select the verify option				
					productCategories = ProductAdminApiService.GetProductCategories(productId.ToString()).Result;
					productCategoryId = productCategories.Single(pc => pc.CategoryId == categoryId).ProductCategoryId;
					ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
					{
						Primary = true,
						StatusTypeId = ProductCategoryStatusType.Active,
						VerificationTypeId = ProductCategoryVerificationType.Verified
					});

					// Refresh the category detail page and assert that verification status is set to Verified
					BrowserUtility.NavigateToPage(BrowserUtility.CategoryPageName, categoryId.ToString());
					Assert.AreEqual("Verified", BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText());
					Log($"The category verification status displayed is {BasePage.GetTableCellByRowNumberAndColumnName(1, Model.Pages.CategoryDetailsPage.ColumnNameSelector.VerificationStatus).GetText()}");

					// Navigate to the product detail page to assert that the core feature from category was added to the products
					BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

					// Assert that the category core features was added to the product and they appear selected
					var getProductFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
					var productFeaturesSelectionStatus = getProductFeatures.Result.SelectMany(res => res.Features.Select(f => f.IsSelected)).ToArray();
				    var coreFeaturesSelected = productFeaturesSelectionStatus.All(f => true);
					Assert.IsTrue(coreFeaturesSelected);

					var productFeaturesName = getProductFeatures.Result.SelectMany(res => res.Features.Select(f => f.FeatureName)).ToArray();

					// Assert that the category core feature appears selected on the product detail page.
					var checkbox = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(productFeaturesName[0]);
					 Assert.IsTrue(checkbox.IsSelected());

					var checkbox2 = Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(productFeaturesName[1]);
					Assert.IsTrue(checkbox2.IsSelected());

					//Cleanup
					CategoryAdminApiService.DeleteCategory(categoryId.ToString());
					ProductAdminApiService.DeleteProduct(productId.ToString());
				});
		}
	}
}