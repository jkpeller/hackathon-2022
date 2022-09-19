using System.Collections.Generic;
using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System.Threading;
using System.Linq;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
	public class ProductPricingTests : BaseTest
	{
		private Model.Pages.ProductsPage _productsPage;
		private Model.Pages.ProductDetailsPage _productDetailsPage;
		private string _productName;
		private const string ProductWebsiteUrl = "http://www.testproductwebsiteurl.com/";
		private string _categoryName;

		public ProductPricingTests() : base(nameof(ProductPricingTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_productsPage = new Model.Pages.ProductsPage();
			_productDetailsPage = new Model.Pages.ProductDetailsPage();
			_productName = RequestUtility.GetRandomString(8);
			_categoryName = RequestUtility.GetRandomString(12);
		}

		[Test]
		[Category("Product")]
		public void ProductPricing_ValidateTrueIsPricingVisibleDisplay_Succeeds()
		{
			const string expectedIsPricingVisibleMessage = "Vendor is hiding pricing information from buyers.";
			ExecuteTimedTest(() =>
			{
				//Open the page
				OpenPage(_productsPage);

				//Setup
			var (productId, categoryId) =  SetUpProductWithMessageApiPut(_productName, _categoryName);		

				//Assert that the true IsPricingVisible value shows the correct message
				Assert.AreEqual(expectedIsPricingVisibleMessage, _productDetailsPage.IsPricingVisibleMessageDisplay.GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
				CategoryAdminApiService.DeleteCategory(categoryId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductPricing_ValidateFalseIsPricingVisibleDisplay_Succeeds()
		{
			const string expectedIsPricingVisibleMessage = "Pricing information is visible to buyers.";
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Assert that the true IsPricingVisible value shows the correct message
				Assert.AreEqual(expectedIsPricingVisibleMessage, _productDetailsPage.IsPricingVisibleMessageDisplay.GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductPricing_ByNoneCurrencyValue_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Select a valid currency option
				_productDetailsPage.SelectCurrency.Click();
				Model.Pages.ProductDetailsPage.GetCurrencyOptionByCode("usd").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Select the None currency option
				_productDetailsPage.SelectCurrency.Click();
				Model.Pages.ProductDetailsPage.GetCurrencyOptionByCode("none").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Click save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//Assert that none of the currency options were saved
				var getResponse = ProductAdminApiService.GetProductPricingById(productId);
				Log($"Product pricing retrieved successfully.");
				Assert.IsFalse(getResponse.Result.Currencies.TrueForAll(c => c.IsSelected));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductPricing_ByNonePriceRangeValue_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Select a valid currency option
				_productDetailsPage.SelectPricingRange.Click();
				Model.Pages.ProductDetailsPage.GetPriceRangeOptionByName("$").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Select the None currency option
				_productDetailsPage.SelectPricingRange.Click();
				Model.Pages.ProductDetailsPage.GetPriceRangeOptionByName("none").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Click save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//Assert that none of the currency options were saved
				var getResponse = ProductAdminApiService.GetProductPricingById(productId);
				Log($"Product pricing retrieved successfully.");
				Assert.IsNull(getResponse.Result.PriceRangeTypeId);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductPricing_ByNonePaymentFrequencyValue_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Select a valid currency option
				_productDetailsPage.SelectPaymentFrequency.Click();
				Model.Pages.ProductDetailsPage.GetPaymentFrequencyOptionByName("one time").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Select the None currency option
				_productDetailsPage.SelectPaymentFrequency.Click();
				Model.Pages.ProductDetailsPage.GetPaymentFrequencyOptionByName("none").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Select a valid cc-verify option
				_productDetailsPage.SelectCCVerifyRequired.Click();
				Model.Pages.ProductDetailsPage.GetCCVerifyRequiredOptionByName("no").ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Click save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();
				Log("Clicked the save product details button.");

				//Assert that none of the currency options were saved
				var getResponse = ProductAdminApiService.GetProductPricingById(productId);
				Log($"Product pricing retrieved successfully.");
				Assert.IsFalse(getResponse.Result.Plans.Single().PaymentFrequencies.TrueForAll(c => c.IsSelected));

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductPricing_ByAboveOldMaximumPricingDescription_Succeeds()
		{
			var updatedPricingDescription = RequestUtility.GetRandomString(501);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the pricing description field above the previous limit of 500 characters
				_productDetailsPage.TextareaPricingDescription.SendKeys(updatedPricingDescription);

                //Click save
                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();
				Log("Clicked the save product pricing button.");
				Thread.Sleep(3000);

				//Get the product pricing from the admin api and validate that it was saved
				var getResponse = ProductAdminApiService.GetProductPricingById(productId);
				Log($"Product pricing returned. ProductId: {productId} Response: {getResponse.ToJson()}");
				Assert.AreEqual(updatedPricingDescription, getResponse.Result.Description);

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void EditProductPricing_ByAboveMaximumPricingDescription_Succeeds()
		{
			var updatedPricingDescription = RequestUtility.GetRandomString(5001);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				//Type a value in the pricing description field above the previous limit of 500 characters
				_productDetailsPage.TextareaPricingDescription.SendKeys(updatedPricingDescription);

				//Click save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitPageToLoadAndOverlayToDisappear();
				Log("Clicked the save product pricing button.");
				Thread.Sleep(3000);

				//Get the product pricing from the admin api and validate that the first 1000 characters were saved
				var getResponse = ProductAdminApiService.GetProductPricingById(productId);
				Log($"Product pricing returned. ProductId: {productId} Response: {getResponse.ToJson()}");
				Assert.AreEqual(updatedPricingDescription.Substring(0, 5000), getResponse.Result.Description);		

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
		[Category("Product")]
		public void ProductPricingPlan_ValidateAllPlanElementsDisplayed_Succeeds()
		{
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				Assert.True(Model.Pages.ProductDetailsPage.GetInputPlanName(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetInputStartingPrice(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetInputEndingPrice(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetRadioButtonPriceTypeCustom(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetRadioButtonPriceTypeNumeric(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetSelectPricingModel(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetSelectPaymentFrequency(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetTextareaPlanDescription(0).ExistsInPage());
				Assert.True(Model.Pages.ProductDetailsPage.GetAttributesAutocomplete(0).ExistsInPage());

				Model.Pages.ProductDetailsPage.GetRadioButtonPriceTypeCustom(0).Click();
                Assert.True(Model.Pages.ProductDetailsPage.GetInputCustomPrice(0).ExistsInPage());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidatePlanNameRequiredToSave_Succeeds()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateCreatePlanWithDuplicateName_Fails()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
			string duplicatePlanName = "Basic";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(duplicatePlanName,true);
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateCreatePlanWithCustomPriceUnder200Chars_Succeeds()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
			string lessThan200CharsCustomPrice = RequestUtility.GetRandomString(15);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
				Model.Pages.ProductDetailsPage.GetRadioButtonPriceTypeCustom(1).ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputCustomPrice(1).SendKeys(lessThan200CharsCustomPrice);

                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing retrieved successfully.");
                Assert.AreEqual(lessThan200CharsCustomPrice, getResponse.Result.Plans.Find(p => p.DisplayOrder == 2)?.CustomPrice);


                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateAddDescriptionToNewPricePlan_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
			string priceDescription = RequestUtility.GetRandomString(100);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
				Model.Pages.ProductDetailsPage.GetTextareaPlanDescription(2).SendKeys(priceDescription);
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateAbleToAddSixPricingPlans_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName3 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName4 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName5 = RequestUtility.GetRandomString(15);

			string priceDescription = RequestUtility.GetRandomString(100);
            ExecuteTimedTest(() =>
            {
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(3).SendKeys(lessThan200CharsPlanName3);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(4).SendKeys(lessThan200CharsPlanName4);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(5).SendKeys(lessThan200CharsPlanName5);

                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Assert.False(_productDetailsPage.ButtonAddAnotherPlan.IsDisplayed());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
			});
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateDeletePricingPlan_Succeeds()
        {
			string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName3 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName4 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName5 = RequestUtility.GetRandomString(15);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(3).SendKeys(lessThan200CharsPlanName3);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(4).SendKeys(lessThan200CharsPlanName4);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(5).SendKeys(lessThan200CharsPlanName5);
				// save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitPageToLoadAndOverlayToDisappear();

				// Reorder
                Model.Pages.ProductDetailsPage.GetIconReOrderPlan(3).ClickAndDragAndDrop(Model.Pages.ProductDetailsPage.GetIconReOrderPlan(2));

				// delete plan
				Model.Pages.ProductDetailsPage.GetButtonDeletePlan(3).ClickAndWaitPageToLoadAndOverlayToDisappear();

				// Assert expected ordering by comparing plan names in each position
                Assert.AreEqual("Basic", Model.Pages.ProductDetailsPage.GetInputPlanName(0).GetTextValue());
				Assert.AreEqual(lessThan200CharsPlanName1, Model.Pages.ProductDetailsPage.GetInputPlanName(1).GetTextValue());
                Assert.AreEqual(lessThan200CharsPlanName3, Model.Pages.ProductDetailsPage.GetInputPlanName(2).GetTextValue());
                Assert.AreEqual(lessThan200CharsPlanName4, Model.Pages.ProductDetailsPage.GetInputPlanName(3).GetTextValue());
                Assert.AreEqual(lessThan200CharsPlanName5, Model.Pages.ProductDetailsPage.GetInputPlanName(4).GetTextValue());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
            });
        }

		[Test]
		[Category("Product")]
		public void ProductPricingPlan_ValidateReorderPricingPlans_Succeeds()
		{
			string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
			string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
			string lessThan200CharsPlanName3 = RequestUtility.GetRandomString(15);
			string lessThan200CharsPlanName4 = RequestUtility.GetRandomString(15);
			string lessThan200CharsPlanName5 = RequestUtility.GetRandomString(15);
			ExecuteTimedTest(() =>
			{
				var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputPlanName(3).SendKeys(lessThan200CharsPlanName3);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputPlanName(4).SendKeys(lessThan200CharsPlanName4);

				_productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
				Model.Pages.ProductDetailsPage.GetInputPlanName(5).SendKeys(lessThan200CharsPlanName5);
				// save
				_productDetailsPage.ButtonSaveProductPricing.ClickAndWaitPageToLoadAndOverlayToDisappear();

				//Reorder
				Model.Pages.ProductDetailsPage.GetIconReOrderPlan(3).ClickAndDragAndDrop(Model.Pages.ProductDetailsPage.GetIconReOrderPlan(2));
                Model.Pages.ProductDetailsPage.GetIconReOrderPlan(5).ClickAndDragAndDrop(Model.Pages.ProductDetailsPage.GetIconReOrderPlan(4));


				//Assert expected ordering by comparing plan names in each position
				Assert.AreEqual("Basic", Model.Pages.ProductDetailsPage.GetInputPlanName(0).GetTextValue());
				Assert.AreEqual(lessThan200CharsPlanName1, Model.Pages.ProductDetailsPage.GetInputPlanName(1).GetTextValue());
				Assert.AreEqual(lessThan200CharsPlanName3, Model.Pages.ProductDetailsPage.GetInputPlanName(2).GetTextValue());
                Assert.AreEqual(lessThan200CharsPlanName2, Model.Pages.ProductDetailsPage.GetInputPlanName(3).GetTextValue());
				Assert.AreEqual(lessThan200CharsPlanName4, Model.Pages.ProductDetailsPage.GetInputPlanName(5).GetTextValue());
				Assert.AreEqual(lessThan200CharsPlanName5, Model.Pages.ProductDetailsPage.GetInputPlanName(4).GetTextValue());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
			});
		}

		[Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateAddPricingModelToNewPricingPlan_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            string priceModelOption = "Per User";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
                Model.Pages.ProductDetailsPage.GetSelectPricingModel(2).Click();
                Model.Pages.ProductDetailsPage.GetOptionPricingModelByName(priceModelOption, 2).Click();
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing retrieved successfully.");
                Assert.AreEqual(priceModelOption, getResponse.Result.Plans.Find(p => p.DisplayOrder == 3)?.PricingModels.FirstOrDefault(pm => pm.IsSelected)?.Name);

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateAddPaymentFrequencyToNewPricingPlan_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            string paymentFrequencyOption = "Per Year";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
                Model.Pages.ProductDetailsPage.GetSelectPaymentFrequency(2).Click();
                Model.Pages.ProductDetailsPage.GetOptionPaymentFrequencyByName(paymentFrequencyOption, 2).Click();
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateAddDiscountTypesToPricing_Succeeds()
        {
            string pricingDiscountTypeOption = "By Usage";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);


                _productDetailsPage.InputDiscountTypes.Click();
                Model.Pages.ProductDetailsPage.GetOptionDiscountTypeByName(pricingDiscountTypeOption).Click();
                _productDetailsPage.InputDiscountTypes.SendKeys();
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing retrieved successfully.");
                Assert.AreEqual(pricingDiscountTypeOption, getResponse.Result.DiscountTypes.FirstOrDefault(dt => dt.IsSelected)?.Name);

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

		[Test]
		[Category("Product")]
		public void ProductPricingPlan_ValidateModifiedByIsUpdatedCorrectly_Succeeds()
        {
			string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();
                Assert.AreEqual("Updated by: test gdmtechatxtest1", _productDetailsPage.ModifiedBy.GetText());
				Assert.AreEqual("Just now", _productDetailsPage.ModifiedOnUtc.GetText());

				//Cleanup
				ProductAdminApiService.DeleteProduct(productId);
            });
		}

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateValidVendorUrlAccepted_Succeeds()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
            string validVendorUrl = $"https://www.{RequestUtility.GetRandomString(10)}.com";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

				_productDetailsPage.InputVendorPricingUrl.SendKeys(validVendorUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                Assert.AreEqual(validVendorUrl, _productDetailsPage.InputVendorPricingUrl.GetTextValue());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
		}

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateEndingPriceMustBeLessThanStarting_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            decimal startingPrice = 100;
            decimal endingPrice = startingPrice - 50;
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
				Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).SendKeys(startingPrice+"");
                Model.Pages.ProductDetailsPage.GetInputEndingPrice(2).SendKeys(endingPrice + "");
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).Click();

				Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
				Assert.True(Model.Pages.ProductDetailsPage.GetInputEndingPriceGreaterThanErrorMessage(2).ExistsInPage());

                //Cleanup
				ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateEndingPriceIsOptional_Succeeds()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
            decimal startingPrice = 100;
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(1).SendKeys(startingPrice + "");
                // leave ending price blank
                Assert.True(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing retrieved successfully.");
                Assert.AreEqual(startingPrice, getResponse.Result.Plans.Find(p => p.DisplayOrder == 2)?.StartingPrice);
                Assert.Null(getResponse.Result.Plans.Find(p => p.DisplayOrder == 2)?.EndingPrice);

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateDefaultsToNumericType_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();

                // Assert Numeric is selected and starting price and ending price shown
                Assert.True(Model.Pages.ProductDetailsPage.GetRadioButtonPriceTypeNumeric(1).HasClass("mat-radio-checked"));
                Assert.True(Model.Pages.ProductDetailsPage.GetInputStartingPrice(1).ExistsInPage());
                Assert.True(Model.Pages.ProductDetailsPage.GetInputEndingPrice(1).ExistsInPage());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
		}

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateEndingPriceMinAndMaxValue_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            decimal startingPrice = 0;
            decimal endingPriceMin = 0;
            decimal endingPriceMax = 999999999999999;
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).SendKeys(startingPrice + "");
                Model.Pages.ProductDetailsPage.GetInputEndingPrice(2).SendKeys(endingPriceMin-1 + "");
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).Click();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Assert.True(Model.Pages.ProductDetailsPage.GetInputEndingPriceDecimalErrorMessage(2).ExistsInPage());

                Model.Pages.ProductDetailsPage.GetInputEndingPrice(2).SendKeys(endingPriceMax+1 + "", true);
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).Click();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Assert.True(Model.Pages.ProductDetailsPage.GetInputEndingPriceMaxErrorMessage(2).ExistsInPage());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateDiscountTypesCanBeRemoved_Succeeds()
        {
            string pricingDiscountTypeOption1 = "By Usage";
            string pricingDiscountTypeOption2 = "By Seat";
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);


                _productDetailsPage.InputDiscountTypes.Click();
                Model.Pages.ProductDetailsPage.GetOptionDiscountTypeByName(pricingDiscountTypeOption1).Click();
                Model.Pages.ProductDetailsPage.GetOptionDiscountTypeByName(pricingDiscountTypeOption2).Click();
                _productDetailsPage.InputDiscountTypes.SendKeys();
                Assert.True(Model.Pages.ProductDetailsPage.GetRemoveChipDiscountTypeByName(pricingDiscountTypeOption1).IsDisplayed());
                Assert.True(Model.Pages.ProductDetailsPage.GetRemoveChipDiscountTypeByName(pricingDiscountTypeOption2).IsDisplayed());

                Model.Pages.ProductDetailsPage.GetRemoveChipDiscountTypeByName(pricingDiscountTypeOption1).Click();

                Assert.False(Model.Pages.ProductDetailsPage.GetRemoveChipDiscountTypeByName(pricingDiscountTypeOption1).ExistsInPage());
                Assert.True(Model.Pages.ProductDetailsPage.GetRemoveChipDiscountTypeByName(pricingDiscountTypeOption2).IsDisplayed());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_VerifyInvalidVendorPriceLinkIsNotAccepted_Succeeds()
        {
            string lessThan200CharsPlanName = RequestUtility.GetRandomString(15);
            string invalidVendorUrl = InvalidUrls.FirstOrDefault();
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.InputVendorPricingUrl.SendKeys(invalidVendorUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName);
                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitForPageToLoad();

                Assert.AreEqual(invalidVendorUrl, _productDetailsPage.InputVendorPricingUrl.GetTextValue());
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateModifiedByIsUpdatedAfterAPICalled_Succeeds()
        {
            ExecuteTimedTest(() =>
            {
                //Open the page
                OpenPage(_productsPage);

                var (productId, _) = PostProduct(_productName, ProductWebsiteUrl);

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing returned. ProductId: {productId} Response: {getResponse.ToJson()}");

                var pricingId = getResponse.Result.PricingId;
                var planRequest = new ProductPricingPlanUpdateRequest()
                {
                    Name = getResponse.Result.Plans.FirstOrDefault()?.Name,
                    Description = getResponse.Result.Plans.FirstOrDefault()?.Description,
                    DisplayOrder = (int) getResponse.Result.Plans.FirstOrDefault()?.DisplayOrder,
                    PricingModelTypeId = PricingModelType.Other,
                    StartingPrice = getResponse.Result.Plans.FirstOrDefault()?.StartingPrice,
                    PaymentFrequencyTypeId = PaymentFrequencyType.OneTime,
                    PriceTypeId = PriceType.Numeric
                };
                var pricingBody = new ProductPricingUpdateRequest
                {
                    Description = $"Description for {_productName}",
                    Plans = new List<ProductPricingPlanUpdateRequest>(){planRequest}
                };
                // Update pricing with API and then verify this is reflected in the 'updated by' label in the details page
                ProductAdminApiService.PutProductPricings($"{pricingId}", pricingBody);

                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, productId);

                Assert.AreEqual("Updated by: test gdmtechatxtest1", _productDetailsPage.ModifiedBy.GetText());
                Assert.AreEqual("Just now", _productDetailsPage.ModifiedOnUtc.GetText());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_ValidateStartingPriceMinAndMaxValue_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            decimal startingPriceMin = 0;
            decimal startingPriceMax = 999999999999999;
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);
                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).SendKeys($"{startingPriceMin - 1}");
                Model.Pages.ProductDetailsPage.GetInputEndingPrice(2).Click();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Assert.True(Model.Pages.ProductDetailsPage.GetInputStartingPriceDecimalErrorMessage(2).ExistsInPage());

                Model.Pages.ProductDetailsPage.GetInputStartingPrice(2).SendKeys($"{startingPriceMax + 1}", true);
                Model.Pages.ProductDetailsPage.GetInputEndingPrice(2).Click();
                Assert.False(_productDetailsPage.ButtonSaveProductPricing.IsDisplayed());
                Assert.True(Model.Pages.ProductDetailsPage.GetInputStartingPriceMaxErrorMessage(2).ExistsInPage());

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }

        [Test]
        [Category("Product")]
        public void ProductPricingPlan_NewPricingCardsSavedInNewPricingModel_Succeeds()
        {
            string lessThan200CharsPlanName1 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName2 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName3 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName4 = RequestUtility.GetRandomString(15);
            string lessThan200CharsPlanName5 = RequestUtility.GetRandomString(15);
            ExecuteTimedTest(() =>
            {
                var (productId, _) = PostAndNavigateToProduct(_productsPage, _productName, ProductWebsiteUrl);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(1).SendKeys(lessThan200CharsPlanName1);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(2).SendKeys(lessThan200CharsPlanName2);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(3).SendKeys(lessThan200CharsPlanName3);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(4).SendKeys(lessThan200CharsPlanName4);

                _productDetailsPage.ButtonAddAnotherPlan.ClickAndWaitForPageToLoad();
                Model.Pages.ProductDetailsPage.GetInputPlanName(5).SendKeys(lessThan200CharsPlanName5);
                // save
                _productDetailsPage.ButtonSaveProductPricing.ClickAndWaitPageToLoadAndOverlayToDisappear();

                var getResponse = ProductAdminApiService.GetProductPricingById(productId);
                Log($"Product pricing returned. ProductId: {productId} Response: {getResponse.ToJson()}");

                //Assert number of plans
                Assert.AreEqual(6, getResponse.Result.Plans.Count);
                Assert.AreEqual("Basic", getResponse.Result.Plans.ElementAt(0).Name);
                Assert.AreEqual(lessThan200CharsPlanName1, getResponse.Result.Plans.ElementAt(1).Name);
                Assert.AreEqual(lessThan200CharsPlanName2, getResponse.Result.Plans.ElementAt(2).Name);
                Assert.AreEqual(lessThan200CharsPlanName3, getResponse.Result.Plans.ElementAt(3).Name);
                Assert.AreEqual(lessThan200CharsPlanName4, getResponse.Result.Plans.ElementAt(4).Name);
                Assert.AreEqual(lessThan200CharsPlanName5, getResponse.Result.Plans.ElementAt(5).Name);

                //Cleanup
                ProductAdminApiService.DeleteProduct(productId);
            });
        }
    }
}