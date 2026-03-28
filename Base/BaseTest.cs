using AventStack.ExtentReports;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
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
            var options = new ChromeOptions();
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--remote-debugging-port=9222");
                options.AddArgument("--disable-extensions");
            }

            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--lang=en-US");
            Console.WriteLine("Launching browser...");
            driver.Value = new ChromeDriver(
                ChromeDriverService.CreateDefaultService(),
                options,
                TimeSpan.FromMinutes(2)
            );

            driver.Value.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            Console.WriteLine("Launching browser...");
            driver.Value.Manage().Cookies.DeleteAllCookies();
            Console.WriteLine("Navigating to login page...");
            driver.Value.Navigate().GoToUrl(
                "https://opensource-demo.orangehrmlive.com/web/index.php/auth/login"
            );

            WebDriverWait wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(40));
            //  Wait page ready
            wait.Until(d =>
                ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState")
                .ToString() == "complete"
            );
            Console.WriteLine("Page loaded: " + driver.Value.Url);
            wait.Until(d =>
                d.FindElements(By.XPath("//input[@placeholder='Username' or @name='username' or @type='text']"))
                 .Count > 0
            );
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
            // fix for CI
            safeTestName = safeTestName
                .Replace("\"", "_")
                .Replace("(", "_")
                .Replace(")", "_")
                .Replace(",", "_")
                .Replace(" ", "_");
            string screenshotName = $"{safeTestName}_{timestamp}";
            string screenshotPath = CaptureScreenshot(screenshotName);
            
           /* string base64 = ((ITakesScreenshot)driver.Value)
                .GetScreenshot()
                .AsBase64EncodedString;*/
            if (status == TestStatus.Failed)
            {
                test.Value.Log(Status.Fail, "Test Failed: " + message);
                test.Value.AddScreenCaptureFromPath(screenshotPath);
            }
            else
            {
                test.Value.Log(Status.Pass, "Test Passed");
                test.Value.AddScreenCaptureFromPath(screenshotPath);

            }

            driver.Value?.Quit();
        }
        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            extent?.Flush();
        }
        public string CaptureScreenshot(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            fileName = fileName.Replace(c, '_');
            fileName = fileName.Replace("\"", "_");
            string folder = Path.Combine(AppContext.BaseDirectory, "Reports", "Screenshots");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string fullPath = Path.Combine(folder, fileName + ".png");
            var screenshot = ((ITakesScreenshot)driver.Value).GetScreenshot();
            screenshot.SaveAsFile(fullPath);
            return fullPath;
        }
    }
}