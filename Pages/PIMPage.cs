using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
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
        private By searchBox = By.XPath("//input[@placeholder='Type for hints...']");
        private By searchButton = By.XPath("//button[@type='submit']");
        private By resultRows = By.XPath("//div[@role='rowgroup']//div[@role='row']");
        private By searchEmpIdField = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
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
            WaitManager.WaitUntilClickable(driver, btnSave).Click();

            WaitManager.WaitForLoaderToDisappear(driver, formLoader);

            // duplicate check
            var duplicateError = driver.FindElements(txtIdDuplicateError);
            if (duplicateError.Count > 0)
            {
                throw new Exception($"Duplicate Employee ID: {empId} already exists.");
            }

            return empId; // return generated employee ID if no duplicate

        }

        public string GetSuccessMessage()
        {
            try {
                return WaitManager.WaitUntilVisible(driver, toastMessage).Text;
            }
            catch {
                return driver.FindElement(validationMessage).Text; 
            }
        }
        public bool SearchEmployeeById(string empId, ExtentTest reportTest)
        {
            //Log to Extent Report
            reportTest.Log(Status.Info, $"Navigating to Employee List to search for ID: {empId}");

            // Go to Employee List
            WaitManager.WaitUntilClickable(driver, menuEmployeeList).Click();
            // Enter Employee ID
            var empIdSearch = WaitManager.WaitUntilVisible(driver, searchEmpIdField);
            // JavaScript clear is often more reliable for these custom inputs
            empIdSearch.SendKeys(Keys.Control + "a");
            empIdSearch.SendKeys(Keys.Backspace);
            empIdSearch.SendKeys(empId);
            // Click Search
            WaitManager.WaitUntilClickable(driver, searchButton).Click();
            reportTest.Log(Status.Info, "Clicked Search button. Waiting for table loader to disappear.");
            // Wait for results
            WaitManager.WaitForLoaderToDisappear(driver, tableLoader);
            WaitManager.WaitForLoaderToDisappear(driver, formLoader);
            // This XPath looks for a cell specifically containing your new Employee ID
            // Use this more flexible XPath
            By resultCell = By.XPath($"//div[@role='cell']//div[contains(text(), '{empId}')]");

            try
            {
                // Wait up to 5-10 seconds for the grid to refresh with the actual ID
                WaitManager.WaitUntilVisible(driver, resultCell);
                reportTest.Log(Status.Pass, $"Success: Employee {empId} is visible in the search results.");
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                reportTest.Log(Status.Fail, $"Timeout: Employee {empId} did not appear in the results grid.");
                return false;
                var rows = driver.FindElements(resultRows);
                return rows.Count > 0;
            }
        }
    }
}