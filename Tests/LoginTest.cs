using AventStack.ExtentReports;
using NUnit.Framework;
using OrangeHRMHybridAutomationFramework.Base;
using OrangeHRMHybridAutomationFramework.Pages;
using OrangeHRMHybridAutomationFramework.Utilities;

namespace OrangeHRMHybridAutomationFramework.Tests
{
    [TestFixture]
    public class LoginTest : BaseTest
    {
        [Test, Order(1)]
        public void LoginWithBothFieldsEmpty()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Negative login test: Both Username and Password empty.");
            login.Login("", "");
            string actualError = login.GetFieldErrorMessage(); // Get error from page
            string expectedError = "Required";
            Assert.That(actualError, Is.EqualTo(expectedError), $"Expected error '{expectedError}', but got '{actualError}'");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(2)]
        public void LoginWithPasswordFieldEmpty()
        {
            LoginPage login = new LoginPage(driver!);
            test.Log(Status.Info, "Negative login test: Username filled, Password empty.");
            login.Login("Admin", "");
            string actualError = login.GetFieldErrorMessage();
            string expectedError = "Required";
            Assert.That(actualError, Is.EqualTo(expectedError), $"Expected error '{expectedError}', but got '{actualError}'");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }
        [Test, Order(3)]
        public void LoginWithInvalidCredentials()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Negative login test: Invalid Username and Password.");
            login.Login("@13!!**", "tes1#$");
            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";
            Assert.That(actualError, Is.EqualTo(expectedError), $"Expected error '{expectedError}', but got '{actualError}'");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(4)]
        public void InvalidPassword()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Negative login test: Valid Username, invalid Password.");
            login.Login("Admin", "wrong_password");
            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";
            Assert.That(actualError, Is.EqualTo(expectedError), $"Expected error '{expectedError}', but got '{actualError}'");
            test.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(5)]
        public void LoginWithValidCredentials()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Positive login test with valid credentials.");
            login.Login("Admin", "admin123");
            bool isLoaded = WaitManager.WaitForUrlToContain(driver, "dashboard");
            Assert.That(isLoaded, Is.True, $"Login failed: Dashboard URL not reached. Current URL: {driver.Url}");
            test.Log(Status.Pass, "Successfully redirected to Dashboard.");
        }

        [Test, Order(6)]
        public void LogoutTest()
        {
            LoginPage login = new LoginPage(driver);
            test.Log(Status.Info, "Testing Logout functionality.");
            login.Login("Admin", "admin123");
            bool isDashboardLoaded = WaitManager.WaitForUrlToContain(driver, "dashboard");
            if (isDashboardLoaded)
            {
                login.Logout();
                bool isLogoutSuccess = WaitManager.WaitForUrlToContain(driver, "Login");
                Assert.That(isLogoutSuccess, Is.True, $"Logout failed: Login page not reached. Current URL: {driver.Url}");
                test.Log(Status.Pass, "Successfully logged out.");
            }
            else
            {
                test.Log(Status.Fail, $"Login failed: Dashboard not reached. Current URL: {driver.Url}");
                Assert.Fail("Test Aborted: Cannot perform Logout because Dashboard was not reached.");
            }
        }
    }
}