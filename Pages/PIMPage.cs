using AventStack.ExtentReports;
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
        private By txtIdDuplicateError = By.XPath("//span[contains(@class,'oxd-input-field-error-message') and text()='Employee Id already exists']");
        private By btnSave = By.XPath("//button[@type='submit']");
        private By formLoader = By.ClassName("oxd-form-loader");
        private By toastMessage = By.CssSelector(".oxd-toast-content p.oxd-text--toast-message");
        private By validationMessage = By.CssSelector(".oxd-input-group__message");
        private By menuEmployeeList = By.XPath("//a[text()='Employee List']");
        private By tableLoader = By.ClassName("oxd-table-loader");
        private By searchEmpIdField = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By searchButton = By.XPath("//button[@type='submit']");
        private By resultCellTemplate = By.XPath("//div[@role='cell']"); // will replace dynamically

        public PIMPage(IWebDriver driver) => this.driver = driver;

        public void NavigateToPIM() => WaitManager.WaitUntilClickable(driver, menuPIM).Click();

        public string AddEmployee(string firstName, string middleName, string lastName)
        {
            WaitManager.WaitUntilClickable(driver, btnAddEmployee).Click();
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);

            WaitManager.WaitUntilVisible(driver, txtFirstName).SendKeys(firstName);
            WaitManager.WaitUntilVisible(driver, txtMiddleName).SendKeys(middleName);
            WaitManager.WaitUntilVisible(driver, txtLastName).SendKeys(lastName);

            var idField = WaitManager.WaitUntilVisible(driver, txtEmployeeId);
            string empId = idField.GetAttribute("value");
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);

            WaitManager.WaitUntilClickable(driver, btnSave).Click();

            // Duplicate ID check
            try
            {
                var duplicateErrors = driver.FindElements(txtIdDuplicateError);
                if (duplicateErrors.Count > 0)
                    throw new Exception($"Duplicate Employee ID: {empId} already exists.");

                string toast = WaitManager.WaitUntilVisible(driver, toastMessage).Text;
                if (!toast.Contains("Success"))
                    throw new Exception("Employee not added. Message: " + toast);
            }
            catch (WebDriverTimeoutException)
            {
                var errors = driver.FindElements(validationMessage);
                if (errors.Count > 0)
                    throw new Exception("Employee not added. Validation error: " + errors[0].Text);

                throw new Exception("Employee not added. No success message or validation error appeared.");
            }

            WaitManager.WaitForLoaderToDisappear(driver, formLoader);
            return empId;
        }

        public bool SearchEmployeeById(string empId, ExtentTest reportTest)
        {
            reportTest.Log(Status.Info, $"Searching for Employee ID: {empId}");
            WaitManager.WaitUntilClickable(driver, menuEmployeeList).Click();

            var searchField = WaitManager.WaitUntilVisible(driver, searchEmpIdField);
            searchField.SendKeys(Keys.Control + "a");
            searchField.SendKeys(Keys.Backspace);
            searchField.SendKeys(empId);

            WaitManager.WaitUntilClickable(driver, searchButton).Click();
            WaitManager.WaitForLoaderToDisappear(driver, tableLoader);
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);

            By resultCell = By.XPath($"//div[@role='cell']//div[text()='{empId}']");

            try
            {
                WaitManager.WaitUntilVisible(driver, resultCell);
                reportTest.Log(Status.Pass, $"Employee {empId} found in search results.");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                reportTest.Log(Status.Fail, $"Employee {empId} NOT found in search results.");
                return false;
            }
        }
    }
}