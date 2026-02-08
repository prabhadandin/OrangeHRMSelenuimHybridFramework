using System;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;


namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest

        {
            [Test]
            public void ValidLoginTest()
            {
            LoginPage login = new LoginPage(driver);
            login.LoginWithValidCredentials("Admin", "admin123");
            // wait untill dashboard loads
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Url.Contains("dashboard")); 
            Assert.That(driver.Url, Does.Contain("dashboard"));

        }
        }
    }

