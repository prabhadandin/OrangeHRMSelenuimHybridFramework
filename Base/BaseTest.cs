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
        protected ExtentReports extent;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            extent = ExtentManager.GetInstance();
        }

        [SetUp]
        public void Setup()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            // FIX: Added 2-minute timeout to stop the "WebDriverException: Timed out" in CI
            driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(2));

            // Set PageLoad timeout explicitly for CI stability
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);

            WaitManager.SetImplicitWait(driver, 10);
            driver.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com");
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";
                string screenShotPath = CaptureScreenshot(screenshotName);

                test.Log(Status.Fail, "Test Failed: " + errorMessage);
                test.AddScreenCaptureFromPath(screenShotPath);
            }
            else if (status == TestStatus.Passed)
            {
                test.Log(Status.Pass, "Test Passed.");
            }
            driver?.Quit();
        }

        [OneTimeTearDown]
        public void FinalFlush()
        {
            extent.Flush();
        }

        public string CaptureScreenshot(string fileName)
        {
            // IMPROVEMENT: More reliable path logic for both Windows and Linux CI
            string projectRoot = AppContext.BaseDirectory;

            // This loop ensures we find the project folder regardless of bin depth
            DirectoryInfo directory = new DirectoryInfo(projectRoot);
            while (directory != null && !File.Exists(Path.Combine(directory.FullName, "OrangeHRMHybridAutomationFramework.sln")))
            {
                directory = directory.Parent;
            }

            string rootPath = directory?.FullName ?? AppContext.BaseDirectory;
            string folder = Path.Combine(rootPath, "Reports", "Screenshots");

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName + ".png");

            var ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            return fullPath;
        }
    }
}
