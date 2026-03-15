using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Utilities;
using SeleniumExtras.WaitHelpers;
namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class PIMPage 
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        public PIMPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        //PiM Locators
        //local locator
        // private By menuPIM = By.XPath("//a[contains(@href, 'viewPimModule')]");
        //github locator
        private By menuPIM = By.XPath("//span[text()='PIM']");
        private By btnAddEmployee = By.XPath("//a[contains(., 'Add')]");
        private By txtFirstName = By.Name("firstName");
        private By txtMiddleName = By.Name("middleName");
        private By txtLastName = By.Name("lastName");
        private By txtEmployeeId = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By btnSave = By.XPath("//button[@type='submit']");
        private By lblSuccessMessage = By.Id("oxd-toaster_1");
        private By formLoader = By.ClassName("oxd-form-loader");
        // Methods
        public void NavigateToPIM()
        {

            //github 
            WaitManager.WaitUntilClickable(driver, menuPIM, 20).Click();
            //local 
            //wait.Until(d => d.FindElement(menuPIM)).Click();
        }

        // Fill the form using data passed from Excel (via the Test class)
        public void AddEmployee(string firstName, string middleName, string lastName, string employeeId)
        {
            //var addBtn = wait.Until(d => d.FindElement(btnAddEmployee));
            // addBtn.Click();


            // Wait until the 'Add' button is ready
            WaitManager.WaitUntilClickable(driver, btnAddEmployee, 20).Click();


            // Verify wheteher on Add Employee page before continuing
            // wait.Until(d => d.Url.Contains("addEmployee"));
            WaitManager.WaitForUrlToContain(driver, "addEmployee", 20);
            WaitManager.WaitForLoaderToDisappear(driver, formLoader, 20);
            // Wait for First Name (signals the form is visible)
            //var fName = wait.Until(d => d.FindElement(txtFirstName));
            //fName.SendKeys(firstName);


            // Ensure the form is visible before typing
            WaitManager.WaitUntilVisible(driver, txtFirstName, 20).SendKeys(firstName);

            // wait.Until(d => d.FindElement(txtMiddleName)).SendKeys(middleName);
            WaitManager.WaitUntilVisible(driver, txtMiddleName, 20).SendKeys(middleName);
            WaitManager.WaitUntilClickable(driver, txtLastName, 20).SendKeys(lastName);
            // driver.FindElement(txtLastName).SendKeys(lastName);
            //wait for the Employee ID field before clearing / typing
            //var idField = wait.Until(d => d.FindElement(txtEmployeeId));
            var idField = WaitManager.WaitUntilVisible(driver, txtEmployeeId, 20);

            // clear autofille first before passing Excel  employee ID
            idField.SendKeys(Keys.Control + "a");
            idField.SendKeys(Keys.Backspace);
            idField.SendKeys(employeeId);
            // Click outside to trigger "ID Validation" check
            // driver.FindElement(txtLastName).Click();
            WaitManager.WaitUntilClickable(driver, txtLastName, 20).Click();
            //Submit with a JS Click  in Headless/Slow page load
            var saveBtn = wait.Until(d => d.FindElement(btnSave));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].click();", saveBtn);
        }

        public string GetSuccessMessage()
        {
            try
            {
                // locator for the toast content 
                By toastLocator = By.CssSelector(".oxd-toast-content p.oxd-text--toast-message");
                // Wait for the element to appear
                var toast = wait.Until(d =>
                {
                    var el = d.FindElement(toastLocator);
                    return (el.Displayed && !string.IsNullOrEmpty(el.Text)) ? el : null;
                });
                return toast.Text;
            }
            catch (WebDriverTimeoutException)
            {
                // Check if there is a red validation error (employee ID already exists)
                try
                {
                    return driver.FindElement(By.CssSelector(".oxd-input-group__message")).Text;
                }
                catch
                {
                    return "No toast message found";
                }
            }

        }
    }
}
