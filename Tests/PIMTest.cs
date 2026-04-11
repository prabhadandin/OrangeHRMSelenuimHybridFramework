using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    public class PIMTest : BaseTest
    {
        [SetUp]
        public void LoginPageSetup()
        {
            LoginPage login = new LoginPage(driver.Value);
            test.Value.Log(Status.Info, "Logging in with Admin credentials");
            login.Login("Admin", "admin123");
        }

        [Test]
        [TestCaseSource(typeof(ExcelManager),nameof(ExcelManager.GetUserData),new object[] { "EmployeeData" })]
        public void AddEmployeeFromExcelTest(string firstName,string middleName,string lastName)
        {
            PIMPage pim = new PIMPage(driver.Value);
            test.Value.Log(Status.Info, $"Adding Employee:{firstName}{middleName}{lastName}");
            pim.NavigateToPIM();
            string empId = pim.AddEmployee(firstName,middleName,lastName);
            test.Value.Log(Status.Pass,$"Employee created with ID: {empId}");
            bool isFound = pim.SearchEmployeeById(empId);
            test.Value.Log(Status.Info,$"Searching Employee ID: {empId}");
            Assert.That(isFound, Is.True,$"Employee {empId} not found");
            if (isFound)
                test.Value.Pass($"Employee {empId} verified successfully");
            else
                test.Value.Fail($"Employee {empId} not found in system");
        }
    }
}