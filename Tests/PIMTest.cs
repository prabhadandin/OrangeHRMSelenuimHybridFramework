using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities; 

namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]  // Methods inside this class run sequentially
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
            test.Value.Log(Status.Info, $"Adding employee: {firstName} {middleName} {lastName}");
            pim.NavigateToPIM();
            // capture emplployee id from ui
            string employeeId = pim.AddEmployee(firstName, middleName, lastName);
            test.Value.Log(Status.Pass, $"Employee added with ID: {employeeId}");
            string addImg = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;
            Assert.That(employeeId, Is.Not.Null);
            test.Value.Pass("Employee Added", MediaEntityBuilder.CreateScreenCaptureFromBase64String(addImg).Build());
            //Search employee id
            test.Value.Log(Status.Info, $"Initiating search for ID: {employeeId}");
            bool isFound = pim.SearchEmployeeById(employeeId,test.Value);
            string searchImg = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;
            //string searchScreenshot = CaptureScreenshot($"Search_{employeeId}");
            if (isFound)
            {
                test.Value.Pass($"Search successful for ID: {employeeId}",
                MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchImg).Build());
            }
            else
            {
                test.Value.Fail($"Search failed for ID: {employeeId}",
              MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchImg).Build());
            }
            Assert.That(isFound, Is.True, $"Search validation failed for Employee ID: {employeeId}");
        }

    }
}



