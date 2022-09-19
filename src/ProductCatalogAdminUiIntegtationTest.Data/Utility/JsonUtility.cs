using Newtonsoft.Json;

namespace ProductCatalogAdminUiIntegrationTest.Data.Utility
{
	public static class JsonUtility
	{
		public static string ToJson(this object value)
		{
			var settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
		}

		public static string ToSingleLineJson(this object value)
		{
			var json = ToJson(value);
			return json.Replace("\r", "").Replace("\n", "");
		}
	}
}
