using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Utilities;

[assembly: Parallelizable(ParallelScope.Fixtures)]

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
                // 🔥 CI STABLE OPTIONS
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--remote-debugging-port=9222");
                options.AddArgument("--disable-extensions");
            }

            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--lang=en-US");
            options.AddUserProfilePreference("intl.accept_languages", "en-US");

            Console.WriteLine("Launching browser...");

            driver.Value = new ChromeDriver(
                ChromeDriverService.CreateDefaultService(),
                options,
                TimeSpan.FromMinutes(2)
            );

            driver.Value.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            WaitManager.SetImplicitWait(driver.Value, 5);

            Console.WriteLine("Navigating to login page...");

            
            driver.Value.Navigate().GoToUrl("https://opensource-demo.orangehrmlive.com/web/index.php/auth/login");
            // wait full page load
            new WebDriverWait(driver.Value, TimeSpan.FromSeconds(30))
                .Until(d => ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState").ToString() == "complete");
            // wait username safely
            var wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(30));


            Console.WriteLine("Waiting for login page to load...");


            wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(By.CssSelector("input[name='username']"));
                    return el.Displayed;
                }
                catch
                {
                    return false;
                }
            });
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message;

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{timestamp}";

            string screenshotPath = CaptureScreenshot(screenshotName);

            string base64 = ((ITakesScreenshot)driver.Value).GetScreenshot().AsBase64EncodedString;

            if (status == TestStatus.Failed)
            {
                test.Value.Log(Status.Fail, "Test Failed: " + message,
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());

                test.Value.Info($"Screenshot saved at: {screenshotPath}");
            }
            else
            {
                test.Value.Log(Status.Pass, "Test Passed",
                    MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
            }

            Console.WriteLine("Closing browser...");
            driver.Value.Quit();
        }

        [OneTimeTearDown]
        public void FinalFlush()
        {
            extent.Flush();
            driver.Dispose();
            test.Dispose();
        }

        public string CaptureScreenshot(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();

            foreach (var c in invalidChars)
                fileName = fileName.Replace(c, '_');

            char[] extraChars = { '"', ':', '<', '>', '|', '*', '?', ',', '(', ')' };

            foreach (var c in extraChars)
                fileName = fileName.Replace(c, '_');

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