using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Request.V2;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Control = ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore.Control;
using Keys = OpenQA.Selenium.Keys;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	[Category("Product")]
	public class ProductDetailTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private const string ProductWebsiteUrl ="http://www.testproductwebsiteurl.com/";

		public ProductDetailTests() : base(nameof(ProductDetailTests))
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
		public void EditProductDetails_ValidValues_DoesNotDisplayValidationErrorMessages()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);
				
				//Product Name
				_productDetailsPage.InputProductName.SendKeys(RequestUtility.GetRandomString(9), true);
				_productDetailsPage.InputProductName.SendKeys(Keys.Tab);
				Assert.IsFalse(_productDetailsPage.InvalidProductNameMinLength.IsDisplayed());
				Assert.IsFalse(_productDetailsPage.InvalidProductNameRequired.IsDisplayed());

				_productDetailsPage.InputProductName.SendKeys(RequestUtility.GetRandomString(101), true);
				_productDetailsPage.InputProductName.SendKeys(Keys.Tab);
				Assert.IsFalse(_productDetailsPage.InvalidProductNameMinLength.IsDisplayed());
				Assert.IsFalse(_productDetailsPage.InvalidProductNameRequired.IsDisplayed());

				//Product URL
				var validUrl = ValidUrls.First();
				_productDetailsPage.InputProductWebsiteUrl.SendKeys(validUrl, true);
				_productDetailsPage.InputProductWebsiteUrl.SendKeys(Keys.Tab);				
				Assert.IsFalse(_productDetailsPage.InvalidProductWebsiteUrl.IsDisplayed());

				//Target Company Size
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				Assert.IsFalse(_productDetailsPage.InvalidTargetCompanySizeRequired.IsDisplayed());

				//Deployment Options
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();		
				Assert.IsFalse(_productDetailsPage.InvalidDeploymentOptionRequired.IsDisplayed());

				//Social Media Profiles & Mobile App URLs
				var inputNames = SocialMedias.Union(MobileApps);

				foreach (var inputName in inputNames)
				{
					var input = new Control(ControlUtility.GetElementSelector($"input-product-{inputName}"));
					var invalidMessage = new Control(ControlUtility.GetElementSelector($"mat-error-invalid-product-{inputName}-url"));

					input.SendKeys(validUrl, true);
					input.SendKeys(Keys.Tab);

					Assert.IsFalse(invalidMessage.IsDisplayed());
				}

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_InvalidValues_DisplayValidationErrorMessages()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Product Logo
				//When creating a new product, this contains no logo by default
				Assert.IsTrue(_productDetailsPage.InvalidProductLogoRequired.IsDisplayed());

				//Product Name
				_productDetailsPage.InputProductName.SendKeys("ab", true);
				_productDetailsPage.InputProductName.SendKeys(Keys.Tab);

				Assert.IsTrue(_productDetailsPage.InvalidProductNameMinLength.IsDisplayed());

				_productDetailsPage.InputProductName.SendKeys(string.Empty, true);
				_productDetailsPage.InputProductName.SendKeys(Keys.Tab);

				Assert.IsTrue(_productDetailsPage.InvalidProductNameRequired.IsDisplayed());

				//Product URL
				var invalidUrl = InvalidUrls.First();
				_productDetailsPage.InputProductWebsiteUrl.SendKeys(invalidUrl, true);
				_productDetailsPage.InputProductWebsiteUrl.SendKeys(Keys.Tab);
				Thread.Sleep(100);

				//Assert that input is invalid by showing invalid message
				Assert.IsTrue(_productDetailsPage.InvalidProductWebsiteUrl.IsDisplayed());

				//Target Company Size
				//When creating a new product no target company size is selected by default
				Assert.IsTrue(_productDetailsPage.InvalidTargetCompanySizeRequired.IsDisplayed());

				//Deployment Options
				//When creating a new product no deployment option is selected by default
				Assert.IsTrue(_productDetailsPage.InvalidDeploymentOptionRequired.IsDisplayed());

				//Social Media Profiles & Mobile App URLs
				var inputNames = SocialMedias.Union(MobileApps);
				foreach (var inputName in inputNames)
				{
					var input = new Control(ControlUtility.GetElementSelector($"input-product-{inputName}"));
					var invalidMessage = new Control(ControlUtility.GetElementSelector($"mat-error-invalid-product-{inputName}-url"));

					input.SendKeys(invalidUrl, true);
					input.SendKeys(Keys.Tab);

					Assert.IsTrue(invalidMessage.IsDisplayed());
				}

				//Save button should be disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveProductDetailsChanges.IsEnabled());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByValidTargetIndustries_Succeeds()
		{
			const string accountingTargetIndustry = "Accounting";
			const string animationTargetIndustry = "Animation";
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a couple of target industries from the multi-select
				_productDetailsPage.InputTargetIndustries.Click();
				Log("Clicked the target industries input box.");
				Model.Pages.ProductDetailsPage.GetTargetIndustryOptionByName(accountingTargetIndustry).Click();
				Log($"{accountingTargetIndustry} target industry option selected.");
				//Click the other other without attempted to click into the select box again to make sure it's a multi-select
				Model.Pages.ProductDetailsPage.GetTargetIndustryOptionByName(animationTargetIndustry).Click();
				Log($"{animationTargetIndustry} target industry option selected.");

				// Check if chips are visible
				var accountingChip = Model.Pages.ProductDetailsPage.GetTargetIndustryActiveChipByName(accountingTargetIndustry);
				var animationChip = Model.Pages.ProductDetailsPage.GetTargetIndustryActiveChipByName(animationTargetIndustry);
				Assert.IsTrue(accountingChip.IsDisplayed());
				Assert.IsTrue(animationChip.IsDisplayed());

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click the Save button
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global product button.");

				//Get the product details from the API and make sure the results were saved
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved successfully. ProductId: {productId}");
				Assert.IsTrue(getResponse.Result.TargetIndustries.Any(ti => ti.Name == accountingTargetIndustry && ti.IsSelected));
				Assert.IsTrue(getResponse.Result.TargetIndustries.Any(ti => ti.Name == animationTargetIndustry && ti.IsSelected));

				// Remove previously selected Target Industry by clicking X on a chip
				Model.Pages.ProductDetailsPage.GetTargetIndustryActiveChipRemoveButtonByName(accountingTargetIndustry).Click();
				Log($"Removed target industry chip. Chip text: {accountingTargetIndustry}");
				Thread.Sleep(1000);
				accountingChip = Model.Pages.ProductDetailsPage.GetTargetIndustryActiveChipByName(accountingTargetIndustry);
				Assert.IsFalse(accountingChip.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByValidTrainingOptions_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a valid option from the licensing model select box
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.InPerson).Click();
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.LiveOnline).Click();
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Webinars).Click();
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Documentation).Click();
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Videos).Click();

				//Click save to validate that there are no issues with the selection
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//TODO: when we have the readonly view, add an assert for this field display

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByNotOfferedTrainingOption_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a valid option from the licensing model select box
				Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.NotOffered).Click();

				//Assert that the other options are not selectable
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.InPerson).IsDisabled());
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.LiveOnline).IsDisabled());
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Webinars).IsDisabled());
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Documentation).IsDisabled());
				Assert.IsTrue(Model.Pages.ProductDetailsPage.GetDetailsCheckboxByOptionName(ProductDetailsUtility.TrainingOptions.Videos).IsDisabled());

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click save to validate that there are no issues with the selection
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//TODO: when we have the readonly view, add an assert for this field display

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_AddAllDeploymentOptions_Succeeds()
		{
			var deploymentOptionTypeIds = Enum.GetValues(typeof(DeploymentOptionType)).Cast<DeploymentOptionType>().ToList();
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Check the boxes for all existing deployment options
				ClickAllDeploymentOptionsAndSave();

				//Target Company Size is required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();

				//Click the Save button
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global product button.");
				Thread.Sleep(2000);

				//Assert the deployment options are selected by retrieving the product details from the api
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved. ProductId: {productId}");
				foreach (var deploymentOptionTypeId in deploymentOptionTypeIds)
				{
					Assert.IsTrue(getResponse.Result.DeploymentOptions.Any(d => d.DeploymentOptionTypeId == deploymentOptionTypeId && d.IsSelected));
				}

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_RemoveAllDeploymentOptions_Succeeds()
		{
			var deploymentOptionTypeIds = Enum.GetValues(typeof(DeploymentOptionType)).Cast<DeploymentOptionType>().ToList();
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Check the boxes for all existing deployment options
				ClickAllDeploymentOptionsAndSave();

				//Uncheck all deployment options
				ClickAllDeploymentOptionsAndSave();

				//Assert the deployment options are selected by retrieving the product details from the api
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved. ProductId: {productId}");
				foreach (var deploymentOptionTypeId in deploymentOptionTypeIds)
				{
					Assert.IsTrue(getResponse.Result.DeploymentOptions.Any(d => d.DeploymentOptionTypeId == deploymentOptionTypeId && !d.IsSelected));
				}

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductDetails_ByValidSupportedLanguages_Succeeds()
		{
			const string englishSupportedLanguage = "English";
			const string arabicSupportedLanguage = "Arabic";
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a couple of target industries from the multi-select
				_productDetailsPage.InputSupportedLanguages.Click();
				Log("Clicked the supported languages input box.");
				Model.Pages.ProductDetailsPage.GetSupportedLanguageOptionByName(englishSupportedLanguage).Click();
				Log($"{englishSupportedLanguage} supported language option selected.");
				//Click the other other without attempted to click into the select box again to make sure it's a multi-select
				Model.Pages.ProductDetailsPage.GetSupportedLanguageOptionByName(arabicSupportedLanguage).Click();
				Log($"{arabicSupportedLanguage} supported language option selected.");

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click the Save button
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global product button.");

				//Get the product details from the API and make sure the results were saved
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved successfully. ProductId: {productId}, Response: {getResponse.ToJson()}");
				Assert.IsTrue(getResponse.Result.SupportedLanguages.Any(ti => ti.Name == englishSupportedLanguage && ti.IsSelected));
				Assert.IsTrue(getResponse.Result.SupportedLanguages.Any(ti => ti.Name == arabicSupportedLanguage && ti.IsSelected));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByValidSupportedCountries_Succeeds()
		{
			const string afghanistanSupportedCountry = "Afghanistan";
			const string usSupportedCountry = "United States of America";
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a couple of target industries from the multi-select
				_productDetailsPage.InputSupportedCountries.Click();
				Log("Clicked the supported countries input box.");
				Model.Pages.ProductDetailsPage.GetSupportedCountryOptionByName(afghanistanSupportedCountry).Click();
				Log($"{afghanistanSupportedCountry} supported country option selected.");
				//Click the other other without attempted to click into the select box again to make sure it's a multi-select
				Model.Pages.ProductDetailsPage.GetSupportedCountryOptionByName(usSupportedCountry).Click();
				Log($"{usSupportedCountry} supported country option selected.");

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Click the Save button
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save global product button.");
				Thread.Sleep(5000);

				//Get the product details from the API and make sure the results were saved
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Log($"Product details retrieved successfully. ProductId: {productId}, Response: {getResponse.ToJson()}");
				Assert.IsTrue(getResponse.Result.SupportedCountries.Any(ti => ti.Name == afghanistanSupportedCountry && ti.IsSelected));
				Assert.IsTrue(getResponse.Result.SupportedCountries.Any(ti => ti.Name == usSupportedCountry && ti.IsSelected));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByUpdatedLicensingModel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a valid option from the licensing model select box
				_productDetailsPage.SelectLicensingModel.Click();
				_productDetailsPage.OptionLicensingModelProprietary.Click();
				Log("Selected the Proprietary option from the select box.");

				//Click save to validate that there are no issues with the selection
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//TODO: when we have the readonly view, add an assert for this field display

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductDetails_ByNoneLicensingModel_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Select a valid option from the licensing model select box
				_productDetailsPage.SelectLicensingModel.Click();
				_productDetailsPage.OptionLicensingModelProprietary.Click();
				Log("Selected the Proprietary option from the select box.");

				//Click save to validate that there are no issues with the selection
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save product details button.");
				Thread.Sleep(2000);

				//Update the licensing model to None to clear out the selection
				_productDetailsPage.SelectLicensingModel.Click();
				_productDetailsPage.OptionLicensingModelNone.Click();
				Log("Selected the None option from the select box.");

				//Click save to validate that there are no issues with the selection
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//Get product details and validate that no options were saved as selected
				var getResponse = ProductAdminApiService.GetProductDetailsById(productId);
				Assert.IsFalse(getResponse.Result.LicensingModels.TrueForAll(lm => lm.IsSelected));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Ignore("Product logo tests do not work in headless mode")]
		public void EditProductDetails_AddLogo_Succeeds()
		{
			var currentDir = AppDomain.CurrentDomain.BaseDirectory;
			ExecuteTimedTest(() =>
			{
				//Setup
				var (productId, productFileId) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);

				//Target Company Size and Deployment Option are required
				_productDetailsPage.CheckboxTargetCompanySizeSelfEmployed.Click();
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased.Click();

				//Remove existing logo
				_productDetailsPage.ContainerProductLogo.HoverOver();
				_productDetailsPage.ButtonRemoveLogo.Click();
				Log("Clicked the remove logo button.");

				//Validate validation error icon for product details sidenav section is visible
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductDetailsValidationError.ExistsInPage(), "Product Details Validation Error Icon should be present");

				//Add new logo
				_productDetailsPage.ContainerProductLogo.HoverOver();
				_productDetailsPage.ButtonAddChangeLogo.Click();
				Log("Clicked the add logo button");
				Thread.Sleep(1000);
				//Select a logo
				SelectAssetForOpenFileDialog(SoftwareAdviceLogoFileName);

				//Validate validation error icon for product details sidenav section is NOT visible
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductDetailsValidationError.ExistsInPage(), "Validation Error Icon should not be present");

				//Validate edit icon for product details sidenav section is visible
				Assert.IsTrue(_productDetailsPage.IconRightSidenavProductDetailsEdit.ExistsInPage(), "Edit Icon should be present");

				//Save the product
				_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
				Log("Clicked the save product button");

				//Validate edit icon for product details sidenav section is NOT visible
				Assert.IsFalse(_productDetailsPage.IconRightSidenavProductDetailsEdit.ExistsInPage(), "Edit Icon should not be present");

				//Cleanup
				//TODO: get added logo's productFileId use it for clean up here
				//ProductAdminApiService.DeleteProductFile(productFileId);
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		public void EditProductStatus_Succeeds()
		{
			var categoryName = RequestUtility.GetRandomString(8);
			var productName2 = RequestUtility.GetRandomString(8);
			var productWebsiteUrl2 = $"https://{productName2}.com";
			ExecuteTimedTest(() =>
			{
				//Set up two published products
				var (productId1, productFileId1) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl, true);
				var category = CategoryAdminApiService.PostCategory(new CategoryInsertRequest
				{
					Name = categoryName
				}).Result;
				PublishCategory(category.CategoryId);
				PublishProduct(productId1, _productName, ProductWebsiteUrl, category.CategoryId);
				var (productId2, productFileId2) = PostProduct(productName2, productWebsiteUrl2, true);
				PublishProduct(productId2, productName2, productWebsiteUrl2, category.CategoryId);

				//Search for, select, and navigate to the first product
				_productDetailsPage.LinkSideNavProducts.ClickAndWaitForPageToLoad();
				_productsPage.InputFilterProductName.SendKeys(_productName);
				_productsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Model.Pages.ProductsPage.GetProductLinkByRowNumber(1).ClickAndWaitForPageToLoad();

				//Search for, select, and navigate to the second product
				_productDetailsPage.LinkSideNavProducts.ClickAndWaitForPageToLoad();
				_productsPage.InputFilterProductName.SendKeys(productName2);
				_productsPage.ButtonApplyFilters.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Model.Pages.ProductsPage.GetProductLinkByRowNumber(1).ClickAndWaitForPageToLoad();

				//Select a archived status from the product status select box
				_productDetailsPage.SelectProductStatus.Click();
				_productDetailsPage.OptionProductStatusArchived.Click();
				Log("Selected the Archived option from the select box.");

				//Click confirmation dialog
				_productDetailsPage.ButtonConfirmationDialogAction.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the Archive product confirmation button.");

				//Assert the first product remained active and published
				var product1 = ProductAdminApiService.GetProductDetailsById(productId1).Result;
				Assert.AreEqual(ProductStatusType.Active, product1.ProductStatuses.First(ps => ps.IsSelected).ProductStatusTypeId);
				Assert.IsTrue(product1.PublishStatuses.Select(ps => ps.PublishStatusTypeId).All(ps => ps == PublishStatusType.Published));

				//Assert the second product updated to archived and was unpublished from all sites
				var product2 = ProductAdminApiService.GetProductDetailsById(productId2).Result;
				Assert.AreEqual(ProductStatusType.Archived, product2.ProductStatuses.First(ps => ps.IsSelected).ProductStatusTypeId);
				Assert.IsTrue(product2.PublishStatuses.Select(ps => ps.PublishStatusTypeId).All(ps => ps == PublishStatusType.Unpublished));

				//Cleanup
				ProductAdminApiService.DeleteProductFile(productFileId1);
				ProductAdminApiService.DeleteProductFile(productFileId2);
				ProductAdminApiService.DeleteProduct(productId1);
				ProductAdminApiService.DeleteProduct(productId2);
				CategoryAdminApiService.DeleteCategory(category.CategoryId.ToString());
			});
		}

		[Test]
		public void EditProduct_DuplicateProductVendorDetected_Succeeds()
		{
			var product1Name = RequestUtility.GetRandomString(10);
			var product2Name = RequestUtility.GetRandomString(10);
			var vendorName = RequestUtility.GetRandomString(10);
			var _vendorWebsiteUrl = $"https://www.vendorwebsiteurl.com/";

			ExecuteTimedTest(() =>
			{
				//Setup a new vendor
				_productDetailsPage.OpenPage();
				var vendor = PostVendor(vendorName, _vendorWebsiteUrl);

				//create a product associated with the vendor created above
				var product1 = PostProduct(product1Name, ProductWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));
				var product2 = PostProduct(product2Name, ProductWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));

				//Navigate to the Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product2.ProductId, 3000);
				Thread.Sleep(3000);

				//Input the name of the product
				_productDetailsPage.InputFilterProductName.SendKeys(clear:true);
				_productDetailsPage.InputFilterProductName.SendKeys(product1Name);
				Thread.Sleep(3000);

				//Assert the page displays no duplicate vendor product was detected
				Assert.IsTrue(_productDetailsPage.DuplicateVendorProductDetected.IsDisplayed());

				//Cleanup
				var product1Id = ProductAdminApiService.GetProductsByPartialName(product1Name).Result.Single().ProductId.ToString();
				var product2Id = ProductAdminApiService.GetProductsByPartialName(product2Name).Result.Single().ProductId.ToString();
				ProductAdminApiService.DeleteProduct(product1Id);
				ProductAdminApiService.DeleteProduct(product2Id);
				DeleteVendor(vendor.VendorId);
			});
		}

		[Test]
		public void EditProduct_NoDuplicateProductFoundCheckMark_Succeeds()
		{
			var product1Name = RequestUtility.GetRandomString(10);
			var product2Name = RequestUtility.GetRandomString(10);
			var vendorName = RequestUtility.GetRandomString(10);
			var _vendorWebsiteUrl = $"https://www.vendorwebsiteurl.com/";

			ExecuteTimedTest(() =>
			{
				//Setup a new vendor
				_productDetailsPage.OpenPage();
				var vendor = PostVendor(vendorName, _vendorWebsiteUrl);

				//create a product associated with the vendor created above
				var product1 = PostProduct(product1Name, ProductWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));
				var product2 = PostProduct(product2Name, ProductWebsiteUrl, false, Convert.ToInt32(vendor.VendorId));

				//Navigate to the Product Details Page
				BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product2.ProductId, 3000);
				Thread.Sleep(3000);
				
				//Input a product name does not match any of the vendor's other product names
				_productDetailsPage.InputFilterProductName.SendKeys(clear: true);
				_productDetailsPage.InputFilterProductName.SendKeys(RequestUtility.GetRandomString(10));
				Thread.Sleep(2000);

				//Assert that the page displays a No duplicate product found with green checkmark
				Assert.IsTrue(_productDetailsPage.DuplicateVendorProductNoneDetected.IsDisplayed());

				//Cleanup
				var product1Id = ProductAdminApiService.GetProductsByPartialName(product1Name).Result.Single().ProductId.ToString();
				var product2Id = ProductAdminApiService.GetProductsByPartialName(product2Name).Result.Single().ProductId.ToString();
				ProductAdminApiService.DeleteProduct(product1Id);
				ProductAdminApiService.DeleteProduct(product2Id);
				DeleteVendor(vendor.VendorId);
			});
		}


		private void ClickAllDeploymentOptionsAndSave()
		{
			var deploymentOptionControls = new List<Control>
			{
				_productDetailsPage.CheckboxDeploymentOptionCloudSaasWebBased,
				_productDetailsPage.CheckboxDeploymentOptionDesktopMac,
				_productDetailsPage.CheckboxDeploymentOptionDesktopWindows,
				_productDetailsPage.CheckboxDeploymentOptionDesktopLinux,
				_productDetailsPage.CheckboxDeploymentOptionDesktopChromebook,
				_productDetailsPage.CheckboxDeploymentOptionOnPremiseWindows,
				_productDetailsPage.CheckboxDeploymentOptionOnPremiseLinux,
				_productDetailsPage.CheckboxDeploymentOptionMobileAndroid,
				_productDetailsPage.CheckboxDeploymentOptionMobileIPhone,
				_productDetailsPage.CheckboxDeploymentOptionMobileIPad
			};

			//Check the boxes for all existing deployment options
			foreach (var deploymentOptionControl in deploymentOptionControls)
			{
				deploymentOptionControl.Click();
			}

			//Click the Save button
			_productDetailsPage.ButtonSaveProductDetailsChanges.ClickAndWaitForPageToLoad();
			Log("Clicked the save global product button.");
			Thread.Sleep(3000);
		}

		private void PublishCategory(int categoryId)
		{
			CategoryAdminApiService.PutCategoryPublishStatusById(categoryId.ToString(), new CategoryPublishStatusUpdateRequest
			{
				SiteId = SiteType.Capterra,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published category on Capterra.");

			CategoryAdminApiService.PutCategoryPublishStatusById(categoryId.ToString(), new CategoryPublishStatusUpdateRequest
			{
				SiteId = SiteType.GetApp,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published category on GetApp.");

			CategoryAdminApiService.PutCategoryPublishStatusById(categoryId.ToString(), new CategoryPublishStatusUpdateRequest
			{
				SiteId = SiteType.SoftwareAdvice,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published category on Software Advice.");
		}

		private void PublishProduct(string productId, string productName, string productWebsiteUrl, int categoryId)
		{
			ProductAdminApiService.PutProductDetails(productId, new ProductUpdateRequest
			{
				Name = productName,
				ProductWebsiteUrl = productWebsiteUrl,
				TargetCompanySizeTypeIds = new List<CompanySizeType> { CompanySizeType.SelfEmployed },
				DeploymentOptionTypeIds = new List<DeploymentOptionType> { DeploymentOptionType.CloudSaaSWebBased }
			});
			Log("Added company size and deployment option to product.");

			ProductAdminApiService.UpsertProductCategoriesByProductId(new ProductCategoriesUpsertRequest
			{
				ProductId = int.Parse(productId),
				CategoryIds = new List<int>
				{
					categoryId
				}
			});
			Log("Added product to category.");

			var (content, extension) = GetBase64Asset(SoftwareAdviceLogoFileName);
			var fileDto = ProductAdminApiService.PostProductScreenshots(new FileRequest
			{
				Content = content,
				Extension = extension
			}).Result;
			Log("Created Software Advice product screenshot.");

			ProductAdminApiService.PutProductMedias(productId, new ProductMediasUpdateRequest
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
								FileId = fileDto.FileId,
								CaptionDescription = RequestUtility.GetRandomString(8),
								DisplayOrder = 1
							}
						}
					}
				}
			});
			Log("Added Software Advice product screenshot.");

			ProductAdminApiService.PutProductDescriptions(productId, new ProductDescriptionsUpdateRequest
			{
				Sites = new List<ProductSiteDescriptionsUpdateRequest>
				{
					new ProductSiteDescriptionsUpdateRequest
					{
						SiteId = SiteType.Capterra,
						Descriptions = new List<DescriptionUpdateRequest>
						{
							new DescriptionUpdateRequest
							{
								ContentTypeId = ContentType.ShortDescription,
								Description = ContentType.ShortDescription.ToString()
							},
							new DescriptionUpdateRequest
							{
								ContentTypeId = ContentType.LongDescription,
								Description = ContentType.LongDescription.ToString()
							}
						}
					},
					new ProductSiteDescriptionsUpdateRequest
					{
						SiteId = SiteType.GetApp,
						Descriptions = new List<DescriptionUpdateRequest>
						{
							new DescriptionUpdateRequest
							{
								ContentTypeId = ContentType.Tagline,
								Description = ContentType.Tagline.ToString()
							},
							new DescriptionUpdateRequest
							{
								ContentTypeId = ContentType.ShortDescription,
								Description = ContentType.ShortDescription.ToString()
							}
						}
					},
					new ProductSiteDescriptionsUpdateRequest
					{
						SiteId = SiteType.SoftwareAdvice,
						Descriptions = new List<DescriptionUpdateRequest>
						{
							new DescriptionUpdateRequest
							{
								ContentTypeId = ContentType.LongDescription,
								Description = ContentType.LongDescription.ToString()
							}
						}
					}
				}
			});
			Log("Added Capterra, GetApp, and Software Advice product descriptions.");

			ProductAdminApiService.PutProductPublishStatuses(productId, new ProductPublishStatusUpdateRequest
			{
				SiteId = (int)SiteType.Capterra,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published product on Capterra.");

			ProductAdminApiService.PutProductPublishStatuses(productId, new ProductPublishStatusUpdateRequest
			{
				SiteId = (int)SiteType.GetApp,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published product on GetApp.");

			ProductAdminApiService.PutProductPublishStatuses(productId, new ProductPublishStatusUpdateRequest
			{
				SiteId = (int)SiteType.SoftwareAdvice,
				PublishStatusTypeId = PublishStatusType.Published
			});
			Log("Published product on Software Advice.");
		}
	}
}