using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class BrandedResearchAdminApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private readonly Uri _baseUri;
		private static HttpClient HttpClient { get; set; }

		public BrandedResearchAdminApiService()
		{
			var adminApiBaseUrl = Configuration.GetValue<string>("BrandedResearchAdminApiBaseUrl");
			_baseUri = new Uri(adminApiBaseUrl);
		}

		public static void InitializeHttpClient(string bearerToken)
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}

		#region Reports

		public ReportDto GetReportById(int reportId)
		{
			return InvokeGetApiCall<ReportDto>(new Uri(_baseUri, $"/api/v1/reports/{reportId}")).Result;
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
			var postResponse = HttpClient.PostAsync(postUri, bodyContent).Result;
			var responseContent = postResponse.Content.ReadAsStringAsync().Result;
			postResponse.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<T>>(responseContent);
			return responsePackage;
		}

		private static void InvokePutApiCall(Uri putUri, object requestObject)
		{
			var json = JsonConvert.SerializeObject(requestObject);
			var bodyContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpClient.PutAsync(putUri, bodyContent).Result.EnsureSuccessStatusCode();
		}

		private static void InvokeDeleteApiCall(Uri deleteUri)
		{
			HttpClient.DeleteAsync(deleteUri).Result.EnsureSuccessStatusCode();
		}

		#endregion
	}
}
