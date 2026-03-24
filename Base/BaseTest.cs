using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMHybridAutomationFramework.Utilities;
using System;
using System.IO;

namespace OrangeHRMHybridAutomationFramework.Base
{
    public class BaseTest
    {
        public IWebDriver driver = null!;
        protected ExtentTest test = null!;
        protected ExtentReports extent = null!;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            extent = ExtentManager.GetInstance();
        }

        [SetUp]
        public void Setup()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            driver = DriverSetup.GetDriver();

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            WaitManager.SetImplicitWait(driver, 10);

            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com");
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";

            // Save screenshot file
            string screenshotPath = CaptureScreenshot(screenshotName);

            // Capture Base64 (for embedding in report)
            string base64 = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;

            if (status == TestStatus.Failed)
            {
                test.Log(Status.Fail, "Test Failed: " + message,
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());

                test.Info($"Screenshot saved at: {screenshotPath}");
            }
            else
            {
                test.Log(Status.Pass, "Test Passed",
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
            }

            driver.Quit();
        }

        [OneTimeTearDown]
        public void FinalFlush()
        {
            extent.Flush();
        }

        public string CaptureScreenshot(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (var c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }

            char[] extraChars = { '"', ':', '<', '>', '|', '*', '?', ',', '(', ')' };

            foreach (var c in extraChars)
            {
                fileName = fileName.Replace(c, '_');
            }

            string folder = Path.Combine(AppContext.BaseDirectory, "Reports", "Screenshots");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName + ".png");

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            return fullPath;
        }
    }
}