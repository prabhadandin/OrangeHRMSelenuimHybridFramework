using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
<<<<<<< HEAD
using OrangeHRMHybridAutomationFramework.Utilities;
=======
using OrangeHRMHybridAutomationFramework.Utilities; 
>>>>>>> 4a218f08b619aa97a3ff8357d870de70ddbf9165

namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
<<<<<<< HEAD
    public class PIMTest : BaseTest
    {
=======
    [Parallelizable(ParallelScope.None)]  // Methods inside this class run sequentially
    public class PIMTest : BaseTest
        {
>>>>>>> 4a218f08b619aa97a3ff8357d870de70ddbf9165
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
<<<<<<< HEAD
            PIMPage pim = new PIMPage(driver);

            test.Log(Status.Info, $"Adding employee: {firstName} {middleName} {lastName}");

            // Navigate to PIM page and Add Employee using Excel data
            pim.NavigateToPIM();
=======
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
>>>>>>> 4a218f08b619aa97a3ff8357d870de70ddbf9165

            // capture employee id from UI
            string employeeId = pim.AddEmployee(firstName, middleName, lastName);

            test.Log(Status.Pass, $"Employee added with ID: {employeeId}");

            string addImg = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;

            Assert.That(employeeId, Is.Not.Null.And.Not.Empty);

            test.Pass("Employee Added",
                MediaEntityBuilder.CreateScreenCaptureFromBase64String(addImg).Build());

            // Search employee id
            test.Log(Status.Info, $"Initiating search for ID: {employeeId}");

            bool isFound = pim.SearchEmployeeById(employeeId, test);

            string searchImg = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;

            if (isFound)
            {
                test.Pass($"Search successful for ID: {employeeId}",
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchImg).Build());
            }
            else
            {
                test.Fail($"Search failed for ID: {employeeId}",
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(searchImg).Build());
            }

            Assert.That(isFound, Is.True, $"Search validation failed for Employee ID: {employeeId}");
        }
    }
}