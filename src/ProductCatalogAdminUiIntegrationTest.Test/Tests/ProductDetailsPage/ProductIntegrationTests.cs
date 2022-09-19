using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	[Category("Product")]
	public class ProductIntegrationTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName1;
		private string _productName2;
		private string _productWebsiteUrl1;
		private string _productWebsiteUrl2;
		
		public ProductIntegrationTests() : base(nameof(ProductIntegrationTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName1 = RequestUtility.GetRandomString(8);
			_productName2 = RequestUtility.GetRandomString(8);
			_productWebsiteUrl1 = $"https://{_productName1}.com";
			_productWebsiteUrl2 = $"https://{_productName2}.com";
		}

		[Test]
		public void ProductIntegration_ByNoIntegrations_DisplaysNoResultsMessage()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");

				//Assert the no results message is displayed
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				Assert.IsTrue(_productDetailsPage.ProductIntegrationNoResultsMessage.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
			});
		}

		[Test]
		public void ProductIntegration_ByValidIntegrationsAndSave_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");
				var (product2, _) = PostProduct(_productName2, _productWebsiteUrl2);
				Log($"Created product successfully. ProductId: {product2.ProductId}, Name: {product2.Name}");

				//Select open API yes
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				_productDetailsPage.SelectOpenApi.Click();
				Model.Pages.ProductDetailsPage.GetOpenApiOption(VerifiedType.Yes).Click();
				Log($"Selected open API option. VerifiedType: {VerifiedType.Yes}");

				//Add product integration and save
				_productDetailsPage.InputProductIntegrationName.SendKeys(_productName2, sendEscape: false);
				Thread.Sleep(2500);
				Model.Pages.ProductDetailsPage.GetProductIntegrationOptionByName(_productName2).Click();
				_productDetailsPage.ButtonAddProductIntegration.Click();
				Log($"Selected integration product. ProductId: {product2.ProductId}, Name: {product2.Name}");
				_productDetailsPage.ButtonSaveProductIntegration.Click();
				Log("Clicked the save product integration button.");
				Thread.Sleep(2500);

				//Assert the product integration successfully saved
				var getResponse = ProductAdminApiService.GetProductIntegrations(productId1);
				Log("Product integrations retrieved successfully.");
				Assert.AreEqual(VerifiedType.Yes, getResponse.Result.OpenApiOfferedVerifieds.SingleOrDefault(v => v.IsSelected)?.VerifiedTypeId);
				Assert.AreEqual(product2.ProductId, getResponse.Result.Integrations.SingleOrDefault()?.ProductId);
				var expectedProductLink = $"{BrowserUtility.BaseUri}products/{product2.ProductId}";
				Assert.AreEqual(expectedProductLink, Model.Pages.ProductDetailsPage.GetLinkProductIntegration(product2.ProductId.ToString()).GetHref());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(product2.ProductId.ToString());
			});
		}

		[Test]
		public void ProductIntegration_ByValidIntegrationsAndCancel_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");
				var (product2, _) = PostProduct(_productName2, _productWebsiteUrl2);
				Log($"Created product successfully. ProductId: {product2.ProductId}, Name: {product2.Name}");

				//Select open API yes
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				_productDetailsPage.SelectOpenApi.Click();
				Model.Pages.ProductDetailsPage.GetOpenApiOption(VerifiedType.Yes).Click();
				Log($"Selected open API option. VerifiedType: {VerifiedType.Yes}");

				//Add product integration and cancel
				_productDetailsPage.InputProductIntegrationName.SendKeys(product2.Name, sendEscape: false);
				Thread.Sleep(2500);
				Model.Pages.ProductDetailsPage.GetProductIntegrationOptionByName(product2.Name).Click();
				_productDetailsPage.ButtonAddProductIntegration.Click();
				Log($"Selected integration product. ProductId: {product2.ProductId}, Name: {product2.Name}");
				_productDetailsPage.ButtonCancelProductIntegration.Click();
				Log("Clicked the cancel product integration button.");
				Thread.Sleep(2500);
				
				//Assert the product integration was not saved
				Assert.IsEmpty(_productDetailsPage.SelectOpenApi.GetText().Trim());
				Assert.IsTrue(_productDetailsPage.ProductIntegrationNoResultsMessage.IsDisplayed());
				var getResponse = ProductAdminApiService.GetProductIntegrations(productId1);
				Log("Product integrations retrieved successfully.");
				Assert.IsTrue(getResponse.Result.OpenApiOfferedVerifieds.All(v => !v.IsSelected));
				Assert.IsEmpty(getResponse.Result.Integrations);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(product2.ProductId.ToString());
			});
		}

		[Test]
		public void ProductIntegration_ByValidIntegrationsAndRemove_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");
				var (product2, _) = PostProduct(_productName2, _productWebsiteUrl2);
				Log($"Created product successfully. ProductId: {product2.ProductId}, Name: {product2.Name}");

				//Select open API yes
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				_productDetailsPage.SelectOpenApi.Click();
				Model.Pages.ProductDetailsPage.GetOpenApiOption(VerifiedType.Yes).Click();
				Log($"Selected open API option. VerifiedType: {VerifiedType.Yes}");

				//Add product integration, save, and remove
				_productDetailsPage.InputProductIntegrationName.SendKeys(product2.Name, sendEscape: false);
				Thread.Sleep(2500);
				Model.Pages.ProductDetailsPage.GetProductIntegrationOptionByName(product2.Name).Click();
				_productDetailsPage.ButtonAddProductIntegration.Click();
				Log($"Selected integration product. ProductId: {product2.ProductId}, Name: {product2.Name}");
				_productDetailsPage.ButtonSaveProductIntegration.Click();
				Log("Clicked the save product integration button.");
				Thread.Sleep(2500);
				Model.Pages.ProductDetailsPage.GetButtonRemoveProductIntegration(product2.Name).Click();
				Log("Clicked the remove product integration button.");
				_productDetailsPage.ButtonSaveProductIntegration.Click();
				Log("Clicked the save product integration button.");
				Thread.Sleep(2500);

				//Assert the product integration was removed
				Assert.IsTrue(_productDetailsPage.ProductIntegrationNoResultsMessage.IsDisplayed());
				var getResponse = ProductAdminApiService.GetProductIntegrations(productId1);
				Log("Product integrations retrieved successfully.");
				Assert.AreEqual(VerifiedType.Yes, getResponse.Result.OpenApiOfferedVerifieds.SingleOrDefault(v => v.IsSelected)?.VerifiedTypeId);
				Assert.IsEmpty(getResponse.Result.Integrations);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(product2.ProductId.ToString());
			});
		}

		[Test]
		public void ProductIntegrationLogo_BySmallLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1, navigateToPage: false);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");
				var (product2, productLogo2) = PostProduct(_productName2, _productWebsiteUrl2, true);
				Log($"Created product successfully. ProductId: {product2.ProductId}, Name: {product2.Name}");
				ProductAdminApiService.PutProductIntegrations(productId1, new ProductIntegrationsUpdateRequest
				{
					OpenApiOfferedVerifiedTypeId = VerifiedType.Unverified,
					IntegrationProductIds = new List<int>
					{
						product2.ProductId
					}
				});
				Log($"Created product integration successfully. ProductId: {productId1}, Name: {_productName1}, IntegratedProductId: {product2.ProductId}, IntegratedProductName: {product2.Name}");

				//Assert the product integration logo is 200x200
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId1);
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				AssertThumbnailHeightAndWidth(Model.Pages.ProductDetailsPage.GetImageProductIntegrationLogo(product2.ProductId.ToString()));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productLogo2.ProductFileId.ToString());
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(product2.ProductId.ToString());
			});
		}

		[Test]
		public void ProductIntegrationLogo_ByLargeLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId1, _) = PostAndNavigateToProduct(_productsPage, _productName1, _productWebsiteUrl1, navigateToPage: false);
				Log($"Created product successfully. ProductId: {productId1}, Name: {_productName1}");
				var (product2, productLogo2) = PostProduct(_productName2, _productWebsiteUrl2);
				Log($"Created product successfully. ProductId: {product2.ProductId}, Name: {product2.Name}");
				ProductAdminApiService.PutProductIntegrations(productId1, new ProductIntegrationsUpdateRequest
				{
					OpenApiOfferedVerifiedTypeId = VerifiedType.Unverified,
					IntegrationProductIds = new List<int>
					{
						product2.ProductId
					}
				});
				Log($"Created product integration successfully. ProductId: {productId1}, Name: {_productName1}, IntegratedProductId: {product2.ProductId}, IntegratedProductName: {product2.Name}");

				//Assert the product integration logo is 200x200
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId1);
				_productDetailsPage.LinkRightSidnavProductIntegrations.Click();
				AssertThumbnailHeightAndWidth(Model.Pages.ProductDetailsPage.GetImageProductIntegrationLogo(product2.ProductId.ToString()));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productLogo2.ProductFileId.ToString());
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(product2.ProductId.ToString());
			});
		}

		private (ProductAdminDto ProductDto, ProductLogoDto ProductLogoDto) PostProduct(string name, string websiteUrl, bool isSmallLogo = false)
		{
			var productRequest = new ProductInsertRequest
			{
				Name = name,
				ProductWebsiteUrl = websiteUrl
			};
			var productDto = ProductAdminApiService.PostProduct(productRequest).Result;

			var (content, extension) = GetBase64Asset(isSmallLogo ? SoftwareAdviceLogo50x50FileName : SoftwareAdviceLogoFileName);
			var productLogoDto = ProductAdminApiService.PostProductLogo(productDto.ProductId.ToString(),
				new FileRequest
				{
					Content = content,
					Extension = extension
				}).Result;
			return (productDto, productLogoDto);
		}
	}
}