using System;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System.Collections.Generic;

namespace ProductCatalogAdminUiIntegrationTest.Data.Dto.V1
{
	public class ProductPricingDto
	{
        public int PricingId { get; set; }
        public bool? FreeVersion { get; set; }
        public bool? FreeTrial { get; set; }
        public bool? IsPricingVisible { get; set; }
        public PriceRangeType? PriceRangeTypeId { get; set; }
        public string VendorPricingUrl { get; set; }
        public string ModifiedByUserFirstName { get; set; }
        public string ModifiedByUserLastName { get; set; }
        public DateTimeOffset ModifiedOnUtc { get; set; }
        public string Description { get; set; }
        public List<DiscountTypeDto> DiscountTypes { get; set; }
        public List<CurrencyDto> Currencies { get; set; }
        public List<VerifiedDto> CreditCardVerifieds { get; set; }
        public List<ProductPricingPlanDto> Plans { get; set; }
	}
}
