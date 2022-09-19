using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	[Category("Product")]
	public class ProductLogoTests : BaseTest
	{
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private string _productWebsiteUrl;

		public ProductLogoTests() : base(nameof(ProductLogoTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
			_productWebsiteUrl = $"https://{_productName}.com";
			OpenPage(_productDetailsPage);
		}

		[Test]
		public void ProductLogo_BySmallLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (product, logo) = PostProduct(_productName, _productWebsiteUrl, true);
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());

				//Assert the product logo is 200x200
				AssertThumbnailHeightAndWidth(_productDetailsPage.ImageProductLogo);

				//Cleanup
				ProductAdminApiService.DeleteProductFile(logo.ProductFileId.ToString());
				ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
			});
		}

		[Test]
		public void ProductLogo_ByLargeLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (product, logo) = PostProduct(_productName, _productWebsiteUrl);
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());

				//Assert the product logo is 200x200
				AssertThumbnailHeightAndWidth(_productDetailsPage.ImageProductLogo);

				//Cleanup
				ProductAdminApiService.DeleteProductFile(logo.ProductFileId.ToString());
				ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
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