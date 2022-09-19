using Microsoft.Extensions.Configuration;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using System.Configuration;
using System.Net.Http.Headers;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service.Base
{
	public abstract class BaseIntegrationApiService : BaseApiService
	{
        private static readonly IConfiguration Configuration = BrowserUtility.InitConfiguration();
		protected BaseIntegrationApiService(string baseUrlAppSetting)
			: base(baseUrlAppSetting)
		{
			var jwtToken = Configuration.GetValue<string>("JwtToken");
			HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
		}
	}
}