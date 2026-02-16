using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public static  class WaitManager
    {
        //Method to wait until the element is ready to click
        public static IWebElement WaitUntilClickable(IWebDriver driver, By locator, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d => {
                var e = d.FindElement(locator);
                return (e.Displayed && e.Enabled) ? e : null;
            });
        }
        //Method to wait for an element to displayed
        public static IWebElement WaitUntilVisible(IWebDriver driver, By locator, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d => d.FindElement(locator).Displayed ? d.FindElement(locator) : null);
        }

        public static bool WaitForUrlToContain(IWebDriver driver, string fraction, int seconds = 100)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            try
            {
                return wait.Until(d => d.Url.ToLower().Contains(fraction.ToLower()));
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        // Method to set Implicit Wait
        public static void SetImplicitWait(IWebDriver driver, int seconds)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        //Method to get the error text once the element is visible
        public static string GetTextWhenReady(IWebDriver driver, By locator, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed ? element.Text : null;
            });
        }
    }
}
