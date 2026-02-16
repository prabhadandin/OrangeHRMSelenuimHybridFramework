using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.BrowsingContext;
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
        private By lblErrorMessage = By.XPath("//div[@role='alert']//p");
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
        public void Logout()
        {

           
            WaitManager.WaitUntilClickable(driver, userDropdown, 10).Click();
            WaitManager.WaitUntilClickable(driver, LogoutBtnLocator, 10);
            driver.FindElement(LogoutBtnLocator).Click();

        }
       

    }
}