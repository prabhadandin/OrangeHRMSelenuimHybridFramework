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
        public void Login_With_EmptyUsername_And_Password()
        {
            LoginPage login = new LoginPage(driver.Value);
            test.Value.Log(Status.Info, "Negative login test: Both Username and Password empty.");

            login.Login("", "");

            string actualError = login.GetFieldErrorMessage();
            string expectedError = "Required";

            Assert.That(actualError, Is.EqualTo(expectedError),
                $"Validation failed. Expected: '{expectedError}', Actual: '{actualError}'");

            test.Value.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(2)]
        public void Login_With_Username_Only()
        {
            LoginPage login = new LoginPage(driver.Value);

            test.Value.Log(Status.Info, "Negative login test: Username filled, Password empty.");

            login.Login("Admin", "");

            string actualError = login.GetFieldErrorMessage();
            string expectedError = "Required";

            Assert.That(actualError, Is.EqualTo(expectedError),
                $"Validation failed. Expected: '{expectedError}', Actual: '{actualError}'");

            test.Value.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(3)]
        public void Login_With_Invalid_Credentials()
        {
            LoginPage login = new LoginPage(driver.Value);

            test.Value.Log(Status.Info, "Negative login test: Invalid Username and Password.");

            login.Login("@13!!**", "tes1#$");

            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";

            Assert.That(actualError, Is.EqualTo(expectedError),
                $"Validation failed. Expected: '{expectedError}', Actual: '{actualError}'");

            test.Value.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(4)]
        public void Login_With_Invalid_Password()
        {
            LoginPage login = new LoginPage(driver.Value);

            test.Value.Log(Status.Info, "Negative login test: Valid Username, invalid Password.");

            login.Login("Admin", "wrong_password");

            string actualError = login.GetErrorMessage();
            string expectedError = "Invalid credentials";

            Assert.That(actualError, Is.EqualTo(expectedError),
                $"Validation failed. Expected: '{expectedError}', Actual: '{actualError}'");

            test.Value.Log(Status.Pass, $"Negative test passed: Received expected error '{actualError}'.");
        }

        [Test, Order(5)]
        public void Login_With_Valid_Credentials()
        {
            LoginPage login = new LoginPage(driver.Value);

            test.Value.Log(Status.Info, "Positive login test with valid credentials.");

            login.Login("Admin", "admin123");

            bool isLoaded = WaitManager.WaitForUrlToContain(driver.Value, "dashboard");

            Assert.That(isLoaded, Is.True,
                $"Login failed: Dashboard not reached. Current URL: {driver.Value.Url}");

            test.Value.Log(Status.Pass, "Successfully redirected to Dashboard.");
        }

        [Test, Order(6)]
        public void Login_With_Case_Variation()
        {
            LoginPage login = new LoginPage(driver.Value);

            test.Value.Log(Status.Info, "Case variation login test.");

            login.Login("admin", "admin123");

            bool isLoaded = WaitManager.WaitForUrlToContain(driver.Value, "dashboard");

            Assert.That(isLoaded, Is.True,
                $"Login failed unexpectedly. Current URL: {driver.Value.Url}");

            test.Value.Log(Status.Pass, "Login successful with lowercase username.");

            test.Value.Log(Status.Info, "Testing Logout functionality.");

            bool isDashboardLoaded = WaitManager.WaitForUrlToContain(driver.Value, "dashboard");

            if (isDashboardLoaded)
            {
                login.Logout();

                bool isLogoutSuccess = WaitManager.WaitForUrlToContain(driver.Value, "auth/Login");

                Assert.That(isLogoutSuccess, Is.True,
                    $"Logout failed: Login page not reached. Current URL: {driver.Value.Url}");

                test.Value.Log(Status.Pass, "Successfully logged out.");
            }
            else
            {
                test.Value.Log(Status.Fail,
                    $"Login failed: Dashboard not reached. Current URL: {driver.Value.Url}");

                Assert.Fail("Test Aborted: Cannot perform Logout because Dashboard was not reached.");
            }
        }
    }
}