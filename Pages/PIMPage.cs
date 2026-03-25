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
        private By searchEmpIdField = By.XPath("//label[text()='Employee Id']/parent::div/following-sibling::div/input");
        private By searchButton = By.XPath("//button[@type='submit']");
        private By resultRows = By.XPath("//div[@role='rowgroup']//div[@role='row']");

        public PIMPage(IWebDriver driver) => this.driver = driver;

        public void NavigateToPIM() =>
            WaitManager.WaitUntilClickable(driver, menuPIM).Click();

        // ---------------- ADD EMPLOYEE ----------------
        public string AddEmployee(string firstName, string middleName, string lastName)
        {
            try
            {
                WaitManager.WaitUntilClickable(driver, btnAddEmployee).Click();
                WaitManager.WaitUntilVisible(driver, txtFirstName).SendKeys(firstName);
                WaitManager.WaitUntilVisible(driver, txtMiddleName).SendKeys(middleName);
                WaitManager.WaitUntilVisible(driver, txtLastName).SendKeys(lastName);

                var idField = WaitManager.WaitUntilVisible(driver, txtEmployeeId);
                string empId = idField.GetAttribute("value");

                WaitManager.WaitUntilClickable(driver, btnSave).Click();

                WaitManager.WaitUntilVisible(driver, toastMessage);

                var duplicateError = driver.FindElements(txtIdDuplicateError);
                if (duplicateError.Count > 0)
                {
                    throw new Exception($"Duplicate Employee ID detected: {empId}");
                }

                return empId;
            }
            catch (Exception ex)
            {
                throw new Exception($"AddEmployee failed: {ex.Message}", ex);
            }
        }

        // ---------------- SUCCESS MESSAGE ----------------
        public string GetSuccessMessage()
        {
            try
            {
                return WaitManager.WaitUntilVisible(driver, toastMessage).Text;
            }
            catch
            {
                return driver.FindElement(validationMessage).Text;
            }
        }

        // ---------------- SEARCH EMPLOYEE ----------------
        public bool SearchEmployeeById(string empId, ExtentTest reportTest)
        {
            try
            {
                reportTest.Log(Status.Info, $"Searching Employee ID: {empId}");

                WaitManager.WaitUntilClickable(driver, menuEmployeeList).Click();

                var empIdSearch = WaitManager.WaitUntilVisible(driver, searchEmpIdField);

                // stable clear for React input
                ((IJavaScriptExecutor)driver)
                    .ExecuteScript("arguments[0].value='';", empIdSearch);

                empIdSearch.SendKeys(empId);

                WaitManager.WaitUntilClickable(driver, searchButton).Click();

                reportTest.Log(Status.Info, "Search clicked. Waiting for results...");

                WaitManager.WaitForLoaderToDisappear(driver, tableLoader);

                By resultCell = By.XPath(
                    $"//div[@role='row']//div[normalize-space()='{empId}']"
                );

                WaitManager.WaitUntilVisible(driver, resultCell);

                reportTest.Log(Status.Pass, $"Employee {empId} found in search results.");

                return true;
            }
            catch (WebDriverTimeoutException)
            {
                var rows = driver.FindElements(resultRows);

                bool found = rows.Count > 0;

                reportTest.Log(
                    found ? Status.Pass : Status.Fail,
                    found
                        ? $"Employee {empId} found indirectly in grid."
                        : $"Employee {empId} NOT found in search results."
                );

                return found;
            }
            catch (Exception ex)
            {
                reportTest.Log(Status.Fail, $"Search failed: {ex.Message}");
                return false;
            }
        }
    }
}