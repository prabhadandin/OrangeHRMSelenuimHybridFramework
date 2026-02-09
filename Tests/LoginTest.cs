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
            [Test]
            public void ValidLoginTest()
            {
            // Initialize Page Object
            LoginPage login = new LoginPage(driver);
            //  Login with credentials
            test.Log(Status.Info, "Entering credentials and clicking login.");
            login.LoginWithValidCredentials("Admin", "admin123");
            //  Wait for Dashboard to load
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("dashboard"));
            // Assert and Log Result
            Assert.That(driver.Url, Does.Contain("dashboard"), "Login failed: Dashboard URL not found.");
            test.Log(Status.Pass, "Successfully navigated to the Dashboard.");

        }
        [Test]
        public void InvalidPasswordTest()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Starting negative login test with invalid password.");

            //  Enter correct username but wrong password
            login.LoginWithValidCredentials("Admin", "wrong_password");

            // Verify "Invalid credentials" message appears
            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";

            Assert.That(actualError, Is.EqualTo(expectedError), "The error message text is incorrect.");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }
    }
    }

