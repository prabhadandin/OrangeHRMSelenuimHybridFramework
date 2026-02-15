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
        public  By ValidationErrorMessageLocator = By.CssSelector(".oxd-input-field-error-message");
        private By lblErrorMessage = By.XPath("//div[@role='alert']//p");
        private By userDropdown = By.CssSelector(".manda user");
        private By LogoutBtnLocator = By.LinkText("Logout");
       
        

        // method to login with valid credentials
        public void Login(string user, string pass)
        {
            // wait if page loads slowly
            // var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            // Wait for the username field and interact
            //wait.Until(d => d.FindElement(txtUsername)).SendKeys(user);
            WaitManager.WaitUntilVisible(driver, txtUsername).SendKeys(user);
            driver.FindElement(txtPassword).SendKeys(pass);
           // driver.FindElement(btnLogin).Click();
           WaitManager.WaitUntilClickable(driver,btnLogin).Click();
        }
        // method to retrieve the error text
        public string GetErrorMessage()
        {
             var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            //return wait.Until(d => d.FindElement(lblErrorMessage)).Text;
            string errormessage = wait.Until(d => d.FindElement(lblErrorMessage).Displayed ? d.FindElement(lblErrorMessage).Text : null);
            return errormessage;
        }
        public void Logout()
        {

            //driver.FindElement(userDropdown).Click();
            WaitManager.WaitUntilClickable(driver, userDropdown, 10);

            WaitManager.WaitUntilClickable(driver, LogoutBtnLocator, 10);

            // driver.FindElement(LogoutBtnLocator).Click();
        }
       

    }
}