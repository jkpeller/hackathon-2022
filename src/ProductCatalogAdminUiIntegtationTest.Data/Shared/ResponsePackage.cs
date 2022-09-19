using System.Collections.Generic;
using System.Net;

namespace ProductCatalogAdminUiIntegrationTest.Data.Shared
{
	public class ResponsePackage<T> where T : class
	{
		/// <summary>
		/// Human/developer readable messages
		/// </summary>
		public List<string> Messages { get; set; }

		/// <summary>
		/// Human/developer readable error information
		/// </summary>
		public List<string> Errors { get; set; }

		/// <summary>
		/// Details of paged results
		/// </summary>
		public ResponsePage Page { get; set; }

		/// <summary>
		/// Data to send back
		/// </summary>
		public T Result { get; set; }

		public HttpStatusCode HttpStatusCode { get; set; }
	}
}
