using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	[Category("Product")]
	public class AddProductTests : BaseTest
	{
		private readonly Model.Pages.VendorPage _vendorPage;
		private readonly Model.Pages.ProductAddPage _productAddPage;
		private readonly string _vendorName;
		private readonly string _vendorWebsiteUrl;

		public AddProductTests() : base(nameof(AddProductTests))
		{
			_vendorPage = new Model.Pages.VendorPage();
			_productAddPage = new Model.Pages.ProductAddPage();
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
		}

		[Test]
		public void ProductAdd_ByValidRequiredFields_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(10);
			var productWebsiteUrl = $"https://www.{_vendorName}.com/{productName}";
			ExecuteTimedTest(() =>
			{
				//Setup
				_vendorPage.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenAddProductScreen(vendor.VendorId);

				//Input the name of the product
				_productAddPage.InputProductName.SendKeys(productName, true);
				Log($"Typed {productName} into the product name input field.");

				//Input product website url
				_productAddPage.InputProductWebsite.SendKeys(productWebsiteUrl, true);
				Log($"Typed {productWebsiteUrl} into the product website url input field.");

				//Save the product
				_productAddPage.ButtonSubmitProductForm.ClickAndWaitForPageToLoad();
				Log("Clicked the create product button.");

				var productId = ProductAdminApiService.SearchProducts(new ProductSearchRequest { TextFilter = productName }).Result.Single().ProductId.ToString();
				var getResponse = ProductAdminApiService.GetProductById(productId);

				//Assert the product name and website url using the Api
				Assert.AreEqual(productName, getResponse.Result.Name);
				Assert.AreEqual(productWebsiteUrl, getResponse.Result.ProductWebsiteUrl);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ProductAdd_WithoutDuplicateVendorProduct_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(10);
			ExecuteTimedTest(() =>
			{
				//Setup
				_vendorPage.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenAddProductScreen(vendor.VendorId);

				//Input the name of the product
				_productAddPage.InputProductName.SendKeys(productName, true);
				Log($"Typed {productName} into the product name input field.");
				Thread.Sleep(3000);

				//Assert the page displays no duplicate vendor product was detected
				Assert.IsTrue(_productAddPage.DuplicateVendorProductNoneDetected.IsDisplayed());

				//Cleanup
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ProductAdd_DuplicateProductVendorDetected_Succeeds()
		{
			var product1Name = RequestUtility.GetRandomString(10);
			var product2Name = RequestUtility.GetRandomString(10);
			var _productWebsiteUrl1 = $"https://www.{product1Name}.com/";
			var _productWebsiteUrl2 = $"https://www.{product2Name}.com/";

			ExecuteTimedTest(() =>
			{
				//Setup
				_vendorPage.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenAddProductScreen(vendor.VendorId);

				//Input the name of the product1
				_productAddPage.InputProductName.SendKeys(product1Name, true);
				_productAddPage.InputProductWebsite.SendKeys(_productWebsiteUrl1, true);
				Log($"Typed {product1Name} into the product name input field.");
				_productAddPage.ButtonSubmitProductForm.Click();
				Thread.Sleep(2000);

				//Input the name of the product2
				OpenAddProductScreen(vendor.VendorId);
				_productAddPage.InputProductName.SendKeys(product2Name, true);
				_productAddPage.InputProductWebsite.SendKeys(_productWebsiteUrl2, true);
				Log($"Typed {product2Name} into the product name input field.");
				_productAddPage.ButtonSubmitProductForm.Click();
				Thread.Sleep(2000);

				//Input a duplicate a product name
				OpenAddProductScreen(vendor.VendorId);
				_productAddPage.InputProductName.SendKeys(product1Name, true);
				Log($"Typed a duplicate product name in the input field.");
				Thread.Sleep(3000);

				//Assert that the page displays duplicate vendor product detected
				Assert.IsTrue(_productAddPage.DuplicateVendorProductDetected.IsDisplayed());

				//Cleanup
				var product1Id = ProductAdminApiService.GetProductsByPartialName(product1Name).Result.Single().ProductId.ToString();
				var product2Id = ProductAdminApiService.GetProductsByPartialName(product2Name).Result.Single().ProductId.ToString();
				ProductAdminApiService.DeleteProduct(product1Id);
				ProductAdminApiService.DeleteProduct(product2Id);
				DeleteVendor(vendor.VendorId);
			});
		}
		
		[Test]
		public void ProductAdd_NoDuplicateProductFoundCheckMark_Succeeds()
		{
			var product1Name = RequestUtility.GetRandomString(10);
			var product2Name = RequestUtility.GetRandomString(10);
			var _productWebsiteUrl1 = $"https://www.{product1Name}.com/";
			var _productWebsiteUrl2 = $"https://www.{product2Name}.com/";

			ExecuteTimedTest(() =>
			{
				//Setup
				_vendorPage.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
				OpenAddProductScreen(vendor.VendorId);

				//Input the name of the product1
				_productAddPage.InputProductName.SendKeys(product1Name, true);
				_productAddPage.InputProductWebsite.SendKeys(_productWebsiteUrl1, true);
				Log($"Typed {product1Name} into the product name input field.");
				_productAddPage.ButtonSubmitProductForm.Click();
				Thread.Sleep(2000);

				//Input the name of the product2
				OpenAddProductScreen(vendor.VendorId);
				_productAddPage.InputProductName.SendKeys(product2Name, true);
				_productAddPage.InputProductWebsite.SendKeys(_productWebsiteUrl2, true);
				Log($"Typed {product2Name} into the product name input field.");
				_productAddPage.ButtonSubmitProductForm.Click();
				Thread.Sleep(2000);

				//Input a product name does not match any of the vendor's other product names
				OpenAddProductScreen(vendor.VendorId);
				_productAddPage.InputProductName.SendKeys(RequestUtility.GetRandomString(10), true);
				Log($"Typed a product name does not match any of the vendor's other product names.");
				Thread.Sleep(2000);

				//Assert that the page displays a No duplicate product found with green checkmark
				Assert.IsTrue(_productAddPage.DuplicateVendorProductNoneDetected.IsDisplayed());

				//Cleanup
				var product1Id = ProductAdminApiService.GetProductsByPartialName(product1Name).Result.Single().ProductId.ToString();
				var product2Id = ProductAdminApiService.GetProductsByPartialName(product2Name).Result.Single().ProductId.ToString();
				ProductAdminApiService.DeleteProduct(product1Id);
				ProductAdminApiService.DeleteProduct(product2Id);
				DeleteVendor(vendor.VendorId);
			});
		}


		[Test]
		public void ProductAdd_WithDuplicateVendorProduct_Succeeds()
		{
			var productName = RequestUtility.GetRandomString(10);
			var productWebsiteUrl = $"https://www.{_vendorName}.com/{productName}";
			ExecuteTimedTest(() =>
			{
				//Setup
				_vendorPage.OpenPage();
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//Create a product associated with the vendor created above
				var product = PostProduct(productName, productWebsiteUrl, false, int.Parse(vendor.VendorId));
				OpenAddProductScreen(vendor.VendorId);

				//Input the name of the product
				_productAddPage.InputProductName.SendKeys(productName, true);
				Log($"Typed {productName} into the product name input field.");
				Thread.Sleep(3000);

				//Assert the page displays duplicate vendor product was detected
				Assert.IsTrue(_productAddPage.DuplicateVendorProductDetected.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(product.ProductId);
				DeleteVendor(vendor.VendorId);
			});
		}

		private void OpenAddProductScreen(string vendorId)
		{
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorId, 3000);

			//Click to edit the vendor
			_vendorPage.ButtonAddVendorProduct.ClickAndWaitForPageToLoad();
			Log("Clicked the add product button.");
		}
	}
}