using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(2)]

namespace OrangeHRMHybridAutomationFramework.Base
{
    public class BaseTest
    {
        protected ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();
        protected ThreadLocal<ExtentTest> test = new ThreadLocal<ExtentTest>();
        protected ExtentReports extent = null!;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            extent = ExtentManager.GetInstance();
        }

        [SetUp]
        public void Setup()
        {
            test.Value = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            Console.WriteLine("Launching browser...");
            driver.Value = DriverSetup.GetDriver();
            driver.Value.Manage().Cookies.DeleteAllCookies();
            Console.WriteLine("Navigating to login page...");
            // Use Config 
            string baseUrl = ConfigReader.Get("baseUrl");
            driver.Value.Navigate().GoToUrl(baseUrl);
            // Wait for Username field using WaitManager
            var loginPage = new LoginPage(GetDriver());
            Assert.That(loginPage.IsLoginPageLoaded(), Is.True, "Login page not loaded properly");
            Console.WriteLine("Login page ready.");
            Console.WriteLine($"TEST: {TestContext.CurrentContext.Test.Name}");
            Console.WriteLine($"THREAD: {Thread.CurrentThread.ManagedThreadId}");
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string testName = TestContext.CurrentContext.Test.Name;

            string safeTestName = new string(testName
                .Where(c => !Path.GetInvalidFileNameChars().Contains(c))
                .ToArray());

            safeTestName = safeTestName
                .Replace("\"", "_")
                .Replace("(", "_")
                .Replace(")", "_")
                .Replace(",", "_")
                .Replace(" ", "_");

            string screenshotName = $"{safeTestName}_{timestamp}";
            string screenshotPath = "";

            // null-safe screenshot
            if (driver.Value != null)
            {
                try
                {
                    screenshotPath = CaptureScreenshot(screenshotName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to capture screenshot: " + ex.Message);
                }
            }

            if (status == TestStatus.Failed && !string.IsNullOrEmpty(screenshotPath))
            {
                test.Value.Log(Status.Fail, "Test Failed: " + message);
                test.Value.AddScreenCaptureFromPath(screenshotPath);
            }
            else
            {
                test.Value.Log(Status.Pass, "Test Passed");
            }
            driver.Value?.Quit();
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            extent?.Flush();
        }

        // Screenshot method
        public string CaptureScreenshot(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars) fileName = fileName.Replace(c, '_');
            fileName = fileName.Replace("\"", "_");

            string folder = Path.Combine(AppContext.BaseDirectory, "Reports", "Screenshots");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fullPath = Path.Combine(folder, fileName + ".png");

            if (driver.Value != null)
            {
                var screenshot = ((ITakesScreenshot)driver.Value).GetScreenshot();
                screenshot.SaveAsFile(fullPath);
            }
            return fullPath;
        }

        //  Driver Getter (for POM usage)
        public IWebDriver GetDriver()
        {
            return driver.Value;
        }
    }
}
