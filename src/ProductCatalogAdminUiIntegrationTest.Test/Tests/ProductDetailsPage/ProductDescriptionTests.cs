using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	[TestFixture]
	public class ProductDescriptionTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";

		public ProductDescriptionTests() : base(nameof(ProductDescriptionTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
			_productsPage = new Model.Pages.ProductsPage();
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_AllRequiredValues_Capterra_Succeeds()
		{
			var shortDescription = RequestUtility.GetRandomString(100);
			var longDescription = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the short description field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Capterra.");
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarDescriptionsCapterra);

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();

				//Assert that short and long descriptions were saved
				Assert.AreEqual(shortDescription, capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(longDescription, capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());
				Assert.IsTrue(_productDetailsPage.TabDescriptionsCapterra.HasClass(ActiveTabClassName));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_AllRequiredValues_SoftwareAdvice_Succeeds()
		{
			var longDescription = RequestUtility.GetRandomString(150);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click Software Advice tab
				_productDetailsPage.TabDescriptionsSoftwareAdvice.Click();
				Log("Clicked Software Advice tab for descriptions.");

				//Type a value in the long description field <= 5000 characters
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveSoftwareAdviceProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Software Advice.");
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarDescriptionsSoftwareAdvice);

				//Get Product descripions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product description retrieved successfully. Response: {getResponse.ToJson()}");
				var softwareAdviceDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.SoftwareAdvice).Select(d => d.Descriptions).FirstOrDefault();

				//Assert that long description was saved
				Assert.AreEqual(longDescription, softwareAdviceDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());
				Assert.IsTrue(_productDetailsPage.TabDescriptionsSoftwareAdvice.HasClass(ActiveTabClassName));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_AllRequiredValues_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = RequestUtility.GetRandomString(200);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.Click();
				Log("Clicked the save descriptions button for GetApp.");
				BrowserUtility.WaitForElementToDisappear(Model.Pages.ProductDetailsPage.MatProgressBarDescriptionsGetApp);

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();

				//Assert that tagline and short description were saved
				Assert.AreEqual(tagline, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Tagline).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(shortDescription, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());
				Assert.IsTrue(_productDetailsPage.TabDescriptionsGetApp.HasClass(ActiveTabClassName));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_AllValidValues_Capterra_Succeeds()
		{
			var targetMarket = RequestUtility.GetRandomString(150);
			var shortDescription = RequestUtility.GetRandomString(100);
			var longDescription = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the target market field <= 200 characters
				_productDetailsPage.TextareaCapterraTargetMarket.SendKeys(targetMarket);

				//Type a value in the short description field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Capterra.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();

				//Assert that descriptions were saved
				Assert.AreEqual(targetMarket, capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.TargetMarket).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(shortDescription, capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(longDescription, capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_AllValidValues_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = RequestUtility.GetRandomString(200);
			var longDescription = RequestUtility.GetRandomString(300);
			var benefits = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 5000 characters
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription);

				//Type a value in the benefits field <= 5000 characters
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits);

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();

				//Assert that descriptions were saved
				Assert.AreEqual(tagline, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Tagline).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(shortDescription, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(longDescription, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(benefits, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Benefits).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumShortDescription_Capterra_Succeeds()
		{
			var shortDescription = RequestUtility.GetRandomString(136);
			var longDescription = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the short description above the max of 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Capterra.");

				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the short description has been saved to only the first 135 characters of the above maximum value
				Assert.AreEqual(shortDescription.Substring(0, shortDescription.Length - 1), capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumTargetMarket_Capterra_Succeeds()
		{
			var targetMarket = RequestUtility.GetRandomString(201);
			var shortDescription = RequestUtility.GetRandomString(50);
			var longDescription = RequestUtility.GetRandomString(100);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the target market above the max of 200 characters
				_productDetailsPage.TextareaCapterraTargetMarket.SendKeys(targetMarket);

				//Type a value in the short description field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Capterra.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the target market has been saved to only the first 200 characters of the above maximum value
				Assert.AreEqual(targetMarket.Substring(0, targetMarket.Length - 1), capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.TargetMarket).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumLongDescription_Capterra_Succeeds()
		{
			var shortDescription = RequestUtility.GetRandomString(100);
			var longDescription = RequestUtility.GetRandomString(1001);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the long description field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description above the max of 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Click save
				_productDetailsPage.ButtonSaveCapterraProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Capterra.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the long description has been saved to only the first 1000 characters of the above maximum value
				Assert.AreEqual(longDescription.Substring(0, longDescription.Length - 1), capterraDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumTagline_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(61);
			var shortDescription = RequestUtility.GetRandomString(100);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline above the max of 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the tagline has been saved to only the first 60 characters of the above maximum value
				Assert.AreEqual(tagline.Substring(0, tagline.Length - 1), getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Tagline).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumShortDescription_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = RequestUtility.GetRandomString(501);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description above the max of 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the short description has been saved to only the first 500 characters of the above maximum value
				Assert.AreEqual(shortDescription.Substring(0, shortDescription.Length - 1), getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumLongDescription_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = RequestUtility.GetRandomString(200);
			var longDescription = RequestUtility.GetRandomString(5001);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Type a value in the long description above the max of 5000 characters
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription.Substring(0, 1000));
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription.Substring(1000, 1000));
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription.Substring(2000, 1000));
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription.Substring(3000, 1000));
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription.Substring(4000, 1001));

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the long description has been saved to only the first 5000 characters of the above maximum value
				Assert.AreEqual(longDescription.Substring(0, longDescription.Length - 1), getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumBenefits_GetApp_Succeeds()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = RequestUtility.GetRandomString(200);
			var benefits = RequestUtility.GetRandomString(5001);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Type a value in the benefits above the max of 5000 characters
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits.Substring(0, 1000));
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits.Substring(1000, 1000));
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits.Substring(2000, 1000));
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits.Substring(3000, 1000));
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits.Substring(4000, 1001));

				//Click save
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that the benefits description has been saved to only the first 5000 characters of the above maximum value
				Assert.AreEqual(benefits.Substring(0, benefits.Length - 1), getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Benefits).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByAboveMaximumLongDescription_SoftwareAdvice_Succeeds()
		{
			var longDescription = RequestUtility.GetRandomString(5001);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click Software Advice tab
				_productDetailsPage.TabDescriptionsSoftwareAdvice.Click();
				Log("Clicked Software Advice tab for descriptions.");

				//Type a value in the long description above the max of 5000 characters
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription.Substring(0, 1000));
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription.Substring(1000, 1000));
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription.Substring(2000, 1000));
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription.Substring(3000, 1000));
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription.Substring(4000, 1001));

				//Click save
				_productDetailsPage.ButtonSaveSoftwareAdviceProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for Software Advice.");

				//Get Product descriptions
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product description retrieved successfully. Response: {getResponse.ToJson()}");
				var softwareAdviceDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.SoftwareAdvice).Select(d => d.Descriptions).FirstOrDefault();
				
				//Assert that long description was saved
				Assert.AreEqual(longDescription.Substring(0, longDescription.Length - 1), softwareAdviceDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.LongDescription).Select(d => d.Description).FirstOrDefault());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByEmptyShortDescription_Capterra_Fails()
		{
			var targetMarket = RequestUtility.GetRandomString(150);
			var shortDescription = string.Empty;
			var longDescription = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the target market field <= 200 characters
				_productDetailsPage.TextareaCapterraTargetMarket.SendKeys(targetMarket);

				//Type an empty short description
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Assert save button is disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveCapterraProductDescriptions.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByEmptyLongDescription_Capterra_Fails()
		{
			var targetMarket = RequestUtility.GetRandomString(150);
			var shortDescription = RequestUtility.GetRandomString(135);
			var longDescription = string.Empty;
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the target market field <= 200 characters
				_productDetailsPage.TextareaCapterraTargetMarket.SendKeys(targetMarket);

				//Type a value in the short description field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescription);

				//Type an empty long description
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescription);

				//Assert save button is disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveCapterraProductDescriptions.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByEmptyTagline_GetApp_Fails()
		{
			var tagline = string.Empty;
			var shortDescription = RequestUtility.GetRandomString(200);
			var longDescription = RequestUtility.GetRandomString(300);
			var benefits = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type an empty tagline
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type a value in the short description field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 5000 characters
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription);

				//Type a value in the benefits field <= 5000 characters
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits);

				//Assert save button is disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveGetAppProductDescriptions.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByEmptyShortDescription_GetApp_Fails()
		{
			var tagline = RequestUtility.GetRandomString(60);
			var shortDescription = string.Empty;
			var longDescription = RequestUtility.GetRandomString(300);
			var benefits = RequestUtility.GetRandomString(300);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(tagline);

				//Type an empty string
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescription);

				//Type a value in the long description field <= 5000 characters
				_productDetailsPage.TextareaGetAppLongDescription.SendKeys(longDescription);

				//Type a value in the benefits field <= 5000 characters
				_productDetailsPage.TextareaGetAppBenefits.SendKeys(benefits);

				//Assert save button is disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveGetAppProductDescriptions.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_ByEmptyLongDescription_SoftwareAdvice_Fails()
		{
			var longDescription = string.Empty;
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Click Software Advice tab
				_productDetailsPage.TabDescriptionsSoftwareAdvice.Click();
				Log("Clicked Software Advice tab for descriptions.");

				//Type an empty long description
				_productDetailsPage.TextareaSoftwareAdviceLongDescription.SendKeys(longDescription);

				//Assert save button is disabled
				Assert.IsFalse(_productDetailsPage.ButtonSaveSoftwareAdviceProductDescriptions.IsDisplayed());
				
				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductDescriptionValidation_SaveButtonFunctionality_Succeeds()
		{
			var shortDescriptionCapterra = RequestUtility.GetRandomString(100);
			var longDescriptionCapterra = RequestUtility.GetRandomString(300);
			var taglineGetApp = RequestUtility.GetRandomString(60);
			var shortDescriptionGetApp = RequestUtility.GetRandomString(100);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the short description for capterra field <= 135 characters
				_productDetailsPage.TextareaCapterraShortDescription.SendKeys(shortDescriptionCapterra);

				//Type a value in the long description for capterra field <= 1000 characters
				_productDetailsPage.TextareaCapterraLongDescription.SendKeys(longDescriptionCapterra);

				//Click GetApp tab
				_productDetailsPage.TabDescriptionsGetApp.Click();
				Log("Clicked GetApp tab for descriptions.");

				//Type a value in the tagline for GetApp field <= 60 characters
				_productDetailsPage.TextareaGetAppTagline.SendKeys(taglineGetApp);

				//Type a value in the long description for capterra field <= 500 characters
				_productDetailsPage.TextareaGetAppShortDescription.SendKeys(shortDescriptionGetApp);

				//Click save GetApp descriptions
				_productDetailsPage.ButtonSaveGetAppProductDescriptions.ClickAndWaitForPageToLoad();
				Log("Clicked the save descriptions button for GetApp.");

				
				var getResponse = ProductAdminApiService.GetProductDescriptionsById(productId);
				Log($"Product descriptions retrieved successfully. Response: {getResponse.ToJson()}");

				//Assert that tagline and short descriptions were saved GetApp
				var getAppDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.GetApp).Select(d => d.Descriptions).FirstOrDefault();
				Assert.AreEqual(taglineGetApp, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.Tagline).Select(d => d.Description).FirstOrDefault());
				Assert.AreEqual(shortDescriptionGetApp, getAppDescriptions?.Where(d => d.ContentTypeId == (int)ContentType.ShortDescription).Select(d => d.Description).FirstOrDefault());

				//Assert that short and long descriptions for Capterra were not saved
				var capterraDescriptions = getResponse.Result.Where(d => d.SiteId == (int)SiteType.Capterra).Select(d => d.Descriptions).FirstOrDefault();
				Assert.IsNull(capterraDescriptions);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

	}
}