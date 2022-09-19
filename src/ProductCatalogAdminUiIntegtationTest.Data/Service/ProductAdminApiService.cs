using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Core.Logging;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Request.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class ProductAdminApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private static LogService _logService;
		private readonly Uri _baseUri;
		private static HttpClient HttpClient { get; set; }

		public ProductAdminApiService(LogService logService)
		{
			_logService = logService;
			var productAdminApiBaseUrl = Configuration.GetValue<string>("ProductAdminApiBaseUrl");
			_baseUri = new Uri(productAdminApiBaseUrl);
		}

		public static void InitializeHttpClient(string bearerToken)
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}

		#region Products

		public void PostSiteProductToProductMapping(SiteProductToProductMapSaveRequest request)
		{
			InvokePostApiCall<object>(new Uri(_baseUri, "api/v1/Map/SiteProducts"), request);
		}

		public ResponsePackage<ProductAdminDto> PostProduct(ProductInsertRequest request)
		{
			return InvokePostApiCall<ProductAdminDto>(new Uri(_baseUri, "api/v1/Products"), request);
		}

		public void DeleteProduct(string productId)
		{
			InvokeDeleteApiCall(new Uri(_baseUri, $"api/v1/Products/{productId}"));
		}

		public void PutProduct(string productId, ProductUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public void PutProductDetails(string productId, Request.V2.ProductUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v2/Products/{productId}"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public ResponsePackage<List<ProductAutocompleteDto>> GetProductsByPartialName(string partialName)
		{
			return InvokeGetApiCall<List<ProductAutocompleteDto>>(new Uri(_baseUri, $"api/v1/Autocomplete/Products/{partialName}"));
		}

		public ResponsePackage<ProductAdminDto> GetProductById(string productId, bool? includeSiteProducts = false)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			if (includeSiteProducts != null)
			{
				query[nameof(includeSiteProducts)] = includeSiteProducts.ToString().ToLower();
			}

			builder.Query = query.ToString();
			return InvokeGetApiCall<ProductAdminDto>(new Uri(builder.ToString()));
		}

		public void PutProductToSiteProductMap(string productId, List<int> request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Map/Products/{productId}"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public ResponsePackage<List<CategorySummaryDto>> UpsertProductCategoriesByProductId(ProductCategoriesUpsertRequest request)
		{
			return InvokePostApiCall<List<CategorySummaryDto>>(new Uri(_baseUri, "api/v1/ProductCategories"), request);
		}

		public ResponsePackage<Dto.V2.ProductDto> GetProductDetailsById(string productId)
		{
			return InvokeGetApiCall<Dto.V2.ProductDto>(new Uri(_baseUri, $"api/v2/Products/{productId}"));
		}

		public ResponsePackage<ProductPricingDto> GetProductPricingById(string productId)
		{
			return InvokeGetApiCall<ProductPricingDto>(new Uri(_baseUri, $"api/v1/Products/{productId}/Pricing"));
		}

		public ResponsePackage<List<ProductDescriptionDto>> GetProductDescriptionsById(string productId)
		{
			return InvokeGetApiCall<List<ProductDescriptionDto>>(new Uri(_baseUri, $"api/v1/Products/{productId}/Descriptions"));
		}

        public void PutProductPricings(string pricingId, ProductPricingUpdateRequest request)
        {
            var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Pricings/{pricingId}"));
            InvokePutApiCall(new Uri(builder.ToString()), request);
        }

		public void PutProductDescriptions(string productId, ProductDescriptionsUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/Descriptions"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public void PutProductStatus(string productId, ProductStatusUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/Status"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public ResponsePackage<List<CategorySummaryDto>> GetProductCategories(string productId)
		{
			return InvokeGetApiCall<List<CategorySummaryDto>>(new Uri(_baseUri, $"api/v1/Products/{productId}/Categories"));
		}
		
		public ResponsePackage<List<ProductFeatureDto>> GetProductCategoryFeatures(int productId)
		{
			return InvokeGetApiCall<List<ProductFeatureDto>>(new Uri(_baseUri, $"api/v1/Products/{productId}/CategoryFeatures"));
		}

		public void PutProductCategory(string productCategoryId, ProductCategoryUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/ProductCategories/{productCategoryId}"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public ResponsePackage<List<ProductMediaDto>> GetProductMedias(string productId)
		{
			return InvokeGetApiCall<List<ProductMediaDto>>(new Uri(_baseUri, $"api/v1/Products/{productId}/Medias"));
		}

		public ResponsePackage<ProductVideoAttributesDto> GetProductVideoAttributes(string url)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/VideoUrls?url={url}"));
			return InvokeGetApiCall<ProductVideoAttributesDto>(builder.Uri);
		}

		public void PutProductMedias(string productId, ProductMediasUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/Medias"));
			InvokePutApiCall(builder.Uri, request);
		}

		public ResponsePackage<FileDto> PostProductScreenshots(FileRequest request)
		{
			return InvokePostApiCall<FileDto>(new Uri(_baseUri, "api/v1/ProductScreenshots"), request);
		}

		public ResponsePackage<ProductLogoDto> PostProductLogo(string productId, FileRequest request)
		{
			return InvokePostApiCall<ProductLogoDto>(new Uri(_baseUri, $"api/v1/Products/{productId}/Logos"), request);
		}

		public void DeleteProductFile(string productFileId)
		{
			InvokeDeleteApiCall(new Uri(_baseUri, $"api/v1/ProductFiles/{productFileId}"));
		}

		public ResponsePackage<ProductIntegrationDto> GetProductIntegrations(string productId)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/Integrations"));
			return InvokeGetApiCall<ProductIntegrationDto>(builder.Uri);
		}

		public void PutProductIntegrations(string productId, ProductIntegrationsUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/Integrations"));
			InvokePutApiCall(builder.Uri, request);
		}

		public void PutProductPublishStatuses(string productId, ProductPublishStatusUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Products/{productId}/PublishStatuses"));
			InvokePutApiCall(builder.Uri, request);
		}

		public ResponsePackage<List<ProductSummaryDto>> SearchProducts(ProductSearchRequest request)
		{
			return InvokePostApiCall<List<ProductSummaryDto>>(new Uri(_baseUri, $"api/v1/Search/Products"), request);
		}

		#endregion

		#region Private methods

		private static ResponsePackage<T> InvokePostApiCall<T>(Uri postUri, object requestObject) where T : class
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			var postResponse = HttpClient.PostAsync(postUri, bodyContent).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(postResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		private static void InvokePutApiCall(Uri putUri, object requestObject)
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			var putResponse = HttpClient.PutAsync(putUri, bodyContent).Result;
			var responseContent = putResponse.Content.ReadAsStringAsync().Result;
			_logService.Log($"HttpStatusCode: {(int)putResponse.StatusCode} - {putResponse.StatusCode}, Content: {responseContent}");
			putResponse.EnsureSuccessStatusCode();
		}

		private static void InvokeDeleteApiCall(Uri deleteUri)
		{
			HttpClient.DeleteAsync(deleteUri).Result.EnsureSuccessStatusCode();
		}

		private static ResponsePackage<T> InvokeGetApiCall<T>(Uri getUri) where T : class
		{
			var getResponse = HttpClient.GetAsync(getUri).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(getResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		#endregion
	}
}