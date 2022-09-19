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
	public class ProductNotesTests : BaseTest
	{
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _vendorName;
		private string _vendorWebsiteUrl;
		private string _notes;
		private string _productName;
		private string _productWebsiteUrl;

		private const string NotSetNotesText = "(Notes not yet set)";

		public ProductNotesTests() : base(nameof(ProductNotesTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productDetailsPage.OpenPage();
			_vendorName = RequestUtility.GetRandomString(8);
			_vendorWebsiteUrl = $"https://{RequestUtility.GetRandomString(8)}.com";
			_productName = RequestUtility.GetRandomString(8);
			_productWebsiteUrl = $"{_vendorWebsiteUrl}/{_productName}";
			_notes = RequestUtility.GetRandomString(50);
		}

		[Test]
		public void ProductNotes_VendorWithNotesDisplaysVendorNotes_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (vendor, product) = SetUpNotes(true);

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-vendor-notes");
				_productDetailsPage.LinkRightSidenavVendorNotes.Click();

				var expectedVendorLink = $"{BrowserUtility.BaseUri}vendors/{vendor.VendorId}";

				Assert.IsTrue(_productDetailsPage.IconRightSidenavVendorNotes.ExistsInPage());
				Assert.IsTrue(_productDetailsPage.VendorNotesGoToVendorMessage.IsDisplayed());
				Assert.AreEqual(expectedVendorLink, _productDetailsPage.LinkToVendorPage.GetHref());
				Assert.AreEqual(_notes, _productDetailsPage.VendorNotes.GetText());

				//Clean up
				ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ProductNotes_VendorWithoutNotesDoesNotDisplaysVendorNotes_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var (vendor, product) = SetUpNotes(false);

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-vendor-notes");
				_productDetailsPage.LinkRightSidenavVendorNotes.Click();

				var expectedVendorLink = $"{BrowserUtility.BaseUri}vendors/{vendor.VendorId}";

				Assert.IsFalse(_productDetailsPage.IconRightSidenavVendorNotes.ExistsInPage());
				Assert.IsTrue(_productDetailsPage.VendorNotesGoToVendorMessage.IsDisplayed());
				Assert.AreEqual(expectedVendorLink, _productDetailsPage.LinkToVendorPage.GetHref());
				Assert.AreEqual(NotSetNotesText, _productDetailsPage.VendorNotes.GetText());

				//Clean up
				ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
			});
		}

		private (VendorDto Vendor, ProductAdminDto Product) SetUpNotes(bool isAddNotes)
		{
			//Create vendor and add notes
			var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);
			if (isAddNotes)
				VendorAdminApiService.PutVendorNotes(vendor.VendorId, new VendorNotesUpdateRequest
				{
					Notes = _notes
				});

			//Create product
			var product = ProductAdminApiService.PostProduct(new ProductInsertRequest
			{
				Name = _productName,
				ProductWebsiteUrl = _productWebsiteUrl,
				VendorId = int.Parse(vendor.VendorId)
			}).Result;

			return (vendor, product);
		}
	}
}