using ProductCatalogAdminUiIntegrationTest.Data.Service.Base;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;

namespace ProductCatalogAdminUiIntegrationTest.Data.Service
{
	public class FileManagementApiService : BaseIntegrationApiService
	{
		public FileManagementApiService()
			: base("FileManagementApiBaseUrl")
		{
		}

		public ResponsePackage<object> DeleteFileById(int fileId)
		{
			return InvokeDeleteApiCall<object>($"/api/v1/Files/{fileId}");
		}
	}
}