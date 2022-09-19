using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Collections.Generic;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	public class SiteProductDisplayTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;

		public SiteProductDisplayTests() : base(nameof(SiteProductDisplayTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
		}

		[Test]
		[Category("Product")]
		public void GlobalProductSummary_ValidateSiteProductDisplay_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(9);
			var siteProductSaveRequest1 = RequestUtility.GetSourceSiteProductSaveRequest($"Z{RequestUtility.GetUniqueId()}");
			var siteProductSaveRequest2 = RequestUtility.GetSourceSiteProductSaveRequest($"A{RequestUtility.GetUniqueId()}");
			var siteProductSaveRequests = new List<SourceSiteProductSaveRequest> { siteProductSaveRequest1, siteProductSaveRequest2 };
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_productsPage);

				//Create a new product
				var productId = ProductAdminApiService.PostProduct(new ProductInsertRequest {Name = productName}).Result.ProductId;
				Log($"Product posted. ProductId: {productId}.");

				//Create 2 new site products
				var sourceSiteProductIds = IntegrationApiService.PostAndGetSiteProducts(siteProductSaveRequests);

				//Map both site products to the product
				ProductAdminApiService.PutProductToSiteProductMap(productId.ToString(), sourceSiteProductIds);
				Log($"Put site product mappings for {sourceSiteProductIds[0]} and {sourceSiteProductIds[1]}");

				//Navigate to product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Validate that both mapped site products show up correctly
				Assert.IsTrue(BasePage.GetCapterraChipByName(siteProductSaveRequest1.Name.ToLower()).IsDisplayed());
				Assert.IsTrue(BasePage.GetCapterraChipByName(siteProductSaveRequest2.Name.ToLower()).IsDisplayed());

				//Validate that the site products show up in descending order by siteProductId
				Assert.IsTrue(BasePage.GetCapterraChipByPosition(1).GetText().Contains(siteProductSaveRequest1.Name));
				Assert.IsTrue(BasePage.GetCapterraChipByPosition(2).GetText().Contains(siteProductSaveRequest2.Name));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId.ToString());
				IntegrationApiService.DeleteSourceSiteProducts(new List<string>{ siteProductSaveRequest1.ProductId, siteProductSaveRequest2.ProductId });
				Log("Cleanup completed.");
			});
		}
	}
}
