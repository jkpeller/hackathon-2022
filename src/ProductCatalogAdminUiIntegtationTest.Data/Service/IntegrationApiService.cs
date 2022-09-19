using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class IntegrationApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private readonly Uri _baseUri;
		private readonly string _jwtToken;
		private static HttpClient HttpClient { get; set; }

		public IntegrationApiService()
		{
			var integrationApiBaseUrl = Configuration.GetValue<string>("IntegrationApiBaseUrl");
			_jwtToken = Configuration.GetValue<string>("JwtToken");
			_baseUri = new Uri(integrationApiBaseUrl);
			InitializeHttpClient();
		}

		private void InitializeHttpClient()
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
		}

		#region Source Site Categories

		public void PostSourceSiteCategory(SourceSiteCategorySingleSaveRequest request)
		{
			InvokePostApiCall(new Uri(_baseUri, "api/v1/SourceSiteCategories"), request);
		}

		public ResponsePackage<SourceSiteCategoryDto> GetSourceSiteCategory(string siteCode, string categoryId)
		{
			var builder = new UriBuilder(new Uri(_baseUri, "api/v1/SourceSiteCategories"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			query[nameof(siteCode)] = siteCode;
			query[nameof(categoryId)] = categoryId;

			builder.Query = query.ToString();
			return InvokeGetApiCall<SourceSiteCategoryDto>(new Uri(builder.ToString()));
		}

		public void DeleteSourceSiteCategory(string siteCode, string categoryId)
		{
			var builder = new UriBuilder(new Uri(_baseUri, "api/v1/SourceSiteCategories"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			query[nameof(siteCode)] = siteCode;
			query[nameof(categoryId)] = categoryId;

			builder.Query = query.ToString();
			InvokeDeleteApiCall(new Uri(builder.ToString()));
		}

		#endregion

		#region Source Site Products

		public void PostSourceSiteProduct(SourceSiteProductSaveRequest request)
		{
			InvokePostApiCall(new Uri(_baseUri, "api/v1/SourceSiteProducts"), request);
		}

		public void DeleteSourceSiteProduct(string productId)
		{
			var builder = new UriBuilder(new Uri(_baseUri, "api/v1/SourceSiteProducts"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["siteCode"] = RequestUtility.SiteCode;
			query[nameof(productId)] = productId;

			builder.Query = query.ToString();
			InvokeDeleteApiCall(new Uri(builder.ToString()));
		}

		public void DeleteSourceSiteProducts(IEnumerable<string> siteProductIds)
		{
			foreach (var siteProductId in siteProductIds)
			{
				DeleteSourceSiteProduct(siteProductId);
			}
		}

		public ResponsePackage<SourceSiteProductDto> GetSourceSiteProduct(string productId)
		{
			var builder = new UriBuilder(new Uri(_baseUri, "api/v1/SourceSiteProducts"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			query["siteCode"] = RequestUtility.SiteCode;
			query[nameof(productId)] = productId;

			builder.Query = query.ToString();
			return InvokeGetApiCall<SourceSiteProductDto>(new Uri(builder.ToString()));
		}

		public List<int> PostAndGetSiteProducts(IEnumerable<SourceSiteProductSaveRequest> requests)
		{
			var productCatalogSiteProductIds = new List<int>();
			foreach (var request in requests)
			{
				PostSourceSiteProduct(request);
				productCatalogSiteProductIds.Add(GetSourceSiteProduct(request.ProductId).Result.ProductCatalogSiteProductId);
			}

			return productCatalogSiteProductIds;
		}

		#endregion

		#region Private methods

		private static ResponsePackage<T> InvokeGetApiCall<T>(Uri getUri) where T : class
		{
			var getResponse = HttpClient.GetAsync(getUri).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(getResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		private static void InvokePostApiCall(Uri postUri, object requestObject)
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpClient.PostAsync(postUri, bodyContent).Result.EnsureSuccessStatusCode();
		}

		private void InvokeDeleteApiCall(Uri deleteUri)
		{
			HttpClient.DeleteAsync(deleteUri).Result.EnsureSuccessStatusCode();

			InitializeHttpClient();
		}

		#endregion
	}
}