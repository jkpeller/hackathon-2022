using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	[Category("Product")]
	public class SitePublishStatusTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private string _shortDescription;
		private string _longDescription;
		private string _tagline;
		private const string ExpectedUnsavedChangesDialogText = "Unable to Publish with Unsaved Changes";
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";
		private const string CapterraSiteName = "capterra";
		private const string GetAppSiteName = "getapp";
		private const string SoftwareAdviceSiteName = "software-advice";
		private const string PublishStatusName = nameof(PublishStatusType.Published);

		public SitePublishStatusTests() : base(nameof(SitePublishStatusTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
			_shortDescription = RequestUtility.GetRandomString(100);
			_longDescription = RequestUtility.GetRandomString(300);
			_tagline = RequestUtility.GetRandomString(20);
		}
		
		[Test]
		public void UpdateProductSitePublishStatus_ByArchivedProductStatus_Fails()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Save the product so it has required fields
				_productDetailsPage.ButtonSaveProductDetailsChanges.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-product-info");
				Log("Clicked the save global product button.");

				//Update the Status of the product to be archived
				ProductAdminApiService.PutProductStatus(productId, new ProductStatusUpdateRequest { ProductStatusId = (int)ProductStatusType.Archived });
				Log("Product status updated.");

				//Refresh the page
				BrowserUtility.Refresh();

				//Assert that none of the site publish status publish buttons can be clicked
				Assert.IsFalse(BasePage.GetSitePublishButtonBySite(CapterraSiteName).ExistsInPage());
				Assert.IsFalse(BasePage.GetSitePublishButtonBySite(GetAppSiteName).ExistsInPage());
				Assert.IsFalse(BasePage.GetSitePublishButtonBySite(SoftwareAdviceSiteName).ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void CapterraPublish_MissingRequiredFields_Fails()
		{
			const string expectedConfirmationDialogTitleText = "Product could not be published on Capterra";
			const string expectedConfirmationDialogSupportingText = "Please address the following issues before trying to publish again:\r\nLogo is required\r\nAt least 1 Deployment Option must be selected\r\nAt least 1 Target Company Size must be selected\r\nCapterra Short Description is required\r\nCapterra Long Description is required\r\nMust be in at least one active published category";

			ExecuteTimedTest(() =>
			{
				//Setup product without logo
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click the publish button for Capterra
				BasePage.GetSitePublishButtonBySite(CapterraSiteName).Click();
				Log("Clicked the publish button for Capterra");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(expectedConfirmationDialogTitleText, dialogTitle);
				Assert.AreEqual(expectedConfirmationDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void GetAppPublish_MissingRequiredFields_Fails()
		{
			const string expectedConfirmationDialogTitleText = "Product could not be published on GetApp";
			const string expectedConfirmationDialogSupportingText = "Please address the following issues before trying to publish again:\r\nLogo is required\r\nAt least 1 Deployment Option must be selected\r\nAt least 1 Target Company Size must be selected\r\nGetApp must have a Tagline\r\nGetApp must have a Short Description\r\nMust be in at least one active published category";

			ExecuteTimedTest(() =>
			{
				//Setup product without logo
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click the publish button for GetApp
				BasePage.GetSitePublishButtonBySite(GetAppSiteName).Click();
				Log("Clicked the publish button for GetApp");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(expectedConfirmationDialogTitleText, dialogTitle);
				Assert.AreEqual(expectedConfirmationDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void SoftwareAdvicePublish_MissingRequiredFields_Fails()
		{
			const string expectedConfirmationDialogTitleText = "Product could not be published on Software Advice";
			const string expectedConfirmationDialogSupportingText = "Please address the following issues before trying to publish again:\r\nLogo is required\r\nAt least 1 Deployment Option must be selected\r\nAt least 1 Target Company Size must be selected\r\nSoftware Advice must have at least 1 screenshot\r\nSoftware Advice must have a Long Description\r\nMust be in at least one active published category";

			ExecuteTimedTest(() =>
			{
				//Setup product without logo
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click the publish button for SoftwareAdvice
				BasePage.GetSitePublishButtonBySite(SoftwareAdviceSiteName).Click();
				Log("Clicked the publish button for Software Advice");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(expectedConfirmationDialogTitleText, dialogTitle);
				Assert.AreEqual(expectedConfirmationDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void CapterraPublish_WithUnsavedChangesRequiredFields_Fails()
		{
			const string expectedUnsavedChangesDialogSupportingText = "Please save or cancel your changes on the following sections, then try publishing again:\r\nDetails\r\nCapterra Descriptions";
			ExecuteTimedTest(() =>
			{
				//Post a product with logo
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Fill Target Company Size and Deployment Option without save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Fill short and long description without save
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(_shortDescription);
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(_longDescription);

				//Click the publish button for Capterra
				BasePage.GetSitePublishButtonBySite(CapterraSiteName).Click();
				Log("Clicked the publish button for Capterra");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(ExpectedUnsavedChangesDialogText, dialogTitle);
				Assert.AreEqual(expectedUnsavedChangesDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void GetAppPublish_WithUnsavedChangesRequiredFields_Fails()
		{
			const string expectedUnsavedChangesDialogSupportingText = "Please save or cancel your changes on the following sections, then try publishing again:\r\nDetails\r\nGetApp Descriptions";
			ExecuteTimedTest(() =>
			{
				//Post a product with logo
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Fill Target Company Size and Deployment Option without save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Fill tagline and short description without save
				_productDetailsPage.TextareaGetAppTagline.SendKeys(_tagline);
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(_shortDescription);

				//Click the publish button for GetApp
				BasePage.GetSitePublishButtonBySite(GetAppSiteName).Click();
				Log("Clicked the publish button for GetApp");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(ExpectedUnsavedChangesDialogText, dialogTitle);
				Assert.AreEqual(expectedUnsavedChangesDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void SoftwareAdvicePublish_WithUnsavedChangesRequiredFields_Fails()
		{
			const string expectedUnsavedChangesDialogSupportingText = "Please save or cancel your changes on the following sections, then try publishing again:\r\nDetails\r\nSoftware Advice Descriptions";
			ExecuteTimedTest(() =>
			{
				//Post a product with logo, does not navigate to product detail page
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true, false);

				//Add screenshot to product
				AddSoftwareAdviceScreenshotToProduct(productId);
				Log("Screenshot added to product.");

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Fill Target Company Size and Deployment Option without save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click SoftwareAdvice's description tab
				_productDetailsPage.TabDescriptionsSoftwareAdvice.Click();
				Log("Clicked SoftwareAdvice tab for descriptions.");

				//Fill long description without save
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(_longDescription);

				//Click the publish button for Software Advice
				BasePage.GetSitePublishButtonBySite(SoftwareAdviceSiteName).Click();
				Log("Clicked the publish button for Software Advice");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(ExpectedUnsavedChangesDialogText, dialogTitle);
				Assert.AreEqual(expectedUnsavedChangesDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void CapterraPublish_MissingActivePublishedCategories_Fails()
		{
			const string expectedConfirmationDialogTitleText = "Product could not be published on Capterra";
			const string expectedConfirmationDialogSupportingText = "Please address the following issues before trying to publish again:\r\nLogo is required\r\nAt least 1 Deployment Option must be selected\r\nAt least 1 Target Company Size must be selected\r\nCapterra Short Description is required\r\nCapterra Long Description is required\r\nMust be in at least one active published category";

			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//This method will setup Product Categories without meeting the minimum required of 1 active and published category for this site
				var (categoryId1, categoryId2) = SetUpProductCategories(productId, SiteType.Capterra, false);

				//Click the publish button for Capterra
				BasePage.GetSitePublishButtonBySite(CapterraSiteName.ToLower()).Click();
				Log("Clicked the publish button for Capterra");

				var dialogTitle = _productDetailsPage.ConfirmationDialogTitle.GetText();
				var dialogSupportingText = _productDetailsPage.ConfirmationDialogMessage.GetText();

				//Validate that the warning message pops up
				Assert.AreEqual(expectedConfirmationDialogTitleText, dialogTitle);
				Assert.AreEqual(expectedConfirmationDialogSupportingText, dialogSupportingText);
				Assert.IsTrue(_productDetailsPage.ButtonConfirmationDialogCancel.IsDisplayed());

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId1.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void CapterraPublish_MeetsRequirements_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Post a product with logo
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true, false);

				//This method will setup Product Categories meeting the minimum required of 1 active and published category for this site
				var (categoryId1, categoryId2) = SetUpProductCategories(productId, SiteType.Capterra, true);

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Fill Target Company Size and Deployment Option and save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();
				_productDetailsPage.ButtonSaveProductDetailsChanges.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-product-info");

				//Fill short and long description and save
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(_shortDescription);
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(_longDescription);
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-descriptions-capterra");
				
				//Click the publish button for Capterra
				BasePage.GetSitePublishButtonBySite(CapterraSiteName).Click();
				Log("Clicked the publish button for Capterra");

				//Click to publish the product
				_productDetailsPage.ButtonConfirmationDialogAction.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-publish-status-capterra");
				Thread.Sleep(3000);

				//Validate status update in Publish status card
				Assert.AreEqual(PublishStatusName, BasePage.GetSitePublishStatusBySite(CapterraSiteName).GetText());

				//Get the product details and validate that the publish status is correct
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved. ProductId: {productId}, Response: {getResponse.ToJson()}");
				var capterraPublishStatusId = getResponse.Result.PublishStatuses.Single(p => p.SiteId == (int)SiteType.Capterra).PublishStatusTypeId;
				Assert.AreEqual(PublishStatusType.Published, capterraPublishStatusId);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId1.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void GetAppPublish_MeetsRequirements_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Post a product with logo
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true, false);

				//This method will setup Product Categories meeting the minimum required of 1 active and published category for this site
				var (categoryId1, categoryId2) = SetUpProductCategories(productId, SiteType.GetApp, true);

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Fill Target Company Size and Deployment Option and save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();
				_productDetailsPage.ButtonSaveProductDetailsChanges.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-product-info");
				Thread.Sleep(3000);

				//Click GetApp description tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Fill tagline and short description and save
				_productDetailsPage.TextareaGetAppTagline.SendKeys(_tagline);
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(_shortDescription);
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-descriptions-getapp");
				Thread.Sleep(3000);
				
				//Click the publish button for GetApp
				BasePage.GetSitePublishButtonBySite(GetAppSiteName).Click();
				Log("Clicked the publish button for GetApp");
				
				//Click to publish the product on GetApp
				_productDetailsPage.ButtonConfirmationDialogAction.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-publish-status-getapp");
				Thread.Sleep(3000);

				//Validate status update in Publish status card
				Assert.AreEqual(PublishStatusName, BasePage.GetSitePublishStatusBySite(GetAppSiteName).GetText());

				//Get the product details and validate that the publish status is correct
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved. ProductId: {productId}, Response: {getResponse.ToJson()}");
				var getAppPublishStatusId = getResponse.Result.PublishStatuses.Single(p => p.SiteId == (int)SiteType.GetApp).PublishStatusTypeId;
				Assert.AreEqual(PublishStatusType.Published, getAppPublishStatusId);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId1.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void SoftwareAdvicePublish_MeetsRequirements_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				//Post a product with logo
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true, false);

				//This method will setup Product Categories meeting the minimum required of 1 active and published category for this site
				var (categoryId1, categoryId2) = SetUpProductCategories(productId, SiteType.SoftwareAdvice, true);

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Add screenshot to product
				AddSoftwareAdviceScreenshotToProduct(productId);
				Log("Screenshot added to product.");

				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

				//Fill Target Company Size and Deployment Option and save
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();
				_productDetailsPage.ButtonSaveProductDetailsChanges.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-product-info");
				Thread.Sleep(3000);

				//Click SoftwareAdvice description tab
				_productDetailsPage.TabDescriptionsSoftwareAdvice.Click();
				Log("Clicked SoftwareAdvice tab for descriptions.");

				//Fill long description and save
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(_longDescription);
				_productDetailsPage.ButtonSaveSoftwareAdviceProductDescriptions.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-descriptions-software-advice");
				Thread.Sleep(3000);

				//Click the publish button for SoftwareAdvice
				BasePage.GetSitePublishButtonBySite(SoftwareAdviceSiteName).Click();
				Log("Clicked the publish button for SoftwareAdvice");

				//Click to publish the product for Software Advice
				_productDetailsPage.ButtonConfirmationDialogAction.Click();
				BrowserUtility.WaitForElementToDisappear("mat-progress-bar-publish-status-software-advice");
				Thread.Sleep(3000);

				//Validate status update in Publish status card
				Assert.AreEqual(PublishStatusName, BasePage.GetSitePublishStatusBySite(SoftwareAdviceSiteName).GetText());

				//Get the product details and validate that the publish status is correct
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved. ProductId: {productId}, Response: {getResponse.ToJson()}");
				var softwareAdvicePublishStatusId = getResponse.Result.PublishStatuses.Single(p => p.SiteId == (int)SiteType.SoftwareAdvice).PublishStatusTypeId;
				Assert.AreEqual(PublishStatusType.Published, softwareAdvicePublishStatusId);

				//Cleanup
				CategoryAdminApiService.DeleteCategory(categoryId1.ToString());
				CategoryAdminApiService.DeleteCategory(categoryId2.ToString());
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		private void AddSoftwareAdviceScreenshotToProduct(string productId)
		{
			var fileId = ProductAdminApiService.PostProductScreenshots(new FileRequest
			{
				Content = "VGVzdCBsb2dv",
				Extension = "png"
			}).Result.FileId;

			var mediasRequest = new ProductMediasUpdateRequest
			{
				Medias = new List<ProductSiteMediasUpdateRequest>
				{
					new ProductSiteMediasUpdateRequest
					{
						SiteId = (int)SiteType.SoftwareAdvice,
						Screenshots = new List<ProductScreenshotMediaUpdateRequest>
						{
							new ProductScreenshotMediaUpdateRequest
							{
								FileId = fileId, 
								DisplayOrder = 1,
								CaptionDescription = "Caption"
							}
						}
					}
				}
			};

			//Associate screenshot to product
			ProductAdminApiService.PutProductMedias(productId, mediasRequest);
		}

		private (int CategoryId1, int CategoryId2) SetUpProductCategories(string productId, SiteType site, bool isMeetRequirements)
		{
			//Create category1
			var category1 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = $"Accounting{RequestUtility.GetRandomString(9)}" });
			var categoryId1 = category1.Result.CategoryId;

			//Create category2
			var category2 = CategoryAdminApiService.PostCategory(new CategoryInsertRequest { Name = $"Sales{RequestUtility.GetRandomString(9)}" });
			var categoryId2 = category2.Result.CategoryId;

			//Publish category2 to Site
			CategoryAdminApiService.PutCategoryPublishStatusById(categoryId2.ToString(), new CategoryPublishStatusUpdateRequest
			{
				PublishStatusTypeId = PublishStatusType.Published,
				SiteId = site
			});

			//Assign categories to Product
			var productCategories = ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
			{
				ProductId = Convert.ToInt32(productId),
				CategoryIds = new List<int> { categoryId1, categoryId2 }
			});

			//Get product category to be archived
			var productCategoryId = productCategories.Result.Single(pc => pc.CategoryId == categoryId2).ProductCategoryId;

			if (!isMeetRequirements)
			{
				//set product category status to archived
				ProductAdminApiService.PutProductCategory(productCategoryId.ToString(), new ProductCategoryUpdateRequest
				{
					StatusTypeId = ProductCategoryStatusType.Archived
				});
			}
			
			return (categoryId1, categoryId2);
		}
	}
}