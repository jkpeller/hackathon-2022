using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalogAdminUiIntegrationTest.Core.Logging;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
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
	public class CategoryAdminApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		private static LogService _logService;
		private readonly Uri _baseUri;
		private static HttpClient HttpClient { get; set; }

		public CategoryAdminApiService(LogService logService)
		{
			_logService = logService;
			var adminApiBaseUrl = Configuration.GetValue<string>("CategoryAdminApiBaseUrl");
			_baseUri = new Uri(adminApiBaseUrl);
		}

		public static void InitializeHttpClient(string bearerToken)
		{
			HttpClient = new HttpClient();
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
		}

		#region Categories

		public ResponsePackage<List<CategoryTagDto>> GetCategoryTagsById(string categoryId)
		{
			return InvokeGetApiCall<List<CategoryTagDto>>(new Uri(_baseUri, $"api/v1/Categories/{categoryId}/Tags"));
		}

		public ResponsePackage<CategoryAdminDto> PostCategory(CategoryInsertRequest request)
		{
			return InvokePostApiCall<CategoryAdminDto>(new Uri(_baseUri, "api/v1/Categories"), request);
		}

		public void DeleteCategory(string categoryId)
		{
			InvokeDeleteApiCall(new Uri(_baseUri, $"api/v1/Categories/{categoryId}"));
		}

		public ResponsePackage<List<CategoryAdminDto>> GetCategoriesByPartialName(string partialName)
		{
			return InvokeGetApiCall<List<CategoryAdminDto>>(new Uri(_baseUri, $"api/v1/Autocomplete/Categories/{partialName}"));
		}

		public ResponsePackage<CategoryAdminDto> GetCategoryById(string categoryId, bool? includeSiteCategories = false)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Categories/{categoryId}"));
			var query = HttpUtility.ParseQueryString(builder.Query);

			if (includeSiteCategories != null)
			{
				query[nameof(includeSiteCategories)] = includeSiteCategories.ToString().ToLower();
			}

			builder.Query = query.ToString();
			return InvokeGetApiCall<CategoryAdminDto>(new Uri(builder.ToString()));
		}

		public void PutCategory(string categoryId, CategoryUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/Categories/{categoryId}"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

		public void PutCategoryPublishStatusById(string categoryId, CategoryPublishStatusUpdateRequest request)
		{
			var builder = new UriBuilder(new Uri(_baseUri, $"api/v1/{categoryId}/PublishStatus"));
			InvokePutApiCall(new Uri(builder.ToString()), request);
		}

        #endregion

		#region Features

		public ResponsePackage<FeatureDto> PostFeature(FeatureInsertRequest request)
		{
			return InvokePostApiCall<FeatureDto>(new Uri(_baseUri, "api/v1/Features"), request);
		}

		public ResponsePackage<List<CategoryFeatureDto>> GetCategoryFeaturesById(string categoryId)
		{
			return InvokeGetApiCall<List<CategoryFeatureDto>>(new Uri(_baseUri, $"api/v1/Categories/{categoryId}/Features"));
		}

		#endregion

		#region CategoryFeatures

		public ResponsePackage<CategoryFeatureDto> PostCategoryFeature(string categoryId, CategoryFeatureInsertRequest request)
		{
			return InvokePostApiCall<CategoryFeatureDto>(new Uri(_baseUri, $"api/v1/Categories/{categoryId}/Features"), request);
		}

        public void DeleteCategoryFeature(string categoryFeatureId)
        {
            InvokeDeleteApiCall(new Uri(_baseUri, $"api/v1/CategoryFeatures/{categoryFeatureId}"));
        }

		#endregion

		#region CategoryTags
        public ResponsePackage<CategoryTagDto> PostCategoryTag(string categoryId, CategoryTagInsertRequest request)
        {
            return InvokePostApiCall<CategoryTagDto>(new Uri(_baseUri, $"api/v1/Categories/{categoryId}/Tags"), request);
        }

        public void PutCategoryTag(string categoryId, CategoryTagUpdateRequest request)
        {
            InvokePutApiCall(new Uri(_baseUri, $"api/v1/Categories/{categoryId}/Tags"), request);
        }

		#endregion

        #region Tags
        public ResponsePackage<TagDto> PostTag(TagInsertRequest request)
        {
            return InvokePostApiCall<TagDto>(new Uri(_baseUri, $"api/v1/Tags"), request);
        }
        public void DeleteTag(string tagId)
        {
            InvokeDeleteApiCall(new Uri(_baseUri, $"api/v1/Tags/{tagId}"));
        }

		#endregion

		#region Site categories

		public void PostMapSiteCategories(SiteCategoryToCategoryMapSaveRequest request)
		{
			InvokePostApiCall<CategoryAdminDto>(new Uri(_baseUri, "api/v1/Map/SiteCategories"), request);
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
			_logService.Log($"HttpStatusCode: {(int)postResponse.StatusCode} - {postResponse.StatusCode}, Content: {responseContent}");
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