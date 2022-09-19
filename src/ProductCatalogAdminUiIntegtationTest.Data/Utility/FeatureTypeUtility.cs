using ProductCatalogAdminUiIntegrationTest.Data.Dto.V1;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductCatalogAdminUiIntegrationTest.Data.Utility
{
	public static class FeatureTypeUtility
	{
		public const string CoreName = nameof(FeatureType.Core);
		public const string CommonName = nameof(FeatureType.Common);
		public const string OptionalName = nameof(FeatureType.Optional);
		public const string DifferentiatorName = nameof(FeatureType.Differentiator);
		public const string UnverifiedName = nameof(FeatureType.Unverified);

		private static readonly IReadOnlyCollection<FeatureTypeDto> FeatureTypeDtos = new List<FeatureTypeDto>
		{
			new FeatureTypeDto { FeatureTypeId = FeatureType.Core, Name = CoreName },
			new FeatureTypeDto { FeatureTypeId = FeatureType.Common, Name = CommonName },
			new FeatureTypeDto { FeatureTypeId = FeatureType.Optional, Name = OptionalName },
			new FeatureTypeDto { FeatureTypeId = FeatureType.Differentiator, Name = DifferentiatorName },
			new FeatureTypeDto { FeatureTypeId = FeatureType.Unverified, Name = UnverifiedName }
		};

		public static string GetNameById(int featureTypeId)
		{
			return Enum.GetName(typeof(FeatureType), featureTypeId);
		}

		public static List<FeatureTypeDto> GetFeatureTypeList()
		{
			return FeatureTypeDtos.ToList();
		}

		public static string GetValidInputDisplayString()
		{
			var displayString = new StringBuilder();
			foreach (var featureType in GetFeatureTypeList().OrderBy(f => f.FeatureTypeId))
			{
				displayString.Append($"{(int)featureType.FeatureTypeId} = {featureType.Name}. ");
			}
			return displayString.ToString().Trim();
		}
	}
}
