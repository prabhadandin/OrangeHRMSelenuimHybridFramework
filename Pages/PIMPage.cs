using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class PIMPage
    {
        private IWebDriver driver;
        private By menuPIM = By.XPath("//span[text()='PIM']");
        private By btnAddEmployee = By.XPath("//a[contains(., 'Add')]");
        private By txtFirstName = By.Name("firstName");
        private By txtMiddleName = By.Name("middleName");
        private By txtLastName = By.Name("lastName");
        private By txtEmployeeId = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By btnSave = By.XPath("//button[@type='submit']");
        private By formLoader = By.ClassName("oxd-form-loader");
        private By toastMessage = By.CssSelector(".oxd-toast-content p.oxd-text--toast-message");
        private By validationMessage = By.CssSelector(".oxd-input-group__message");

        public PIMPage(IWebDriver driver) => this.driver = driver;

        public void NavigateToPIM() => WaitManager.WaitUntilClickable(driver, menuPIM).Click();

        public void AddEmployee(string firstName, string middleName, string lastName, string employeeId)
        {
            WaitManager.WaitUntilClickable(driver, btnAddEmployee).Click();
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);
            WaitManager.WaitUntilVisible(driver, txtFirstName).SendKeys(firstName);
            WaitManager.WaitUntilVisible(driver, txtMiddleName).SendKeys(middleName);
            WaitManager.WaitUntilVisible(driver, txtLastName).SendKeys(lastName);

            var idField = WaitManager.WaitUntilVisible(driver, txtEmployeeId);
            idField.Clear();
            idField.SendKeys(employeeId);

            WaitManager.WaitUntilClickable(driver, btnSave).Click();
        }

        public string GetSuccessMessage()
        {
            try { return WaitManager.WaitUntilVisible(driver, toastMessage).Text; }
            catch { return driver.FindElement(validationMessage).Text; }
        }
    }
}