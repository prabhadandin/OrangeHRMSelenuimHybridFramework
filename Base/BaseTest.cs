using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using System;
using System.IO;
using OrangeHRMHybridAutomationFramework.Utilities;
using OpenQA.Selenium.Chrome;

namespace OrangeHRMHybridAutomationFramework.Base
{
    public class BaseTest
    {
        public IWebDriver driver;

        protected ExtentTest test;
        private ExtentReports extent;
        
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Initialize the report once for the entire test run
            extent = ExtentManager.GetInstance();
        }


        [SetUp]

        public void Setup()
        {
            //Create the test entry in the report before the test starts
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            //initialize your driver
            driver = new ChromeDriver();
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
                //  Generate unique name with Timestamp
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";

                // Capture and get the full path
                string screenShotPath = CaptureScreenshot(screenshotName);

                // Log to Extent Report
                test.Log(Status.Fail, "Test Failed: " + errorMessage);
                test.AddScreenCaptureFromPath(screenShotPath);
            }
            else if (status == TestStatus.Passed)
            {
                test.Log(Status.Pass, "Test Passed.");
            }
            //clean up driver
            driver?.Quit();
        }
        [OneTimeTearDown]
        public void FinalFlush()
        {
            // report is finally saved to the disk
            extent.Flush();
        }
        public string CaptureScreenshot(string fileName)
        {
            // Path: ProjectRoot/Reports/Screenshots/
           // Navigate up to Project Root(Adjust ".." count if needed for your bin structure)
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
           // string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
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