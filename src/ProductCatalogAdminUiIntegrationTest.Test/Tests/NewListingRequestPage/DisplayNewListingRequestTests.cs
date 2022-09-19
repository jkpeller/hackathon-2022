using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.NewListingRequestPage
{
	public class DisplayNewListingRequestTests : BaseTest
	{
		private Model.Pages.NewListingRequestPage _page;
		private string _companyName;
		private string _companyWebsiteUrl;
		private string _productName;
		private string _productWebsiteUrl;

		public DisplayNewListingRequestTests() : base(nameof(DisplayNewListingRequestTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.NewListingRequestPage();
			_companyName = RequestUtility.GetRandomString(10);
			_companyWebsiteUrl = $"https://{_companyName}.com/test";
			_productName = RequestUtility.GetRandomString(10);
			_productWebsiteUrl = $"https://{_companyName}.com/{_productName}";
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateDisplay_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(result.ListingRequestId);

				//validate that all of the information on the page is correct
				Assert.AreEqual(result.CompanyName, _page.ListingRequestCompanyName.GetText());
				Assert.AreEqual(result.CompanyWebsiteUrl, _page.ListingRequestCompanyUrl.GetText());
				Assert.AreEqual($"{result.CompanyContactFirstName} {result.CompanyContactLastName}", 
					_page.ListingRequestContactFullName.GetText());
				Assert.AreEqual(result.CompanyContactEmail, _page.ListingRequestContactEmail.GetText());
				Assert.AreEqual($"{result.CompanyCity}, {result.CompanyStateProvinceRegionName}, United States of America",
					_page.ListingRequestAddress.GetText());
				Assert.AreEqual(result.CompanyPhoneNumber, _page.ListingRequestPhoneNumber.GetText());
				Assert.AreEqual(SocialMediaUtility.LinkedInUrl, _page.ListingRequestLinkedinUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.FacebookUrl, _page.ListingRequestFacebookUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.TwitterUrl, _page.ListingRequestTwitterUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.YoutubeUrl, _page.ListingRequestYoutubeUrl.GetText());
				Assert.AreEqual(SocialMediaUtility.InstagramUrl, _page.ListingRequestInstagram.GetText());
				Assert.AreEqual(result.ProductName, _page.ListingRequestProductName.GetText());
				Assert.AreEqual(result.ProductShortDescription, _page.ListingRequestProductDesc.GetText());
				Assert.AreEqual("(Product URL not set)", _page.ListingRequestProductUrl.GetText());
				Assert.AreEqual(result.ProductProposedCategoryName, _page.ListingRequestProposedCategory.GetText());
				Assert.AreEqual("(not set)", _page.ListingRequestApprovedCategory.GetText());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateCompanyUrl_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(result.ListingRequestId);

				//validate that the company url link contains the correct URL
				Assert.AreEqual(result.CompanyWebsiteUrl.ToLower(), _page.ListingRequestCompanyUrl.GetHref());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateSocialMediaUrls_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(result.ListingRequestId);

				//validate that all of the information on the page is correct
				Assert.AreEqual(SocialMediaUtility.LinkedInUrl, _page.ListingRequestLinkedinUrl.GetHref());
				Assert.AreEqual(SocialMediaUtility.TwitterUrl, _page.ListingRequestTwitterUrl.GetHref());
				Assert.AreEqual(SocialMediaUtility.FacebookUrl, _page.ListingRequestFacebookUrl.GetHref());
				Assert.AreEqual(SocialMediaUtility.YoutubeUrl, _page.ListingRequestYoutubeUrl.GetHref());
				Assert.AreEqual(SocialMediaUtility.InstagramUrl, _page.ListingRequestInstagram.GetHref());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ValidateBackButton_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(result.ListingRequestId);

				//click the back button and validate that the user is returned to the new listing request table
				_page.ButtonGoBack.ClickAndWaitForPageToLoad();
				Log("Back button was pressed.");
				Assert.AreEqual("New Listing Requests", _page.PageTitle.GetText());
			});
		}

		//TODO: when we have the PUT for listing request, add a test to validate the ProductWebsiteUrl

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_ByNullSocialMediaUrls_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();
				var result = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, false);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(result.ListingRequestId);

				//validate that the product url and description fields show the correct message for being null
				Assert.AreEqual("(not set)", _page.ListingRequestSocialMedia.GetText());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_WithProvidedVendorId_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

                //create a listing request using the same name/url from the vendor
                var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, false, true);

                //open the page and then go to the page for the newly-created listing request
                NavigateToNewListingRequest(listingRequest.ListingRequestId);
                Thread.Sleep(1000);

                //validate that the page does not show the duplicate vendor alert if VendorId is provided
                Assert.IsFalse(_page.DuplicateVendorsNoneDetected.IsDisplayed());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_WithDuplicateVendor_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//set up a vendor with the same name/url that will be used to create the listing request
				var vendor = PostVendor(_companyName, _companyWebsiteUrl);

				//create a listing request using the same name/url from the vendor
				var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, false);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(listingRequest.ListingRequestId);
				Thread.Sleep(1000);

				//validate that the page displays a suggested duplicate vendor
				Assert.IsTrue(_page.DuplicateVendorsDetected.IsDisplayed());
				Assert.IsTrue(Model.Pages.NewListingRequestPage.GetDuplicateVendorLinkByVendorName(_companyName).IsDisplayed());
				var duplicateVendorText = Model.Pages.NewListingRequestPage.GetDuplicateVendorLinkByVendorName(_companyName).GetText();
				Assert.IsTrue(duplicateVendorText.Contains(_companyName));
				Assert.IsTrue(duplicateVendorText.Contains(_companyWebsiteUrl));

				//cleanup
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_WithoutDuplicateVendor_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//create a listing request using a unique vendor name and url
				var listingRequest = SetupNewListingRequestWithAllValidFields(_companyName, _companyWebsiteUrl, false);

				//open the page and then go to the page for the newly-created listing request
				NavigateToNewListingRequest(listingRequest.ListingRequestId);

				//validate that the page displays that no duplicate vendors were detected
				Assert.IsTrue(_page.DuplicateVendorsNoneDetected.IsDisplayed());
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_WithDuplicateVendorProduct_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//create a new vendor
				var vendor = PostVendor(_companyName, _companyWebsiteUrl);
				var vendorId = int.Parse(vendor.VendorId);

				//create a product associated with the vendor created above
				var product = PostProduct(_productName, _productWebsiteUrl, false, vendorId);

				//create a listing request with the existing vendorId created above and the same product name as the product created above
				var listingRequest = SetupNewListingRequestWithExistingVendor(vendor.VendorId, _productName);

				//open the page for the newly-created listing request
				NavigateToNewListingRequest(listingRequest.ListingRequestId);
				Thread.Sleep(1000);

				//validate that the page displays a duplicate vendor product was detected
				Assert.IsTrue(_page.DuplicateVendorProductDetected.IsDisplayed());
				Assert.IsTrue(Model.Pages.NewListingRequestPage.GetDuplicateVendorProductLinkByProductName(_productName).IsDisplayed());
				var duplicateVendorProductText = Model.Pages.NewListingRequestPage.GetDuplicateVendorProductLinkByProductName(_productName).GetText();
				Assert.IsTrue(duplicateVendorProductText.Contains(_productName));

				//cleanup
				ProductAdminApiService.DeleteProduct(product.ProductId);
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		[Category("ListingRequest")]
		public void DisplayNewListingRequest_WithoutDuplicateVendorProduct_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				_page.OpenPage();

				//create a new vendor
				var vendor = PostVendor(_companyName, _companyWebsiteUrl);

				//create a listing request with the existing vendorId created above
				var listingRequest = SetupNewListingRequestWithExistingVendor(vendor.VendorId);

				//open the page for the newly-created listing request
				NavigateToNewListingRequest(listingRequest.ListingRequestId);
				Thread.Sleep(1000);

				//validate that the page displays no duplicate vendor product was detected
				Assert.IsTrue(_page.DuplicateVendorProductNoneDetected.IsDisplayed());

				//cleanup
				DeleteVendor(vendor.VendorId);
			});
		}

		private void NavigateToNewListingRequest(int listingRequestId)
		{
			//open the page and then go to the page for the newly-created listing request
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.BaseUri + $"{BrowserUtility.ListingRequestPageName}/{listingRequestId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(2000);
		}
	}
}