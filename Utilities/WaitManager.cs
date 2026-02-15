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
        public static IWebElement WaitUntilClickable(IWebDriver driver, By locator, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d => {
                var e = d.FindElement(locator);
                return (e.Displayed && e.Enabled) ? e : null;
            });
        }
        public static IWebElement WaitUntilVisible(IWebDriver driver, By locator, int seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d => d.FindElement(locator).Displayed ? d.FindElement(locator) : null);
        }
        public static string GetTextWhenVisible(IWebDriver driver, By locator, int seconds = 10)
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
