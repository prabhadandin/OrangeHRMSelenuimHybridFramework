using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Utilities;
using OpenQA.Selenium.Chrome;

[assembly: Parallelizable(ParallelScope.None)]
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
            options.AddArgument("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");
            // Sets the browser language to English (US)
            options.AddArgument("--lang=en-US");
            //  set the specific preference
            options.AddUserProfilePreference("intl.accept_languages", "en-US");
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            // 2-minute timeout to stop the "WebDriverException: Timed out" in CI
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
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotName = $"{TestContext.CurrentContext.Test.Name}_Success_{timestamp}";
                string screenShotPath = CaptureScreenshot(screenshotName);
                test.Log(Status.Pass, "Test Passed.");
                test.AddScreenCaptureFromPath(screenShotPath);
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
            // Remove invalid characters like " , < , > , | , : from the test name
            string cleanFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            // path logic for both Windows and Linux CI
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
            string fullPath = Path.Combine(folder, cleanFileName + ".png");
            var ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot();
            screenshot.SaveAsFile(fullPath);
            // returns a relative path for the HTML report
            return Path.Combine("Screenshots", cleanFileName + ".png");
        }
    }
}

