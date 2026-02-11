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
        // runs before every [TestCase]
        [SetUp]
        public void LoginPageSetup()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Logging in with Admin credentials via SetUp.");
            login.LoginWithValidCredentials("Admin", "admin123");
        }

        [Test]
        [TestCaseSource(typeof(ExcelManager), nameof(ExcelManager.GetUserData), new object[] { "EmployeeData" })]
        public void AddEmployeeFromExcelTest(string firstName,string middleName, string lastName,string employeeId)
        {
            PIMPage pim = new PIMPage(driver);    
                //  Navigate to PIM and Add Employee using Excel data
            test.Log(Status.Info, $"Adding employee from Excel: {firstName} {middleName} {lastName} {employeeId}");
            pim.NavigateToPIM();
            pim.AddEmployee(firstName, middleName,lastName,employeeId);

                // Verify Success Message
            string successMsg = pim.GetSuccessMessage();
            Assert.That(successMsg, Does.Contain("Successfully Saved"), $"Failed to add employee: {firstName}");

            test.Log(Status.Pass, $"Employee '{firstName} {middleName} {lastName} {employeeId}' successfully added from Excel sheet.");
            }
        }
    }


