OrangeHRM Hybrid Selenium Framework

Automated tests using Selenium and C# Sandbox used: https://opensource-demo.orangehrmlive.com

Framework Architecture
This project follows the Page Object Model (POM) design pattern to ensure maintainability and reusability.
-Base: Contains the `BaseTest` class,it setup and teardown of Selenium tests, specifically handling browser initialization, 
logging test results into an ExtentReport, and automatically capturing screenshots of any failures.
- Pages: Contains Page Objects i,e WebElements and Page Actions  are stored.
- Utilities:Contains EXtentmanager  to creates a  HTML report of test results, including passes, failures, and system details.
- Tests:  Contains all test scripts
