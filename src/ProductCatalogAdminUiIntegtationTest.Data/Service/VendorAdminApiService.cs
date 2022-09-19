using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Web;
using System;
using Microsoft.Extensions.Configuration;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class VendorAdminApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private readonly Uri _baseUri;
		private static HttpClient HttpClient { get; set; }

		public VendorAdminApiService()
		{
			var vendorAdminApiBaseUrl = Configuration.GetValue<string>("VendorAdminApiBaseUrl");
			_baseUri = new Uri(vendorAdminApiBaseUrl);
		}

		public static void InitializeHttpClient(string bearerToken)
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}

		public ResponsePackage<List<VendorSummaryDto>> SearchVendors(VendorSearchRequest request)
		{
			return SearchVendors((object)request);
		}

		private ResponsePackage<List<VendorSummaryDto>> SearchVendors(object request)
		{
			return InvokePostApiCall<List<VendorSummaryDto>>(new Uri(_baseUri, "/api/v1/Search/Vendors"), request);
		}

		public ResponsePackage<VendorDto> PostVendor(VendorInsertRequest request)
		{
			return InvokePostApiCall<VendorDto>(new Uri(_baseUri, "/api/v1/Vendors"), request);
		}

		public VendorDto GetVendorById(string vendorId)
		{
			return InvokeGetApiCall<VendorDto>(new Uri(_baseUri, $"/api/v1/Vendors/{vendorId}")).Result;
		}

		public void DeleteVendor(string vendorId)
		{
			InvokeDeleteApiCall(new Uri(_baseUri, $"/api/v1/Vendors/{vendorId}"));
		}

		public ResponsePackage<ListingRequestDto> PutListingRequest(string listingRequestId, ListingRequestUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"/api/v1/ListingRequests/{listingRequestId}"));
			return InvokePutApiCall<ListingRequestDto>(new Uri(builder.ToString()), request);
		}

		public ListingRequestDto GetListingRequestById(string listingRequestId)
		{
			return InvokeGetApiCall<ListingRequestDto>(new Uri(_baseUri, $"/api/v1/ListingRequests/{listingRequestId}")).Result;
		}

		public ResponsePackage<VendorNotesDto> PutVendorNotes(string vendorId, VendorNotesUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"/api/v1/Vendors/{vendorId}/Notes"));
			return InvokePutApiCall<VendorNotesDto>(new Uri(builder.ToString()), request);
		}

		public ResponsePackage<VendorLogoDto> PutVendorLogo(string vendorId, VendorLogoUpsertRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"/api/v1/Vendors/{vendorId}/Logos"));
			return InvokePutApiCall<VendorLogoDto>(new Uri(builder.ToString()), request);
		}


		public ResponsePackage<List<VendorProductSummaryDto>> GetProductsByVendorId(string vendorId, bool isWithAuthentication = true, string productName = null)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"/api/v1/Vendors/{vendorId}/Products"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			if (productName != null)
				query.Add(nameof(productName), productName);

			builder.Query = query.ToString();
			return InvokeGetApiCall<List<VendorProductSummaryDto>>(new Uri(builder.ToString()));			
		}

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

		private static ResponsePackage<T> InvokePutApiCall<T>(Uri putUri, object requestObject) where T : class
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			var putResponse = HttpClient.PutAsync(putUri, bodyContent).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(putResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}

		private static void InvokeDeleteApiCall(Uri deleteUri)
		{
			HttpClient.DeleteAsync(deleteUri).Result.EnsureSuccessStatusCode();
		}

		#endregion
	}
}