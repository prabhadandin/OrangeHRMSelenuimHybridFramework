using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public static class WaitManager
    {
        public static IWebElement WaitUntilClickable(IWebDriver driver, By locator, int seconds = 20)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    return (el.Displayed && el.Enabled) ? el : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public static IWebElement WaitUntilVisible(IWebDriver driver, By locator, int seconds = 20)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(locator);
                    return el.Displayed ? el : null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException) 
                {
                    return null;
                }
            });
        }
        public static bool WaitForUrlToContain(IWebDriver driver, string fraction, int seconds = 20)
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
        public static void WaitForLoaderToDisappear(IWebDriver driver, By loaderLocator, int seconds = 20)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loaderLocator));
        }
    }
}