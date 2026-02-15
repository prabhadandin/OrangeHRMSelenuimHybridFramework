using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;
using NUnit.Framework;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public class ExcelManager
    {
        public static IEnumerable<TestCaseData> GetUserData(string sheetName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "TestData", "EmployeeData.xlsx");

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });

                    DataTable table = result.Tables[sheetName];
                    if (table != null)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            string firstName = row[0]?.ToString() ?? "";
                            string lastName = row[1]?.ToString() ?? "";

                            yield return new TestCaseData(firstName, lastName);
                        }
                    }
                }
            }
        }
    }
}
    
