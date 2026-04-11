using AventStack.ExtentReports;
using OpenQA.Selenium;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Pages
{
    public class PIMPage
    {
        private IWebDriver driver;

        public PIMPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        private By menuPIM = By.XPath("//span[text()='PIM']");
        private By btnAddEmployee = By.XPath("//a[contains(., 'Add')]");
        private By txtFirstName = By.Name("firstName");
        private By txtMiddleName = By.Name("middleName");
        private By txtLastName = By.Name("lastName");
        private By txtEmployeeId = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By txtIdDuplicateError = By.XPath("//span[contains(@class,'oxd-input-field-error-message') and text()='Employee Id already exists']");
        private By btnSave = By.XPath("//button[@type='submit']");
        private By formLoader = By.ClassName("oxd-form-loader");
        private By toastMessage = By.CssSelector(".oxd-toast-content p.oxd-text--toast-message");
        private By menuEmployeeList = By.XPath("//a[text()='Employee List']");
        private By tableLoader = By.ClassName("oxd-table-loader");
        private By searchEmpIdField = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By searchButton = By.XPath("//button[@type='submit']");
        public void NavigateToPIM()
        {
            WaitManager.WaitUntilClickable(driver, menuPIM).Click();
        }

        // Add employee
        public string AddEmployee(string firstName,string middleName,string lastName)
        {
            WaitManager.WaitUntilClickable(driver,btnAddEmployee).Click();
            WaitManager.WaitUntilVisible(driver, txtFirstName,20);
            WaitManager.WaitUntilVisible(driver,txtFirstName).SendKeys(firstName);
            WaitManager.WaitUntilVisible(driver,txtMiddleName).SendKeys(middleName);
            WaitManager.WaitUntilVisible(driver,txtLastName).SendKeys(lastName);
            var idField = WaitManager.WaitUntilVisible(driver,txtEmployeeId,20);
            string empId = idField.GetAttribute("value");
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);
            WaitManager.WaitUntilClickable(driver,btnSave).Click();
            // check duplicate
            var duplicate = driver.FindElements(txtIdDuplicateError);
            if (duplicate.Count > 0)
                throw new Exception($"Duplicate Employee ID: {empId}");
            // check toast
            var toast = WaitManager.WaitUntilVisible(driver,toastMessage,15).Text;
            if (!toast.Contains("Success"))
                throw new Exception("Employee not added. Message: " + toast);
            return empId;
        }

        //Search employee
        public bool SearchEmployeeById(string empId)
        {
            WaitManager.WaitUntilClickable(driver, menuEmployeeList).Click();
            WaitManager.WaitForLoaderToDisappear(driver, tableLoader);
            var searchField = WaitManager.WaitUntilVisible(driver, searchEmpIdField);
            searchField.SendKeys(Keys.Control + "a");
            searchField.SendKeys(Keys.Backspace);
            searchField.SendKeys(empId);
            WaitManager.WaitUntilClickable(driver, searchButton).Click();
            WaitManager.WaitForLoaderToDisappear(driver, tableLoader);
            By resultCell = By.XPath($"//div[@role='cell']//div[text()='{empId}']");
            try
            {
                WaitManager.WaitUntilVisible(driver, resultCell,10);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}