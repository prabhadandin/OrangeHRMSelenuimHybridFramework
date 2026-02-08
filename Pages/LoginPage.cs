using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;



namespace OrangeHRMHybridAutomationFramework.Pages
{
    class LoginPage
    {
        private IWebDriver driver;

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        // Locators
        private By txtUsername = By.Name("username");
        private By txtPassword = By.Name("password");
        private By btnLogin = By.XPath("//button[@type='submit']");

        // Actions
        public void LoginWithValidCredentials(string user, string pass)
        {
            // wait if page loads slowly
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(txtUsername)).SendKeys(user);
            driver.FindElement(txtPassword).SendKeys(pass);
            driver.FindElement(btnLogin).Click();
        }
    }
}