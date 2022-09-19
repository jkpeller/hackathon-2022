using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	public class ProductFeatureTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;

		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";

		public ProductFeatureTests() : base(nameof(SiteProductDisplayTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
		}

		[Test]
		[Category("Product")]
		public void ProductFeatures_SaveCoreFeatureState_Succeed()
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

				// Create Core Feature
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

				//Refresh Product Detail Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"category 1 with core features {categoryName} was added to Product {_productName}");

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Click the core feature and click the save button on the feature card
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var featureName = productFeatures.Result.FirstOrDefault().Features.Single().FeatureName;
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the core feature is selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && x.IsSelected));

				//Click core checkbox feature to uncheck the core feature and click the save button on feature card
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the core feature is not selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && !x.IsSelected));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductFeatures_SaveCommonFeatureState_Succeed()
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

				// Create Core Feature
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
						FeatureTypeId = FeatureType.Common
					});

				//Refresh Product Detail Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"category 1 with common features {categoryName} was added to Product {_productName}");

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Click the common feature and click save button on feature card
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var featureName = productFeatures.Result.FirstOrDefault().Features.Single().FeatureName;
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the common feature is selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && x.IsSelected));

				//Click the common checkbox feature to uncheck the common feature and click the save button on feature card
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the common feature is not selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && !x.IsSelected));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductFeatures_SaveOptionalFeatureState_Succeed()
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

				// Create Core Feature
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
						FeatureTypeId = FeatureType.Optional
					});

				//Refresh Product Detail Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"category 1 with optional features {categoryName} was added to Product {_productName}");

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Click the optional feature and click save button on feature card
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var featureName = productFeatures.Result.FirstOrDefault().Features.Single().FeatureName;
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the optional feature is selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && x.IsSelected));

				//Click the optional feature checkbox to uncheck the optional feature and click the save button on feature card
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the optional feature is not selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && !x.IsSelected));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductFeatures_SaveDifferentiatorFeatureState_Succeed()
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

				// Create Core Feature
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
						FeatureTypeId = FeatureType.Differentiator
					});

				//Refresh Product Detail Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"category 1 with optional features {categoryName} was added to Product {_productName}");

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Click the differentiator feature and click the save button on feature card
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var featureName = productFeatures.Result.FirstOrDefault().Features.Single().FeatureName;
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the differentiator feature is selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && x.IsSelected));

				//Click checkbox to uncheck the differentiator feature and click save button on feature card
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the differentiator feature is not selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && !x.IsSelected));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductFeatures_SaveUnverifiedFeatureState_Succeed()
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

				// Create Core Feature
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
						FeatureTypeId = FeatureType.Optional
					});

				//Refresh Product Detail Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Assign Category1 to Product
				ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
				{
					ProductId = productId,
					CategoryIds = new List<int> { categoryId }
				});
				Log($"category 1 with optional features {categoryName} was added to Product {_productName}");

				//Refresh Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Click unverified feature and click save button on feature card
				var productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				var featureName = productFeatures.Result.FirstOrDefault().Features.Single().FeatureName;
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the unverified feature is selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && x.IsSelected));

				//Click the checkbox to uncheck the unverified feature and click the save button on feature card
				Model.Pages.ProductDetailsPage.GetFeatureCheckboxByFeatureName(featureName).Click();
				_productDetailsPage.ButtonSaveFeatures.Click();
				Thread.Sleep(2000);

				//Assert that the unverified feature is not selected
				productFeatures = ProductAdminApiService.GetProductCategoryFeatures(productId);
				Assert.IsTrue(productFeatures.Result.Single(x => x.CategoryId == categoryId).Features.Exists(x => x.FeatureName.Equals(featureName) && !x.IsSelected));

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId.ToString());
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}
	}
}
