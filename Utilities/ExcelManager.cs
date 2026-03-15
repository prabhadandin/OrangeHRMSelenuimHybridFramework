using ExcelDataReader;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public class ExcelManager
    {
        public static IEnumerable<TestCaseData> GetUserData(string sheetName)
        {
            List<TestCaseData> testData = new List<TestCaseData>();
            try
            {

                /* string filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName,
                  "TestData",
                  "EmployeeData.xlsx");*/
                string filePath = Path.Combine(
                     TestContext.CurrentContext.TestDirectory,
                     "TestData",
                     "EmployeeData.xlsx");
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                    });
                    DataTable table = result.Tables[sheetName];
                    if (table == null)
                    {
                        /* List all available sheets to help you debug
                        string availableSheets = string.Join(", ", result.Tables.Cast<DataTable>().Select(t => t.TableName));
                        Assert.Fail($"Excel data load failed: Sheet '{sheetName}' not found. Available sheets: [{availableSheets}]");
                        yield break;*/
                        throw new Exception($"Sheet {sheetName} not found.");
                    }
                        
                    foreach (DataRow row in table.Rows)
                    {
                        string firstName = row["FirstName"].ToString()??"" ;
                        string middleName = row["MiddleName"].ToString()??"";
                        string lastName = row["LastName"].ToString()??"";
                        string employeeId = row["EmployeeID"].ToString()??"";
                        testData.Add(new TestCaseData(firstName, middleName, lastName, employeeId));

                    }
                }
            }
            catch (Exception ex)
            {
                Assert.Fail("Excel data load failed: " + ex.Message);
            }
             foreach (var data in testData)
            {
                yield return data;
            }
        }
    }
}

