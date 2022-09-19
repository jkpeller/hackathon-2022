using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;

namespace ProductCatalogAdminUiIntegrationTest.Data.Request
{
	public class ProductPricingUpdateRequest
	{
        public VerifiedType? CreditCardVerifiedTypeId { get; set; }
        public int? CurrencyId { get; set; }
        public bool? FreeTrial { get; set; }
        public bool? FreeVersion { get; set; }
        public PriceRangeType? PriceRangeTypeId { get; set; }
        public string VendorPricingUrl { get; set; }
        [StringLength(ProductPricingUtility.MaximumPricingDescriptionLength)]
        public string Description { get; set; }
        public List<DiscountType> DiscountTypeIds { get; set; }
        public List<ProductPricingPlanUpdateRequest> Plans { get; set; }
    }
}
