using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.ProductDetailsPage
{
    [TestFixture]
    [Category("Product")]
    public class ProductReviewCountTests : BaseTest
    {
        private Model.Pages.ProductDetailsPage _productDetailsPage;
        private string _productName;

        public ProductReviewCountTests() : base(nameof(ProductReviewCountTests)) { }

        [SetUp]
        public void SetUp()
        {
            _productDetailsPage = new Model.Pages.ProductDetailsPage();
            _productDetailsPage.OpenPage();
            _productName = RequestUtility.GetRandomString(10);
        }

        [Test]
        public void ProductUpc_ProductReviewCountNonZero_Succeed()
        {
            //Create Product
            var product = ProductAdminApiService.PostProduct(new ProductInsertRequest
            {
                Name = _productName
            }).Result; ;

            //Set up Kafka message
            var template = MessagePublishUtility.UpcMessage.ProductReviewsPriceRating.Replace("EVENTID", Guid.NewGuid().ToString());
            template = template.Replace("PRODUCTID", product.ProductIntegrationId.ToString());

            ExecuteTimedTest(() =>
            {
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();

                // confirm that we started with 0
                Assert.AreEqual(_productDetailsPage.ProductReviewsCount.GetText(), "0");

                // update the review count
                var message = template.Replace("COUNT", "123");
                _ = MessagePublishService.PublishMessage(MessagePublishUtility.UpcTopic.Upc_BatchRequest, message);
                Thread.Sleep(3000);

                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();
                Thread.Sleep(2000);

                // confirm review count number
                var value = _productDetailsPage.ProductReviewsCount.GetText();
                Assert.AreEqual(value, "123");

            });

            ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
        }

        [Test]
        public void ProductUpc_ProductReviewCountOverwriteToZero_Succeed()
        {
            //Create Product
            var product = ProductAdminApiService.PostProduct(new ProductInsertRequest
            {
                Name = _productName
            }).Result; ;


            //load Kafka request template
            var template = MessagePublishUtility.UpcMessage.ProductReviewsPriceRating.Replace("PRODUCTID", product.ProductIntegrationId.ToString());

            ExecuteTimedTest(() =>
            {
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();

                // confirm that we started with 0
                Assert.AreEqual(_productDetailsPage.ProductReviewsCount.GetText(), "0");

                // set up the reivew count
                var messageNonZero = template.Replace("COUNT", "123");
                messageNonZero = messageNonZero.Replace("EVENTID", Guid.NewGuid().ToString());
                _ = MessagePublishService.PublishMessage(MessagePublishUtility.UpcTopic.Upc_BatchRequest, messageNonZero);
                Thread.Sleep(3000);
                
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();
                Thread.Sleep(2000);

                // confirm review count number
                Assert.AreEqual(_productDetailsPage.ProductReviewsCount.GetText(), "123");

                // overwrite to 0
                var messageZero = template.Replace("COUNT", "0");
                messageZero = messageZero.Replace("EVENTID", Guid.NewGuid().ToString());
                _ = MessagePublishService.PublishMessage(MessagePublishUtility.UpcTopic.Upc_BatchRequest, messageZero);
                Thread.Sleep(3000);

                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();
                Thread.Sleep(2000);

                // confirm that value is 0
                Assert.AreEqual(_productDetailsPage.ProductReviewsCount.GetText(), "0");                                                     
            });

            ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
        }

        [Test]
        public void ProductUpc_ProductReviewCountInvalidValue_Failed()
        {
            //Create Product
            var product = ProductAdminApiService.PostProduct(new ProductInsertRequest
            {
                Name = _productName
            }).Result; ;

            //Set up Kafka message
            var template = MessagePublishUtility.UpcMessage.ProductReviewsPriceRating.Replace("EVENTID", Guid.NewGuid().ToString());
            template = template.Replace("PRODUCTID", product.ProductIntegrationId.ToString());

            ExecuteTimedTest(() =>
            {
                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();

                // confirm that we started with 0
                Assert.AreEqual(_productDetailsPage.ProductReviewsCount.GetText(), "0");

                // try to update the review count to invalid value 
                var message = template.Replace("COUNT", "test");
                var response = MessagePublishService.PublishMessage(MessagePublishUtility.UpcTopic.Upc_BatchRequest, message);
                Thread.Sleep(3000);

                BrowserUtility.NavigateToPage(BrowserUtility.ProductPageName, product.ProductId.ToString());
                BrowserUtility.WaitForPageToLoad();

                // confirm that value isn't updated
                var value = _productDetailsPage.ProductReviewsCount.GetText();
                Assert.AreNotEqual(value, "test");
                Assert.AreEqual(value, "0");

            });

            ProductAdminApiService.DeleteProduct(product.ProductId.ToString());
        }
    }
}