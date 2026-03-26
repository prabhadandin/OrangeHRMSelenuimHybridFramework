using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public class DriverSetup
    {
        public static IWebDriver GetDriver()
        {
            var options = new ChromeOptions();

            // Run headless only in CI (GitHub Actions)
            if (Environment.GetEnvironmentVariable("CI") == "true")
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-gpu");
            }

            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--lang=en-US");
            options.PageLoadStrategy = PageLoadStrategy.Normal;
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(service, options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }
    }
}