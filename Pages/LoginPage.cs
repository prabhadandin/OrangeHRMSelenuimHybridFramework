using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        private By txtUsername = By.Name("username");
        private By txtPassword = By.Name("password");
        private By btnLogin = By.XPath("//button[@type='submit']");
        private By fieldError = By.CssSelector(".oxd-input-field-error-message");
        private By loginError = By.CssSelector(".oxd-alert-content");

        public LoginPage(IWebDriver driver) => this.driver = driver;

        public void Login(string username, string password)
        {
            WaitManager.WaitUntilVisible(driver, txtUsername).SendKeys(username);
            WaitManager.WaitUntilVisible(driver, txtPassword).SendKeys(password);
            WaitManager.WaitUntilClickable(driver, btnLogin).Click();
        }

        public string GetFieldErrorMessage() => driver.FindElement(fieldError).Text;
        public string GetErrorMessage() => driver.FindElement(loginError).Text;

        public void Logout()
        {
            var userMenu = By.XPath("//span[@class='oxd-userdropdown-tab']");
            var logoutBtn = By.XPath("//a[text()='Logout']");
            WaitManager.WaitUntilClickable(driver, userMenu).Click();
            WaitManager.WaitUntilClickable(driver, logoutBtn).Click();
        }
    }
}