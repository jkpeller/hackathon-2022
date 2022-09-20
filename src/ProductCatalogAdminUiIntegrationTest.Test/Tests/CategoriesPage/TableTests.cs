using NUnit.Framework;
using ProductCatalogAdminUiIntegrationTest.Data.Request;
using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using ProductCatalogAdminUiIntegrationTest.Data.Utility;
using ProductCatalogAdminUiIntegrationTest.Model.Shared;
using ProductCatalogAdminUiIntegrationTest.Test.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using ProductCatalogAdminUiIntegrationTest.Core.Utility;
using System.Data.OleDb;

namespace ProductCatalogAdminUiIntegrationTest.Test.Tests.CategoriesPage
{
	[TestFixture]
	public class TableTests : BaseTest
	{
		private Model.Pages.CategoriesPage _page;

		public TableTests() : base(nameof(TableTests))
		{
		}

		[SetUp]
		public void SetUp()
		{
			_page = new Model.Pages.CategoriesPage();
        }

		[Test]
		[Category("Readonly")]
		public void InsertURLsFromSpreadsheetToSpark()
		{
			ExecuteTimedTest(() =>
			{
				OpenPage(_page);

                var filePath = "C:\\Users\\jpelleri\\Downloads\\Hackathon\\Joshua_Hackathon_poc.xlsx";
                string conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "'; Extended Properties=Excel 12.0;";

                conStr = string.Format(conStr, filePath, "yes");

                DataTable dt = new DataTable();
                using (OleDbConnection connExcel = new OleDbConnection(conStr))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;
                            connExcel.Open();
                            cmdExcel.CommandText = $"SELECT * From [Sheet1$A1:B]";
                            odaExcel.SelectCommand = cmdExcel;
                            try
                            {
                                odaExcel.Fill(dt);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                            connExcel.Close();
                        }
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    var reviewerId = row["ReviewerId"].ToString();
                    if (!string.IsNullOrEmpty(reviewerId))
                    {
                        BrowserUtility.NavigateToPage("?srcCompareReview=Capterra___3318162&reviewerId=" + reviewerId);
                    }
                    var urlString = row["URL"].ToString();
					Console.WriteLine(urlString);

                    //type the filter text into the category name filter box and click the apply filters button
                    _page.InputReviewerUrl.SendKeys(urlString);
                    _page.ButtonReviewerUrlSave.Click();
                }
            });
		}
    }
}
