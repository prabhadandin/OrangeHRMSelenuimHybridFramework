using OpenQA.Selenium;


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

        // method to login with valid credentials
        public void LoginWithValidCredentials(string user, string pass)
        {
            // wait if page loads slowly
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            // Wait for the username field and interact
            wait.Until(d => d.FindElement(txtUsername)).SendKeys(user);
            driver.FindElement(txtPassword).SendKeys(pass);
            driver.FindElement(btnLogin).Click();
        }
        // method to retrieve the error text
        public string GetErrorMessage()
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, System.TimeSpan.FromSeconds(10));
            return wait.Until(d => d.FindElement(lblErrorMessage)).Text;
        }
    }
}