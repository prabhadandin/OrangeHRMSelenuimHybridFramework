using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class LoginPage
    {
        private IWebDriver driver;
        // private By txtUsername = By.Name("username");
        private By txtUsername = By.XPath("//input[@placeholder='Username' or @name='username' or @type='text']");
        private By txtPassword = By.XPath("//input[@type='password' or @placeholder='Password']");
        private By btnLogin = By.XPath("//button[@type='submit']");
        private By fieldError = By.CssSelector(".oxd-input-field-error-message");
        private By loginError = By.XPath("//p[contains(@class,'oxd-alert-content-text')]");
        public LoginPage(IWebDriver driver) => this.driver = driver;
        public void Login(string username, string password)
        {
            // wait for page to fully render
            WaitManager.WaitUntilVisible(driver, By.TagName("body"));
            var user = WaitManager.WaitUntilVisible(driver, txtUsername,30);
            user.Clear();
            user.SendKeys(username);
            var pass = WaitManager.WaitUntilVisible(driver, txtPassword);
            pass.Clear();
            pass.SendKeys(password);
            WaitManager.WaitUntilClickable(driver, btnLogin).Click();
        }
        public string GetFieldErrorMessage()
        {
            var elements = driver.FindElements(fieldError);
            return elements.Count > 0 ? elements[0].Text : string.Empty;
        }
        // login error (toast or inline)
        public string GetErrorMessage()
        {
            try
            {
                var element = WaitManager.WaitUntilVisible(driver, loginError, 10);
                return element.Text;
            }
            catch
            {
                return string.Empty;
            }
        
    }
 public void Logout()
         {
             var userMenu = By.XPath("//span[@class='oxd-userdropdown-tab']");
             var logoutBtn = By.XPath("//a[text()='Logout']");
             WaitManager.WaitUntilClickable(driver, userMenu).Click();
             WaitManager.WaitUntilClickable(driver, logoutBtn).Click();
         }
    }
}