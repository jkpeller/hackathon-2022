using NLog;
using NUnit.Framework;
using System;

namespace ProductCatalogAdminUiIntegrationTest.Core.Logging
{
	public class LogService
	{
		private readonly ILogger _logger;

		public LogService(ILogger logger)
		{
			_logger = logger;
		}

		public void Log(string message)
		{
			_logger.Debug(message);
			TestContext.WriteLine(message);
		}

		public void Log(Exception ex)
		{
			_logger.Error(ex);
			TestContext.WriteLine(ex);
		}
	}
}