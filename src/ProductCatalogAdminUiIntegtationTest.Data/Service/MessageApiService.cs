using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ProductUpdateRequest = ProductCatalogAdminUiIntegrationTest.Data.Request.MessageApi.ProductUpdateRequest;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class MessageApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private readonly Uri _baseUri;
		private readonly string _jwtToken;
		private static HttpClient HttpClient { get; set; }

		public MessageApiService()
		{
			var messageApiBaseUrl = Configuration.GetValue<string>("MessageApiBaseUrl");
			_jwtToken = Configuration.GetValue<string>("JwtToken");
			_baseUri = new Uri(messageApiBaseUrl);
			InitializeHttpClient();
		}

		private void InitializeHttpClient()
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
		}

		#region ListingRequests

		public ResponsePackage<Dto.V1.MessageApi.ListingRequestDto> GetListingRequestById(string listingRequestId)
		{
			return InvokeGetApiCall<Dto.V1.MessageApi.ListingRequestDto>(new Uri(_baseUri, $"api/v1/ListingRequests/{listingRequestId}"));
		}

		public ResponsePackage<Dto.V1.MessageApi.ListingRequestDto> PostListingRequest(ListingRequestInsertRequest request)
		{
			return InvokePostApiCall<Dto.V1.MessageApi.ListingRequestDto>(new Uri(_baseUri, "/api/v1/ListingRequests"), request);
		}

		#endregion

		#region Products

		public ResponsePackage<ProductDynamicDto> PutProduct(Guid id, ProductUpdateRequest request)
		{
			var response = InvokePutApiCall<JObject>(new Uri(_baseUri, $"/api/v1/Products/{id}"), request);
			return new ResponsePackage<ProductDynamicDto>
			{
				Errors = response.Errors,
				HttpStatusCode = response.HttpStatusCode,
				Messages = response.Messages,
				Result = response.Result == null ? null : new ProductDynamicDto(response.Result),
				Page = response.Page
			};
		}

		#endregion

		#region Private methods

		private static ResponsePackage<T> InvokeGetApiCall<T>(Uri getUri) where T : class
		{
			var getResponse = HttpClient.GetAsync(getUri).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(getResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		private static ResponsePackage<T> InvokePostApiCall<T>(Uri postUri, object requestObject) where T : class
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			var postResponse = HttpClient.PostAsync(postUri, bodyContent).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(postResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		private void InvokeDeleteApiCall(Uri deleteUri)
		{
			HttpClient.DeleteAsync(deleteUri).Result.EnsureSuccessStatusCode();

			InitializeHttpClient();
		}

		private static ResponsePackage<T> InvokePutApiCall<T>(Uri putUri, object requestObject) where T : class
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			var putResponse = HttpClient.PutAsync(putUri, bodyContent).Result;
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(putResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		#endregion
	}
}