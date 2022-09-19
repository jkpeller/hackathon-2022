using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	[Category("Product")]
	public class ProductMediaTests : BaseTest
	{
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private string _productWebsiteUrl;
		private string _vendorName;
		private string _vendorWebsiteUrl;
		private const string UpdateUrl = "http://www.vimeo.com/test";

		public ProductMediaTests() : base(nameof(ProductMediaTests)) { }

		[SetUp]
		public void SetUp()
		{
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productDetailsPage.OpenPage();
			_productName = RequestUtility.GetRandomString(10);
			_vendorName = RequestUtility.GetRandomString(10);
		    _vendorWebsiteUrl = $"https://{_vendorName}.com/{_vendorName}";
		    _productWebsiteUrl = $"https://{_productName}.com/{_productName}";
		}

		[Test]
		public void ProductMedia_UpdateVideoUrl_Capterra_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.Capterra);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var videos = GetMedias(productId, SiteType.Capterra).Videos;

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Update
				var videoId = videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(UpdateUrl, true);

				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon should be present");
				Assert.IsTrue(_productDetailsPage.IconEditTabMediasCapterra.ExistsInPage(), "Should Capterra tab edit icon should be present");

				//Save
				_productDetailsPage.ButtonSaveMediasCapterra.Click();
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarMediasCapterra);

				//Get updated videos
				var videosUpdated = GetMedias(productId, SiteType.Capterra).Videos;

				//Assert
				Assert.AreEqual(videosUpdated.Single().Url, UpdateUrl);
				Assert.IsTrue(_productDetailsPage.TabMediasCapterra.HasClass(ActiveTabClassName), "Should Capterra tab be active");
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon not be present");
				Assert.IsFalse(_productDetailsPage.IconEditTabMediasCapterra.ExistsInPage(), "Should Capterra tab edit icon should not be present");

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_AttemptToUpdateVideoUrlWithInvalidVideoUrl_Capterra_Fail()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;		

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.Capterra);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var videos = GetMedias(productId, SiteType.Capterra).Videos;

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());

				//Attempt to update the videoUrl with an invalid video url
				var videoId = videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(string.Empty, true);
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys("https://com", true);

				// Assert that the video save button is not enabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveMediasCapterra.IsEnabled());			
				Assert.That(_productDetailsPage.InvalidVideoURL.GetText(), Does.Contain("Please make sure your URL includes http:// or https:// and is free of spaces"));

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_AttemptToUpdateVideoUrlWithInvalidVideoUrl_GetApp_Fail()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.GetApp);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var videos = GetMedias(productId, SiteType.GetApp).Videos;

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.TabMediasGetApp.Click();

				//Attempt to update the videoUrl with an invalid video url
				var videoId = videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(string.Empty, true);
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys("https://com", true);

				// Assert that the video save button is not enabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveMediasGetApp.IsEnabled());
				Assert.That(_productDetailsPage.InvalidVideoURL.GetText(), Does.Contain("Please make sure your URL includes http:// or https:// and is free of spaces"));

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_AttemptToUpdateVideoUrlWithInvalidVideoUrl_SoftwareAdvice_Fail()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.SoftwareAdvice);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var videos = GetMedias(productId, SiteType.SoftwareAdvice).Videos;

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Attempt to update the videoUrl with an invalid video url
				var videoId = videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(string.Empty, true);
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys("https://com", true);

				// Assert that the video save button is not enabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveMediasSoftwareAdvice.IsEnabled());
				Assert.That(_productDetailsPage.InvalidVideoURL.GetText(), Does.Contain("Please make sure your URL includes http:// or https:// and is free of spaces"));

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_UpdateVideoUrl_GetApp_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert video and screenshot
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.GetApp);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var videos = GetMedias(productId, SiteType.GetApp).Videos;

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.TabMediasGetApp.Click();

				//Update
				var videoId = videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(UpdateUrl, true);

				//Validate Edit Icons
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon should be present");
				Assert.IsTrue(_productDetailsPage.IconEditTabMediasGetApp.ExistsInPage(), "Should GetApp tab edit icon should be present");

				//Save
				_productDetailsPage.ButtonSaveMediasGetApp.Click();
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarMediasGetApp);

				//Get updated videos
				var videosUpdated = GetMedias(productId, SiteType.GetApp).Videos;

				//Assert
				Assert.AreEqual(videosUpdated.Single().Url, UpdateUrl);
				Assert.IsTrue(_productDetailsPage.TabMediasGetApp.HasClass(ActiveTabClassName), "Should GetApp Tab be active");
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon should not be present");
				Assert.IsFalse(_productDetailsPage.IconEditTabMediasGetApp.ExistsInPage(), "Should GetApp tab edit icon should not be present");

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_UpdateVideoUrl_SoftwareAdvice_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.SoftwareAdvice);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Get videos
				var medias = GetMedias(productId, SiteType.SoftwareAdvice);

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Update
				var videoId = medias.Videos.Single().UrlId;
				Model.Pages.ProductDetailsPage.GetTextAreaProductVideoUrl(videoId).SendKeys(UpdateUrl, true);

				//Validate
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon should be present");
				Assert.IsTrue(_productDetailsPage.IconEditTabMediasSoftwareAdvice.ExistsInPage(), "Should SoftwareAdvice tab edit icon should be present");

				//Save
				_productDetailsPage.ButtonSaveMediasSoftwareAdvice.Click();
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarMediasSoftwareAdvice);

				//Get updated videos
				var mediasUpdated = GetMedias(productId, SiteType.SoftwareAdvice);

				//Assert
				Assert.IsNotNull(Model.Pages.ProductDetailsPage.GetProductScreenshot(medias.Screenshots.First().FileId));
				Assert.AreEqual(mediasUpdated.Videos.Single().Url, UpdateUrl);
				Assert.IsTrue(_productDetailsPage.TabMediasSoftwareAdvice.HasClass(ActiveTabClassName), "Should Software Advice tab be active");
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductMediasEdit.ExistsInPage(), "Should sidenav edit icon should not be present");
				Assert.IsFalse(_productDetailsPage.IconEditTabMediasSoftwareAdvice.ExistsInPage(), "Should SoftwareAdvice tab edit icon should not be present");

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Ignore("Product screenshot tests do not work in headless mode")]
		public void ProductMedia_AddRequiredScreenshotAndSave_SoftwareAdvice_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Add a screenshot
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);
				_productDetailsPage.ButtonSaveMediasSoftwareAdvice.Click();

				//Assert
				Thread.Sleep(1500);
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductMediasValidationError.IsDisplayed());
				Assert.IsEmpty(_productDetailsPage.ValidationMessageScreenshotsSoftwareAdvice.GetText());

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_WithRequiredScreenshot_SoftwareAdvice_Success()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshot and video
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.SoftwareAdvice);
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Assert
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductMediasValidationError.IsDisplayed());
				Assert.IsEmpty(_productDetailsPage.ValidationMessageScreenshotsSoftwareAdvice.GetText());

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Ignore("Product screenshot tests do not work in headless mode")]
		public void ProductMedia_AddRequiredScreenshotAndCancel_SoftwareAdvice_Fail()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Add a screenshot
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);
				_productDetailsPage.ButtonCancelMediasSoftwareAdvice.Click();

				//Assert
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductMediasValidationError.IsDisplayed());
				Assert.IsTrue(_productDetailsPage.ValidationMessageScreenshotsSoftwareAdvice.IsDisplayed());

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}
		
		[Test]
		public void ProductMedia_WithoutRequiredScreenshot_SoftwareAdvice_Fails()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Assert
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductMediasValidationError.IsDisplayed());
				Assert.IsTrue(_productDetailsPage.ValidationMessageScreenshotsSoftwareAdvice.IsDisplayed());

				//Delete product
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Ignore("Product screenshot tests do not work in headless mode")]
		public void ProductMedia_AddScreenShots_GetApp_VerifyProductMediaUpdatedMessage()
		{
			const string snackBarMessage = "Product medias updated successfully";
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasGetApp.Click();

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotGetApp.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				Thread.Sleep(2500);
				_productDetailsPage.ButtonSaveMediasGetApp.Click();

				//Assert
				Log(_productDetailsPage.SnackBarContainer.GetText(false));
				Assert.AreEqual(snackBarMessage, _productDetailsPage.SnackBarContainer.GetText(false));

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		[Ignore("Product screenshot tests do not work in headless mode")]
		public void ProductMedia_AddScreenShots_SoftwareAdvice_VerifyProductMediaUpdatedMessage()
		{
			const string snackBarMessage = "Product medias updated successfully";
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				Thread.Sleep(2500);
				_productDetailsPage.ButtonSaveMediasSoftwareAdvice.Click();

				//Assert
				Log(_productDetailsPage.SnackBarContainer.GetText(false));
				Assert.AreEqual(snackBarMessage, _productDetailsPage.SnackBarContainer.GetText(false));

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
        [Ignore("Product screenshot tests do not work in headless mode")]
        public void ProductMedia_AddScreenShots_Capterra_VerifyProductMediaUpdatedMessage()
		{
			const string snackBarMessage = "Product medias updated successfully";
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasCapterra.Click();

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				Thread.Sleep(2500);
				_productDetailsPage.ButtonSaveMediasCapterra.Click();

				//Assert
				Log(_productDetailsPage.SnackBarContainer.GetText(false));
				Assert.AreEqual(snackBarMessage, _productDetailsPage.SnackBarContainer.GetText(false));

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
        [Ignore("Product screenshot tests do not work in headless mode")]
        public void ProductMedia_ExceedsAllowedScreenShots_Capterra_VerifyToastMessage()
		{
			const string snackBarMessage = "Only 5 screenshots allowed. Please try again.";
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasCapterra.Click();

				//Add a multiple screenshots
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
                SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);

				//Add multiple screenshots
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
                SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);
				Thread.Sleep(200);

				//Assert
				Log(_productDetailsPage.SnackBarContainer.GetText(false));
				Assert.AreEqual(snackBarMessage, _productDetailsPage.SnackBarContainer.GetText(false));
				Assert.IsFalse(_productDetailsPage.ButtonAddProductScreenshotCapterra.IsDisabled());

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
        [Ignore("Product screenshot tests do not work in headless mode")]
        public void ProductMedia_ExceedsAllowedScreenShots_SoftwareAdvice_VerifyToastMessage()
		{
			const string snackBarMessage = "Only 6 screenshots allowed. Please try again.";
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Add a multiple screenshots
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Add multiple screenshots
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);
				Thread.Sleep(200);

				//Assert
				Log(_productDetailsPage.SnackBarContainer.GetText(false));
				Assert.AreEqual(snackBarMessage, _productDetailsPage.SnackBarContainer.GetText(false));

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
        [Ignore("Product screenshot tests do not work in headless mode")]
        public void ProductMedia_MaximumScreenShotsAllowed_Capterra_AddScreenShotButtonIsDisabled()
		{
		   ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasCapterra.Click();

				//Add a screenshot Multiple Screenshots
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
                SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Add a one screenshot
				_productDetailsPage.ButtonAddProductScreenshotCapterra.Click();
				Thread.Sleep(2500);
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				Thread.Sleep(2500);
				
				//Assert
				Assert.IsTrue(_productDetailsPage.ButtonAddProductScreenshotCapterra.IsDisabled());

				//Delete product - Test may fail due to known defect with ProductAdminApiService
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
        [Ignore("Product screenshot tests do not work in headless mode")]
        public void ProductMedia_MaximumScreenShotsAllowed_SoftwareAdvice_AddScreenShotButtonIsDisabled()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Navigate to Software Advice screenshots
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();

				//Add a screenshot Multiple Screenshots
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);

				//Add a screenshot Multiple Screenshots
				_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.Click();
				Thread.Sleep(2500);
				SelectMultipleFilesForOpenFileDialog(ScreenshotUploadFiles);

				Thread.Sleep(2500);

				//Assert
				Assert.IsTrue(_productDetailsPage.ButtonAddProductScreenshotSoftwareAdvice.IsDisabled());

				//Delete product  - 
				//ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductScreenshot_BySmallScreenshot_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshots
				var (capterraContent, capterraExtension) = GetBase64Asset(CapterraLogoFileName);
				var (softwareAdviceContent, softwareAdviceExtension) = GetBase64Asset(SoftwareAdviceLogo50x50FileName);
				var capterraFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = capterraContent,
					Extension = capterraExtension
				}).Result.FileId;
				var softwareAdviceFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = softwareAdviceContent,
					Extension = softwareAdviceExtension
				}).Result.FileId;
				var getAppFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = capterraContent,
					Extension = capterraExtension
				}).Result.FileId;
				var mediaUpdateRequest = new ProductMediasUpdateRequest
				{
					Medias = new List<ProductSiteMediasUpdateRequest>
					{
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.Capterra,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = capterraFileId,
									DisplayOrder = 1,
									CaptionDescription = "Capterra screenshot"
								}
							}
						},
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.SoftwareAdvice,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = softwareAdviceFileId,
									DisplayOrder = 1,
									CaptionDescription = "Software Advice screenshot"
								}
							}
						},
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.GetApp,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = getAppFileId,
									DisplayOrder = 1,
									CaptionDescription = "GetApp screenshot"
								}
							}
						}
					}
				};
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Assert the product screenshots are 200x200
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				var capterraScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(capterraFileId);
				AssertThumbnailHeightAndWidth(capterraScreenshot);
				_productDetailsPage.TabMediasGetApp.Click();
				var getAppScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(getAppFileId);
				AssertThumbnailHeightAndWidth(getAppScreenshot);
				_productDetailsPage.TabMediasSoftwareAdvice.Click();
				var softwareAdviceScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(softwareAdviceFileId);
				AssertThumbnailHeightAndWidth(softwareAdviceScreenshot);
				
				//Cleanup
				FileManagementApiService.DeleteFileById(capterraFileId);
				FileManagementApiService.DeleteFileById(getAppFileId);
				FileManagementApiService.DeleteFileById(softwareAdviceFileId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductScreenshot_ByLargeScreenshot_CorrectThumbnailSize()
		{
			ExecuteTimedTest(() =>
			{
				//Create product
				var productInsertRequest = GetProductInsertRequest(_productName);
				var productId = ProductAdminApiService.PostProduct(productInsertRequest).Result.ProductId;

				//Insert screenshots
				var (capterraContent, capterraExtension) = GetBase64Asset(CapterraLogoFileName);
				var (softwareAdviceContent, softwareAdviceExtension) = GetBase64Asset(SoftwareAdviceLogoFileName);
				var capterraFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = capterraContent,
					Extension = capterraExtension
				}).Result.FileId;
				var softwareAdviceFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = softwareAdviceContent,
					Extension = softwareAdviceExtension
				}).Result.FileId;
				var getAppFileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
				{
					Content = capterraContent,
					Extension = capterraExtension
				}).Result.FileId;
				var mediaUpdateRequest = new ProductMediasUpdateRequest
				{
					Medias = new List<ProductSiteMediasUpdateRequest>
					{
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.Capterra,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = capterraFileId,
									DisplayOrder = 1,
									CaptionDescription = "Capterra screenshot"
								}
							}
						},
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.SoftwareAdvice,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = softwareAdviceFileId,
									DisplayOrder = 1,
									CaptionDescription = "Software Advice screenshot"
								}
							}
						},
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)SiteType.GetApp,
							Screenshots = new List<ProductScreenshotMediaUpdateRequest>
							{
								new ProductScreenshotMediaUpdateRequest
								{
									FileId = getAppFileId,
									DisplayOrder = 1,
									CaptionDescription = "GetApp screenshot"
								}
							}
						}
					}
				};
				ProductAdminApiService.PutProductMedias(productId.ToString(), mediaUpdateRequest);

				//Assert the product screenshots are 200x200
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId.ToString());
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				var capterraScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(capterraFileId);
				AssertThumbnailHeightAndWidth(capterraScreenshot);
				_productDetailsPage.TabMediasGetApp.Click();
				var getAppScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(getAppFileId);
				AssertThumbnailHeightAndWidth(getAppScreenshot);
				_productDetailsPage.TabMediasSoftwareAdvice.Click();
				var softwareAdviceScreenshot = Model.Pages.ProductDetailsPage.GetProductScreenshot(softwareAdviceFileId);
				AssertThumbnailHeightAndWidth(softwareAdviceScreenshot);

				//Cleanup
				FileManagementApiService.DeleteFileById(capterraFileId);
				FileManagementApiService.DeleteFileById(getAppFileId);
				FileManagementApiService.DeleteFileById(softwareAdviceFileId);
				ProductAdminApiService.DeleteProduct(productId.ToString());
			});
		}

		[Test]
		public void ProductMedia_AddYoutubeShortsVideo_Capterra_Success()
		{
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//create a product associated with the vendor created above
				var product = PostProduct(_productName, _productWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));

				//Add Youtube shorts video in Capterra
				string shortVideoUrl = "https://youtu.be/CpU2NhUaDpY";
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.Capterra, shortVideoUrl);
				ProductAdminApiService.PutProductMedias(product.ProductId.ToString(), mediaUpdateRequest);

				//Navigate to Product detail page and save video
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId);
				_productDetailsPage.ButtonSaveMediasCapterra.Click();

				//Get video added in Capterra
				var video = GetMedias(Convert.ToInt32(product.ProductId), SiteType.Capterra).Videos;
	
				//Assert that the YouTube video was successfully added
				Assert.AreEqual(video.Single().Url, GetVideoAttributes(shortVideoUrl).Url);
				Assert.AreEqual(1, GetVideoAttributes(shortVideoUrl).VideoTypeId);

				//Delete product
				ProductAdminApiService.DeleteProduct(product.ProductId);
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ProductMedia_AddYoutubeShortsVideo_GetApp_Success()
		{
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//create a product associated with the vendor created above
				var product = PostProduct(_productName, _productWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));

				//Add Youtube shorts video in GetApp
				string shortVideoUrl = "https://youtu.be/CpU2NhUaDpY";
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.GetApp, shortVideoUrl);
				ProductAdminApiService.PutProductMedias(product.ProductId.ToString(), mediaUpdateRequest);

				//Navigate to Product detail page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId);
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasGetApp.Click();

				//Save
				_productDetailsPage.ButtonSaveMediasGetApp.Click();				

				//Get video added in GetApp
				var video = GetMedias(Convert.ToInt32(product.ProductId), SiteType.GetApp).Videos;

				//Assert that the YouTube video was successfully added
				Assert.AreEqual(video.Single().Url, GetVideoAttributes(shortVideoUrl).Url);
				Assert.AreEqual(1, GetVideoAttributes(shortVideoUrl).VideoTypeId);

				//Delete product
				ProductAdminApiService.DeleteProduct(product.ProductId);
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void ProductMedia_AddYoutubeShortsVideo_SoftwareAdvice_Success()
		{
			ExecuteTimedTest(() =>
			{
				var vendor = PostVendor(_vendorName, _vendorWebsiteUrl);

				//create a product associated with the vendor created above
				var product = PostProduct(_productName, _productWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));

				//Add Youtube shorts video in GetApp
				string shortVideoUrl = "https://youtu.be/CpU2NhUaDpY";
				var mediaUpdateRequest = GetProductMediasUpdateRequest(SiteType.GetApp, shortVideoUrl);
				ProductAdminApiService.PutProductMedias(product.ProductId.ToString(), mediaUpdateRequest);

				//Navigate to Product detail page and save video
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId);
				_productDetailsPage.LinkRightSidnavProductMedias.Click();
				_productDetailsPage.TabMediasSoftwareAdvice.Click();
				_productDetailsPage.ButtonSaveMediasSoftwareAdvice.Click();

				//Get video added in Software Advice
				var video = GetMedias(Convert.ToInt32(product.ProductId), SiteType.GetApp).Videos;

				//Assert that the YouTube video was successfully added
				Assert.AreEqual(video.Single().Url, GetVideoAttributes(shortVideoUrl).Url);
				Assert.AreEqual(1, GetVideoAttributes(shortVideoUrl).VideoTypeId);

				//Delete product
				ProductAdminApiService.DeleteProduct(product.ProductId);
				DeleteVendor(vendor.VendorId);
			});
		}

		private ProductMediasUpdateRequest GetProductMediasUpdateRequest(SiteType siteType, string videoUrl = "http://www.youtube.com/test")
		{
			//Software Advice is the only site that requires a screenshot
			//Insert File
			var fileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
			{
				Content = "test",
				Extension = "jpg"
			}).Result.FileId;

			var productScreenshotMediaUpdateRequest = new List<ProductScreenshotMediaUpdateRequest>
				{
					new ProductScreenshotMediaUpdateRequest
					{
						FileId = fileId,
						DisplayOrder = 1,
						CaptionDescription = "Test"
					}
				};

			var request = new ProductMediasUpdateRequest
			{
				Medias = new List<ProductSiteMediasUpdateRequest>
					{
						new ProductSiteMediasUpdateRequest
						{
							SiteId = (int)siteType,
							Videos = new List<ProductVideoMediaUpdateRequest>
							{
								new ProductVideoMediaUpdateRequest
								{
									Url = videoUrl
								}
							}
						}
					}
			};

			//Add file to request to make sure no validation error is displayed on right sidenav
			if (siteType == SiteType.SoftwareAdvice)
				request.Medias.First().Screenshots = productScreenshotMediaUpdateRequest;
			else
				request.Medias.Add(new ProductSiteMediasUpdateRequest
				{
					SiteId = (int)SiteType.SoftwareAdvice,
					Screenshots = productScreenshotMediaUpdateRequest
				});

			return request;
		}

		private ProductMediaDto GetMedias(int productId, SiteType siteType)
		{
			return ProductAdminApiService.GetProductMedias(productId.ToString())
				   .Result
				   .Single(pm => pm.SiteId == (int)siteType);
		}

		private ProductVideoAttributesDto GetVideoAttributes(string videoUrl)
		{
			return ProductAdminApiService.GetProductVideoAttributes(videoUrl)
				   .Result;
				   					
		}
	}
}