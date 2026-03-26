using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class PIMTest : BaseTest
    {
        [SetUp]
        public void LoginPageSetup()
        {
            LoginPage login = new LoginPage(driver.Value);
            test.Value.Log(Status.Info, "Logging in with Admin credentials via SetUp.");
            login.Login("Admin", "admin123");
        }

        [Test]
        [TestCaseSource(typeof(ExcelManager), nameof(ExcelManager.GetUserData), new object[] { "EmployeeData" })]
        public void AddEmployeeFromExcelTest(string firstName, string middleName, string lastName)
        {
            PIMPage pim = new PIMPage(driver.Value);

            test.Value.Log(Status.Info, $"Adding Employee: {firstName} {middleName} {lastName}");
            pim.NavigateToPIM();

            string employeeId = pim.AddEmployee(firstName, middleName, lastName);
            test.Value.Log(Status.Pass, $"Employee added with ID: {employeeId}");

            string addScreenshot = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;
            test.Value.Pass("Employee Added", MediaEntityBuilder.CreateScreenCaptureFromBase64String(addScreenshot).Build());

            test.Value.Log(Status.Info, $"Searching for Employee ID: {employeeId}");
            bool isFound = pim.SearchEmployeeById(employeeId, test.Value);

            string searchScreenshot = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;
            if (isFound)
                test.Value.Pass($"Search successful for ID: {employeeId}", MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchScreenshot).Build());
            else
                test.Value.Fail($"Search failed for ID: {employeeId}", MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchScreenshot).Build());

            Assert.That(isFound, Is.True, $"Search validation failed for Employee ID: {employeeId}");
        }
    }
}