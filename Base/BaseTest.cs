using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.IO;



namespace OrangeHRMHybridAutomationFramework.Base
{
    public class BaseTest
    {
        public IWebDriver driver;

        protected ExtentTest test;
        private ExtentReports extent;

        [SetUp]

        public void Setup()
        {
            driver.Manage().Window.Maximize();
            // Implicit wait to find elements that take a second to load
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com");
        }
        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                // 1. Generate unique name with Timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";

                // 2. Capture and get the full path
                string screenShotPath = CaptureScreenshot(screenshotName);

                // 3. Log to Extent Report
                test.Log(Status.Fail, "Test Failed: " + errorMessage);
                test.AddScreenCaptureFromPath(screenShotPath);
            }
            else if (status == TestStatus.Passed)
            {
                test.Log(Status.Pass, "Test Passed.");
            }

            driver.Quit();
        }

        public string CaptureScreenshot(string fileName)
        {
            // Path: ProjectRoot/Reports/Screenshots/
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
            string folder = Path.Combine(projectPath, "Reports", "Screenshots");

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName + ".png");

            // Capture using Selenium ITakesScreenshot
            var ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot();
            screenshot.SaveAsFile(fullPath); // Selenium 4 style

            return fullPath;
        }

    } 
}