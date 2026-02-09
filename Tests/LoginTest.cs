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
        }
    }

