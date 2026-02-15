using System;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using AventStack.ExtentReports;


namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest

        {
            [Test, Order(5)]
            public void LoginWithValidCredentials()
            {
            // Initialize Page Object
            LoginPage login = new LoginPage(driver);
           
            //  Login with credentials
            test.Log(Status.Info, "Entering credentials and clicking login.");
            login.Login("Admin", "admin123");
            //  Wait for Dashboard to load
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
            wait.Until(d => d.Url.Contains("dashboard"));
            // Assert and Log Result
            Assert.That(driver.Url, Does.Contain("dashboard"), "Login failed: Dashboard URL not found.");
            test.Log(Status.Pass, "Successfully navigated to the Dashboard.");

        }
        [Test ,Order(1)]
        public void LoginWithBothFieldsEmpty()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Starting negative login test with Username and Password Empty .");
            login.Login("", "");

            // Verify "empty fields" message appears
             string emptyactualError = "Required";
             string emptyexpectedError = "Required";
            Assert.That(emptyactualError, Is.EqualTo(emptyexpectedError));
             test.Log(Status.Pass, $"Negative test passed: Received expected error '{emptyactualError}'.");

        }
        [Test, Order(2)]
        public void LoginWithPasswordFieldEmpty()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Starting negative login test With Username and leaving password empty ");

            login.Login("Admin", "");
            string emptyactualError = "Required";
            string emptyexpectedError = "Required";
            Assert.That(emptyactualError, Is.EqualTo(emptyexpectedError));
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{emptyactualError}'.");
        }

        [Test, Order(3)]
        public void LoginWithUnvalidCredentials()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Starting negative login test with Invalid Username and password  ");

            login.Login("@13!!**", "tes1#$");
            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";

            Assert.That(actualError, Is.EqualTo(expectedError), "The error message text is incorrect.");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");

        }


        [Test, Order(4)]
        public void InvalidPasswordTest()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Starting negative login test with valid username and invalid password.");

            //  Enter correct username but wrong password
            login.Login("Admin", "wrong_password");

            // Verify "Invalid credentials" message appears
            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";

            Assert.That(actualError, Is.EqualTo(expectedError), "The error message text is incorrect.");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }
    }
    }

