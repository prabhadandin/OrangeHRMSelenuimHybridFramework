using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver driver;

        //  Locators
        private readonly By txtUsername = By.XPath("//input[@placeholder='Username' or @name='username' or @type='text']");
        private readonly By txtPassword = By.XPath("//input[@type='password' or @placeholder='Password']");
        private readonly By btnLogin = By.XPath("//button[@type='submit']");
        private readonly By fieldError = By.CssSelector(".oxd-input-field-error-message");
        private readonly By loginError = By.XPath("//p[contains(@class,'oxd-alert-content-text')]");
        
        private readonly By userMenu = By.XPath("//span[@class='oxd-userdropdown-tab']");
        private readonly By logoutBtn = By.XPath("//a[text()='Logout']");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        //  Login method
        public void Login(string username, string password)
        {
            WaitManager.WaitUntilVisible(driver, txtUsername, 30).Clear();
            WaitManager.WaitUntilVisible(driver, txtUsername).SendKeys(username);

            WaitManager.WaitUntilVisible(driver, txtPassword).Clear();
            WaitManager.WaitUntilVisible(driver, txtPassword).SendKeys(password);

            WaitManager.WaitUntilClickable(driver, btnLogin).Click();
        }

        //  Field validation error 
        public string GetFieldErrorMessage()
        {
            try
            {
                return WaitManager.WaitUntilVisible(driver, fieldError, 5).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }

        // Login error (Invalid credentials)
        public string GetErrorMessage()
        {
            try
            {
                return WaitManager.WaitUntilVisible(driver, loginError, 10).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
        public bool IsLoginPageLoaded()
        {
            try
            {
                return WaitManager.WaitUntilVisible(driver, txtUsername, 10) != null;
            }
            catch
            {
                return false;
            }
        }

        // Logout action
        public void Logout()
        {
            WaitManager.WaitUntilClickable(driver, userMenu).Click();
            WaitManager.WaitUntilClickable(driver, logoutBtn).Click();
            // wait for login page again
            WaitManager.WaitUntilVisible(driver, txtUsername, 10);
        }
    }
}