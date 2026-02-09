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
       
        [Test]
        [TestCaseSource(typeof(ExcelManager), nameof(ExcelManager.GetUserData), new object[] { "EmployeeData" })]
        public void AddEmployeeFromExcelTest(string firstName, string lastName)
        {
               //  Initialize Page Objects
                  LoginPage login = new LoginPage(driver);
                 PIMPage pim = new PIMPage(driver);

                //  Perform Login
                test.Log(Status.Info, "Logging in with Admin credentials.");
                login.LoginWithValidCredentials("Admin", "admin123");

                //  Navigate to PIM and Add Employee using Excel data
                test.Log(Status.Info, $"Adding employee from Excel: {firstName} {lastName}");
                pim.NavigateToPIM();
                pim.AddEmployee(firstName, lastName);

                // 4. Verify Success Message
                string successMsg = pim.GetSuccessMessage();
                Assert.That(successMsg, Does.Contain("Successfully Saved"), $"Failed to add employee: {firstName}");

                test.Log(Status.Pass, $"Employee '{firstName} {lastName}' successfully added from Excel sheet.");
            }
        }
    }


