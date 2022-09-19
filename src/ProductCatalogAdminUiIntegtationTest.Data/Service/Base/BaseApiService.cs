using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Configuration;
using System.Net.Http;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service.Base
{
	public class BaseApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private readonly Uri _baseUri;
		protected HttpClient HttpClient { get; }

		protected BaseApiService(string baseUrlAppSetting)
		{
			var baseUrl = Configuration.GetValue<string>(baseUrlAppSetting);
			_baseUri = new Uri(baseUrl);
			HttpClient = new HttpClient();
		}

		protected ResponsePackage<TResult> InvokeDeleteApiCall<TResult>(string relativeUrl)
			where TResult : class
		{
			var deleteResponse = HttpClient.DeleteAsync(new Uri(_baseUri, relativeUrl)).Result.EnsureSuccessStatusCode();
			var responsePackage = JsonConvert.DeserializeObject<ResponsePackage<TResult>>(deleteResponse.Content.ReadAsStringAsync().Result);
			return responsePackage;
		}
	}
}