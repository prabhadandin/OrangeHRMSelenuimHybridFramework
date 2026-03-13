using NUnit.Framework;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities; 
using AventStack.ExtentReports;

namespace OrangeHRMHybridAutomationFramework.Tests
{
        [TestFixture]
        public class PIMTest : BaseTest
        {
        [SetUp]
        public void LoginPageSetup()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Logging in with Admin credentials via SetUp.");
            login.Login("Admin", "admin123");
        }

        [Test]
        [TestCaseSource(typeof(ExcelManager), nameof(ExcelManager.GetUserData), new object[] { "EmployeeData" })]
        public void AddEmployeeFromExcelTest(string firstName,string middleName, string lastName,string employeeId)
        {
            PIMPage pim = new PIMPage(driver);   
            test.Log(Status.Info, $"Adding employee from Excel: {firstName} {middleName} {lastName} {employeeId}");
            //  Navigate to PIM page and Add Employee using Excel data
            pim.NavigateToPIM();
            pim.AddEmployee(firstName, middleName, lastName, employeeId);
            string successMsg = pim.GetSuccessMessage();
            // Accept English 'Successfully Saved' OR Chinese '成功' (Success)
            bool isSuccess = successMsg.Contains("Successfully Saved") || successMsg.Contains("成功");
            Assert.That(isSuccess, Is.True, $"Failed to add employee. Actual message: '{successMsg}'");
            test.Log(Status.Pass, $"Employee '{firstName} {middleName} {lastName} {employeeId}' successfully added from Excel sheet.");
            }
        }
    }



