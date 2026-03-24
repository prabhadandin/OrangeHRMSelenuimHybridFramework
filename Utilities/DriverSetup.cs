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

            return new ChromeDriver(options);
        }
    }
}