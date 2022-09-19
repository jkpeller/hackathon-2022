using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ProductCatalogAdminUiIntegrationTest.Core.SeleniumCore
{
	/// <summary>
	/// The methods in this class call methods from the IWebElement class within Selenium to interact with UI elements on the page.
	/// </summary>
	public class Control
	{
		#region Constructor and Variables

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private static readonly TimeSpan DefaultTimeout = BrowserUtility.DefaultTimeout;
		private readonly string _selector;
		private string _dataQa;
		private const string AriaSelected = "aria-selected";
		private const string HrefValue = "href";
		private const string SrcValue = "src";
		//finds all IDs wrapped in parentheses, example the "(5671)" from "My Product Name (5671)"
		private static readonly Regex IdRegex = new Regex(@".+?(?=\((.*)\))");

		private IWebElement Element => FindElement();

		public Control(string selector)
		{
			_selector = selector;
		}

		#endregion

		#region Text Interactions

		/// <summary>
		/// Types the given text into the element. Supports special characters using the Keys class.
		/// </summary>
		/// <param name="clear">Determines whether or not to clear any text currently in the element before typing.</param>
		/// <param name="text">The text or keys to be typed into the element.</param>
		/// <param name="sendEscape">Determines whether or not to send the escape key after typing into the element.</param>
		/// <param name="hoverOver">Determines whether or not to hover over the element to find it prior to interaction.</param>
		public void SendKeys(string text = null, bool clear = false, bool sendEscape = true, bool hoverOver = true)
		{
			try
			{
				if(hoverOver) HoverOver();
				if (clear)
				{
					Element.Clear(); 
					Element.SendKeys("a" + Keys.Backspace);		
				}
				if (text != null)
				{
					Element.SendKeys(text);
					Logger.Info($"Typed '{text}' into {GetDataQaAttributeSelector()}.");
				}
				Thread.Sleep(100);
				if (!sendEscape)
				{
					Thread.Sleep(100);
					return;
				}
				Element.SendKeys(Keys.Escape);
				Thread.Sleep(100);
			}
			catch (Exception e)
			{
				HandleException(e, $"Could not type '{text}' into {GetDataQaAttributeSelector()}");
			}
		}

		/// <summary>
		/// Gets the inner text value from the element.
		/// </summary>
		/// <returns>The inner text from the element, even if that text is contained within a child element of the one being called upon.</returns>
		public string GetText(bool hoverOver = true)
		{
			try
			{
				if (hoverOver) HoverOver();
				var text = Element.Text;
				Logger.Info($"Retrieved text from {GetDataQaAttributeSelector()}: {text} {Element}");
				return text;
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to retrieve text from {GetDataQaAttributeSelector()}.");
				return null;
			}
		}

		/// <summary>
		/// Gets the inner text value from the element.
		/// </summary>
		/// <returns>The inner text from the element, even if that text is contained within a child element of the one being called upon.</returns>
		public string GetTextValue(bool hoverOver = true)
		{
			try
			{
				if (hoverOver) HoverOver();
				var text = Element.GetAttribute("value");
				Logger.Info($"Retrieved text from {GetDataQaAttributeSelector()}: {text} {Element}");
				return text;
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to retrieve text from {GetDataQaAttributeSelector()}.");
				return null;
			}
		}	
		/// <summary>
		/// Gets the inner text value from the element, using Regex to get rid of the Id in parentheses.
		/// </summary>
		/// <returns>The inner text from the element, even if that text is contained within a child element of the one being called upon.</returns>
		public string GetTextWithoutTrailingId()
		{
			try
			{
				var text = IdRegex.Match(Element.Text).ToString().Trim();
				Logger.Info($"Retrieved text from {GetDataQaAttributeSelector()}: {text} {Element}");
				return text;
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to retrieve text from {GetDataQaAttributeSelector()}.");
				return null;
			}
		}

		#endregion

		#region Click Methods

		/// <summary>
		/// Attempts to perform a standard left button mouse click on the element.
		/// </summary>
		public void Click(bool hoverOver = true)
		{
			try
			{
				if (hoverOver)
				{
					HoverOver();
					var action = new Actions(BrowserUtility.WebDriver);
					action.MoveToElement(Element).Click().Build().Perform();
				}
				else
				{
					Element.Click();
				}
				Logger.Info($"Clicked on {GetDataQaAttributeSelector()}");
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to click on {GetDataQaAttributeSelector()}");
			}
		}

		/// <summary>
		/// Performs a left-click on the element, then waits for the overlay container to disappear.
		/// </summary>
		public void ClickAndWaitForPageToLoad()
		{
			try
			{
				Click();
				BrowserUtility.WaitForPageToLoad();
				Thread.Sleep(3000);
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to click on {GetDataQaAttributeSelector()}");
			}
		}

		/// <summary>
		/// Performs a left-click on the element, then waits for the overlay container to disappear.
		/// </summary>
		public void ClickAndWaitPageToLoadAndOverlayToDisappear()
		{
			try
			{
				Click();
				BrowserUtility.WaitForPageToLoad();
				BrowserUtility.WaitForOverlayToDisappear();
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to click on {GetDataQaAttributeSelector()}");
			}
		}

        /// <summary>
        /// Performs a left-click on the element, then waits for the overlay container to disappear.
        /// </summary>
        public void ClickAndDragAndDrop(Control targetElment)
        {
            try
            {
                var action = new Actions(BrowserUtility.WebDriver);
                action.DragAndDrop(Element, targetElment.Element).Build().Perform();
                //BrowserUtility.WaitForPageToLoad();
                //BrowserUtility.WaitForOverlayToDisappear();
            }
            catch (Exception e)
            {
                HandleException(e, $"Failed to drag and drop on {GetDataQaAttributeSelector()}");
            }
        }

		#endregion

		#region Checkboxes

		/// <summary>
		/// Checks whether or not the box is currently checked, then checks it if it is not already and logs the result. Should only be called on check box elements ('mat-option' tags in Angular).
		/// </summary>
		public void Check()
		{
			try
			{
				var selected = Element.GetAttribute(AriaSelected);
				if (selected.Equals(true.ToString().ToLower()))
				{
					Logger.Info($"{GetDataQaAttributeSelector()} was already checked.");
				}
				else
				{
					Click(false);
					Logger.Info($"Checked {GetDataQaAttributeSelector()}.");
				}
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to check {GetDataQaAttributeSelector()}.");
			}
		}

		/// <summary>
		/// Checks whether or not the box is currently checked, then un-checks it if it is not already and logs the result. Should only be called on check box elements ('mat-option' tags in Angular).
		/// </summary>
		public void Uncheck()
		{
			try
			{
				var selected = Element.GetAttribute(AriaSelected);
				if (selected.Equals(false.ToString().ToLower()))
				{
					Logger.Info($"{GetDataQaAttributeSelector()} was already unchecked.");
				}
				else
				{
					Click();
					Logger.Info($"Unchecked {GetDataQaAttributeSelector()}.");
				}
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to uncheck {GetDataQaAttributeSelector()}.");
			}
		}

		/// <summary>
		/// Returns a boolean value of true if the checkbox is checked false otherwise.
		/// </summary>
		public bool IsSelected()
		{
			return Element.Selected;
		}

		#endregion

		#region Enabled/Disabled

		public bool IsEnabled()
		{
			var element = BrowserUtility.WebDriver.FindElement(By.CssSelector(_selector));
			var disabled = element.GetAttribute("ng-reflect-disabled") ?? element.GetAttribute("disabled");

			if (disabled != null && disabled.Equals(bool.TrueString, StringComparison.InvariantCultureIgnoreCase))
			{
				Logger.Info($"{GetDataQaAttributeSelector()} was disabled.");
				return false;
			}
			
			Logger.Info($"{GetDataQaAttributeSelector()} was enabled.");
			return true;
		}

		public bool IsDisabled()
		{
			// added this line
			var element = BrowserUtility.WebDriver.FindElement(By.CssSelector(_selector));
			try
			{
				bool isDisabled;
				// Element.TagName
				switch (element.TagName)
				{
					case "mat-checkbox":
						isDisabled = element.GetAttribute("class").ToLower().Contains("mat-checkbox-disabled");
						break;
					case "mat-select":
						isDisabled = element.GetAttribute("class").ToLower().Contains("mat-select-disabled");
						break;
					default:
						isDisabled = element.GetAttribute("ng-reflect-disabled").Equals(bool.TrueString, StringComparison.InvariantCultureIgnoreCase);
						break;
				}
				
				if (isDisabled)
				{
					Logger.Info($"{GetDataQaAttributeSelector()} was disabled.");
					return true;
				}
			}
			catch (Exception e)
			{
				HandleException(e, $"Failed to check if {GetDataQaAttributeSelector()} was disabled.");
			}

			Logger.Info($"{GetDataQaAttributeSelector()} was enabled.");
			return false;
		}

		public bool IsDisplayed(bool hoverOver = true)
		{
			try
			{
				if (hoverOver)
					HoverOver();

				if (Element == null)
					return false;

				Logger.Info($"{GetDataQaAttributeSelector()} was displayed.");
				return Element.Displayed;
			}
			catch (Exception e)
			{
				Logger.Info($"{GetDataQaAttributeSelector()} was not diplayed. Exception: {e}");
				return false;
			}
		}

		public string IsAttributePresent(string attribute)
		{
			try
			{
				var value = Element.GetAttribute(attribute);
				Logger.Info($"Attribute was present. Attribute: {attribute}");
				return value;
			}
			catch (Exception)
			{
				Logger.Info($"Attribute was not present. Attribute: {attribute}");
				return null;
			}
		}

		public bool ExistsInPage()
		{
			return FindElement() != null;
		}

		#endregion

		#region Hover over

		public void HoverOver()
		{
			var element = BrowserUtility.WebDriver.FindElement(By.CssSelector(_selector));
			var action = new Actions(BrowserUtility.WebDriver);
			action.MoveToElement(element).Perform();
		}

		#endregion

		#region Links

		public string GetHref()
		{
			HoverOver();
			var value = Element.GetAttribute(HrefValue);
			return value;
		}

		#endregion

		#region Images

		public string GetImageSrc()
		{
			HoverOver();
			var value = Element.GetAttribute(SrcValue);
			return value;
		}
		
		public string GetImageSrcHeightQueryParamValue()
		{
			var srcParams = GetImageSrcQueryParams();
			return srcParams.FirstOrDefault(p =>
				string.Equals(p.Name, "h", StringComparison.InvariantCultureIgnoreCase))
				.Value;
		}

		public string GetImageSrcWidthQueryParamValue()
		{
			var srcParams = GetImageSrcQueryParams();
			return srcParams.FirstOrDefault(p =>
					string.Equals(p.Name, "w", StringComparison.InvariantCultureIgnoreCase))
				.Value;
		}

		private List<(string Name, string Value)> GetImageSrcQueryParams()
		{
			var src = GetImageSrc();
			const char queryParamsStartChar = '?';
			return src.Contains(queryParamsStartChar.ToString()) ?
				src.Substring(src.IndexOf(queryParamsStartChar) + 1)
					.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
					.Select(s =>
					{
						var equalIndex = s.IndexOf('=');
						return (s.Substring(0, equalIndex), s.Substring(equalIndex + 1, s.Length - equalIndex - 1));
					})
					.ToList()
				: new List<(string Name, string Value)>();
		}

		#endregion

		#region CSS

		public string GetCssValue(string propertyName)
		{
			HoverOver();
			var value = Element.GetCssValue(propertyName);
			return value;
		}

		#endregion

		#region Class

		public bool HasClass(string className)
		{
			HoverOver();
			var value = Element.GetAttribute("class");
			return value.Contains(className);
		}

        #endregion

        #region Elements Count

        public int GetChildElementCount()
        {
			var value = Element.GetDomProperty("childElementCount");
			var _ = int.TryParse(value, out var childElementCount);
			return childElementCount;
		}

        public int GetElementCount()
        {
	        return BrowserUtility.WebDriver.FindElements(By.CssSelector(_selector)).Count;
        }

        #endregion

        /// <summary>
        /// Method used to find the element once it is confirmed to be on the screen. It is called by all other methods in this class.
        /// </summary>
        private IWebElement FindElement()
		{
			try
			{
				var wait = new WebDriverWait(BrowserUtility.WebDriver, DefaultTimeout); //keeps the test from failing unless the action cannot be completed within the configured timeout value in seconds
				wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(_selector))); //waits for the element to be click able before interacting with it
				var element = BrowserUtility.WebDriver.FindElement(By.CssSelector(_selector));
				_dataQa = element.GetAttribute(ControlUtility.DataQa);
				return element;
			}
			catch (Exception e)
			{
				Logger.Info($"{GetDataQaAttributeSelector()} was not found on the page. Exception: {e}");
				return null;
			}
		}

		private static void HandleException(Exception e, string message)
		{
			Logger.Error(message);
			Logger.Trace(e);
		}

		private string GetDataQaAttributeSelector()
		{
			return $"[{ControlUtility.DataQa}='{_dataQa}']";
		}
	}
}