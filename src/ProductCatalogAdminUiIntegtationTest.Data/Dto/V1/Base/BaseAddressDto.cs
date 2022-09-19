namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1.Base
{
	public class BaseAddressDto
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public int? StateProvinceId { get; set; }
		public string StateProvinceRegionName { get; set; }
		public string ZipPostalCode { get; set; }
	}
}
