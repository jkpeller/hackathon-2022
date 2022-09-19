using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.VendorPage
{
	[TestFixture]
	[Category("Vendor")]
	public class VendorLogoTests : BaseTest
	{
		private Model.Pages.VendorPage _page;
		private string _vendorName;
		private string _vendorWebsiteUrl;
		private const string ButtonAddLogoText = "ADD LOGO";
		private const string ButtonChangeLogoText = "CHANGE LOGO";

		public VendorLogoTests() : base(nameof(VendorLogoTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.VendorPage();
			_vendorName = RequestUtility.GetRandomString(9);
			_vendorWebsiteUrl = $"https://www.{_vendorName}.com/";
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_ByValidLogo_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Assert that the logo is on the page
				Assert.IsTrue(_page.ImageVendorLogo.IsDisplayed());

				//Validate that the replace and remove buttons are present
				_page.ImageVendorLogo.HoverOver();
				Assert.IsTrue(_page.ButtonAddChangeLogo.IsDisplayed());
				Assert.AreEqual(ButtonChangeLogoText, _page.ButtonAddChangeLogo.GetText());
				Assert.IsTrue(_page.ButtonRemoveLogo.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_RemoveLogo_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Click to remove the logo
				_page.ButtonRemoveLogo.ClickAndWaitForPageToLoad();
				Log("Clicked the remove logo button.");

				//Assert that the logo is not displayed and Add Logo button is displayed
				Assert.IsFalse(_page.ImageVendorLogo.IsDisplayed());
				Assert.IsTrue(_page.ButtonAddChangeLogo.IsDisplayed());
				Assert.AreEqual(ButtonAddLogoText, _page.ButtonAddChangeLogo.GetText());

				//Click save
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();

				//Assert that the logo is gone and the card is in readonly mode
				Assert.IsFalse(_page.ImageVendorLogo.IsDisplayed());
				Assert.IsTrue(_page.ButtonEditVendor.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_ReplaceLogo_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);
				var insertedLogo = _page.ImageVendorLogo.GetImageSrc();

				//Click to replace the logo
				_page.ButtonAddChangeLogo.Click();
				Log("Clicked to replace the logo");
				Thread.Sleep(1000);

				//Select another logo
				SelectAssetForOpenFileDialog(CapterraLogoFileName);

				Assert.AreNotEqual(insertedLogo, _page.ImageVendorLogo.GetImageSrc());

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_AddLogoAndSave_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Save the vendor
				_page.ButtonSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the save vendor button");

				//Assert that the logo is on the page and the card is in readonly mode
				Assert.IsTrue(_page.ImageVendorLogo.IsDisplayed());
				Assert.IsTrue(_page.ButtonEditVendor.IsDisplayed());
				
				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_AddLogoAndCancel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Cancel changes
				_page.ButtonCancelSubmitVendorForm.ClickAndWaitForPageToLoad();
				Log("Clicked the cancel button");

				//Assert that the logo is not on the page
				Assert.IsFalse(_page.ImageVendorLogo.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		[Ignore("Vendor logo tests do not work in headless mode")]
		public void ValidateVendorLogo_ByLogoAboveMaximumFileSize_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendorId = SetUpPage();

				//Select a logo
				SelectAssetForOpenFileDialog(LargeFileName);

				//Assert that the logo is not on the page
				Assert.IsFalse(_page.ImageVendorLogo.IsDisplayed());

				//Cleanup the vendor
				DeleteVendor(vendorId);
			});
		}

		[Test]
		public void VendorLogo_BySmallLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendor = PostVendorWithLogo(SoftwareAdviceLogo50x50FileName);
				
				//Assert the vendor logo is 200x200
				AssertThumbnailHeightAndWidth(_page.ImageVendorLogo);

				//Cleanup
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void VendorLogo_ByLargeLogo_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Setup
				var vendor = PostVendorWithLogo(SoftwareAdviceLogoFileName);

				//Assert the vendor logo is 200x200
				AssertThumbnailHeightAndWidth(_page.ImageVendorLogo);

				//Cleanup
				VendorAdminApiService.DeleteVendor(vendor.VendorId);
			});
		}

		private string SetUpPage()
		{
			_page.OpenPage();
			var vendorId = PostVendor(_vendorName, _vendorWebsiteUrl).VendorId;
			BrowserUtility.WebDriver.Navigate().GoToUrl(BrowserUtility.WebDriver.Url + $"/{vendorId}");
			BrowserUtility.WaitForPageToLoad();
			BrowserUtility.WaitForOverlayToDisappear();
			Thread.Sleep(2500);

			//Click to edit the vendor
			_page.ButtonEditVendor.ClickAndWaitForPageToLoad();
			Log("Clicked on the edit vendor button.");

			//Click to add a logo
			_page.ContainerVendorLogo.HoverOver();
			_page.ButtonAddChangeLogo.Click();
			Log("Clicked to add a logo");
			Thread.Sleep(2500);

			return vendorId;
		}

		private VendorDto PostVendorWithLogo(string vendorLogoAssetName)
		{
			_page.OpenPage();
			var vendorDto = PostVendor(_vendorName, _vendorWebsiteUrl);

			var (content, extension) = GetBase64Asset(vendorLogoAssetName);
			VendorAdminApiService.PutVendorLogo(vendorDto.VendorId, new VendorLogoUpsertRequest
			{
				Content = content,
				Extension = extension
			});
			
			BrowserUtility.NavigateToPage(BrowserUtility.VendorPageName, vendorDto.VendorId);
			return vendorDto;
		}
	}
}