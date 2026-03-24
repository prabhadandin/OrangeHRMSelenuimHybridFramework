using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMHybridAutomationFramework.Utilities;
using System;
using System.IO;

namespace OrangeHRMHybridAutomationFramework.Base
{
//    [assembly: Parallelizable(ParallelScope.None)]
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

            var options = new ChromeOptions();
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                // Headless for GitHub Actions
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--lang=en-US");
            options.AddUserProfilePreference("intl.accept_languages", "en-US");
            // driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(2));
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
            //Generate unique name for the file
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";
            //Save physical file (GitHub Actions Artifacts)
            string screenshotPath = CaptureScreenshot(screenshotName);
            if (status == TestStatus.Failed)
            {
<<<<<<< HEAD
                test.Log(Status.Fail, message);
                test.AddScreenCaptureFromPath(screenshotPath);
            }
            else
            {
                test.Log(Status.Pass, "Test Passed");
                test.AddScreenCaptureFromPath(screenshotPath);
=======
                //Capture Base64 for the Extent Report (Self-contained in the HTML)
                string base64 = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;
                test.Value.Log(Status.Fail, "Test Failed: " + message,
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
                //link the physical path in the report
                test.Value.Info($"Local screenshot saved at: {screenshotPath}");
            }
            else
            {
                test.Value.Log(Status.Pass, "Test Passed");
              //  test.Value.AddScreenCaptureFromPath(screenshotPath);
>>>>>>> Added PIM Employee automation with Excel-driven data, duplicate ID validation, and search verification
            }

            driver.Quit();
        }

        [OneTimeTearDown]
        public void FinalFlush()
        {

            extent.Flush();
            // Disposes the ThreadLocal container
            driver.Dispose();
            test.Dispose();
        }

        public string CaptureScreenshot(string fileName)
        {
            // Remove invalid characters from filename
            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (var c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }

            // Additional characters GitHub Actions does not allow
            char[] extraChars = { '"', ':', '<', '>', '|', '*', '?', ',', '(', ')' };

            foreach (var c in extraChars)
            {
                fileName = fileName.Replace(c, '_');
            }

            string folder = Path.Combine(AppContext.BaseDirectory, "Reports", "Screenshots");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName + ".png");

            var ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot();
            screenshot.SaveAsFile(fullPath);

            //return Path.Combine("Screenshots", fileName + ".png");
            // return FULL PATH for ExtentReports
            return fullPath;
        }
    }
}