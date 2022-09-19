namespace ProductCatalogAdminUiIntegrationTest.Core.Utility
{
	public static class ControlUtility
	{
		public const string DataQa = "data-qa";

		public static string GetElementSelector(string elementName)
		{
			return $"[{DataQa}='{elementName}']";
		}

		public static string GetTableRow()
		{
			return "[role='row']";
		}
		
		public static string GetTableCellByRowNumberAndColumnName(int rowNumber, string columnName)
		{
			return $"[role=\'row\']:nth-of-type({rowNumber}) {GetElementSelector(columnName)}";
		}

		public static string GetTableRowByRowNumber(int rowNumber)
		{
			return $"[role=\'row\']:nth-of-type({rowNumber})";
		}

		public static string GetSiteCategoriesMappingModalSuggestionSelector(string categoryName)
		{
			return $"[{DataQa}='mat-option-mapping-dialog-global-category-name-{categoryName}']";
		}

		public static string GetSiteProductsMappingModalSuggestionSelector(string productName)
		{
			return $"[{DataQa}='mat-option-mapping-dialog-global-product-name-{productName}']";
		}

		public static string GetAutocompleteSuggestionSelector(int? position = null)
		{
			return position.HasValue ? $"[role='option']:nth-of-type({position})" : $"[role='option']";
		}
		
		public static string GetSiteCategoriesMappingTableGlobalCategoryDisplay(int rowNumber)
		{
			return $"[role='row']:nth-of-type({rowNumber}) .global-category-name";
		}

		public static string GetSiteProductsMappingTableGlobalProductDisplay(int rowNumber)
		{
			return $"[role='row']:nth-of-type({rowNumber}) .global-product-name";
		}

		public static string GetChipListItemByDisplayName(string name)
		{
			return GetElementSelector($"mat-chip-list-item-{name}");
		}

		public static string GetCategoryFromFilterByName(string name)
		{
			return GetElementSelector($"mat-option-category-name-{name.ToLower()}");
		}
	}
}