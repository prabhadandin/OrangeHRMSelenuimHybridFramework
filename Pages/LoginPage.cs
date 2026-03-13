using OpenQA.Selenium;
//using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Utilities;
using System;
namespace OrangeHRMHybridAutomationFramework.Pages
{
   public class LoginPage
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
        // For "Invalid Credentials"
        private By lblErrorMessage = By.XPath("//div[@role='alert']//p");
        // For "Required" field validation
        private By lblFieldErrors = By.XPath("//span[contains(@class, 'oxd-input-field-error-message')]");
        private By userDropdown = By.CssSelector(".oxd-userdropdown-tab");
        private By LogoutBtnLocator = By.XPath("//a[contains(text(),'Logout')]");



        // method to login with valid credentials
        public void Login(string user, string pass)
        {
            // wait if page loads slowly
            WaitManager.WaitUntilVisible(driver, txtUsername).SendKeys(user);
            driver.FindElement(txtPassword).SendKeys(pass);
            WaitManager.WaitUntilClickable(driver,btnLogin).Click();
        }
        // method to retrieve the error text
        public string GetErrorMessage()
        {
            return WaitManager.GetTextWhenReady(driver, lblErrorMessage);
        }
        public string GetFieldErrorMessage()
        {
            // This returns the text of the FIRST validation error found (e.g., under Username)
            return WaitManager.GetTextWhenReady(driver, lblFieldErrors, 10);
        }
        public void Logout()
        {

           
            WaitManager.WaitUntilClickable(driver, userDropdown, 10).Click();
            WaitManager.WaitUntilClickable(driver, LogoutBtnLocator, 10);
            driver.FindElement(LogoutBtnLocator).Click();

        }
       

    }
}