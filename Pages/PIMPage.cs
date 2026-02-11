using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

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

        // Locators
        private By menuPIM = By.XPath("//span[text()='PIM']");
        private By btnAddEmployee = By.XPath("//a[text()='Add Employee']");
        private By txtFirstName = By.Name("firstName");
        private By txtMiddleName = By.Name("middleName");
        private By txtLastName = By.Name("lastName");
        private By txtEmployeeId = By.Name("employeeId");
        private By btnSave = By.XPath("//button[@type='submit']");
        private By lblSuccessMessage = By.XPath("//div[contains(@class, 'oxd-toast-content')]");

        // Methods
        public void NavigateToPIM()
        {
            wait.Until(d => d.FindElement(menuPIM)).Click();
        }

        // Fill the form using data passed from Excel (via the Test class)
        public void AddEmployee(string firstName,string middleName, string lastName,string employeeId)
        {
            wait.Until(d => d.FindElement(btnAddEmployee)).Click();
            wait.Until(d => d.FindElement(txtFirstName)).SendKeys(firstName);
            wait.Until(d => d.FindElement(txtMiddleName)).SendKeys(middleName);
            driver.FindElement(txtLastName).SendKeys(lastName);
            wait.Until(d => d.FindElement(txtEmployeeId)).SendKeys(employeeId);
            driver.FindElement(btnSave).Click();
        }

        public string GetSuccessMessage()
        {
            return wait.Until(d => d.FindElement(lblSuccessMessage)).Text;
        }
    }
}
