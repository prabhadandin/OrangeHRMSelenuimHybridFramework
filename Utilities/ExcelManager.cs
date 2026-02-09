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
            // Path: ProjectRoot/testdata/pim.xlsx
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(basePath, "..", "..", ".."));
            string filePath = Path.Combine(projectPath, "TestData", "EmployeeData.xlsx");
           
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                // Register encoding for .xlsx files
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[sheetName];

                    // Skip header (i=1) and loop through rows
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        string firstName = table.Rows[i][0].ToString();
                        string lastName = table.Rows[i][1].ToString();

                        // Return strings needed by AddEmployeeFromExcelTest
                        yield return new TestCaseData(firstName, lastName);
                    }
                }
            }
        }

    }
}
