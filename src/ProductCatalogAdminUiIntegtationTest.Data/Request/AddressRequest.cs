namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class AddressRequest
	{
		public int? CountryId { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public int? StateProvinceId { get; set; }
		public string StateProvinceRegionName { get; set; }
		public string ZipPostalCode { get; set; }
	}
}
