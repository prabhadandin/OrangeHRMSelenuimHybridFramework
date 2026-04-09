## 🧪 OrangeHRM Selenium Hybrid Automation Framework (C#)

This is a real-world Selenium Hybrid Automation Framework built using **C#, NUnit, and Selenium WebDriver**. The framework follows the **Page Object Model (POM)** design and supports Data-Driven Testing, Excel integration, Extent Reports, and CI/CD execution via GitHub Actions.

It automates key functionalities of the **OrangeHRM** demo application, including login validation and employee management (PIM module).

---
## 🌐 Live Test Report
(https://prabhadandin.github.io/OrangeHRMSeleniumHybridFramework/Reports/ExtentReport_20260329_222755.html)

### 🎯 Target Application
- **Application:** OrangeHRM Open Source
- **URL:** [https://opensource-demo.orangehrmlive.com](https://opensource-demo.orangehrmlive.com)
- **Purpose:** Automating End-to-End (E2E) Human Resources Management workflows.
- **Workflows:** Automating HR Admin tasks including Login, Employee Creation, and Records Search.

---

### 🚀 Tech Stack
- **Language:** C# (.NET)
- **Engine:** Selenium WebDriver
- **Test Runner:** NUnit
- **Reporting:** ExtentReports (HTML)
- **Data Handling:** Excel (Data-Driven)
- **CI/CD:** GitHub Actions (YAML Pipeline)
- **Architecture:** Page Object Model (POM)

---

### 📁 Project Structure

Base/
└── BaseTest.cs          # Setup/Teardown for Driver & Reports


Pages/
├── LoginPage.cs         # Locators & Actions for Login
└── PIMPage.cs           # Locators & Actions for Employee Management


Tests/
├── LoginTest.cs         # Authentication Test Suites
└── PIMTest.cs           # E2E Employee Workflow Test Suites


Utilities/
├── DriverSetup.cs       # Browser Factory 
├── WaitManager.cs       # Explicit & Fluent Wait wrappers
├── ExcelManager.cs      # Data-reading logic
└── ExtentManager.cs     # Reporting configuration


TestData/
└── EmployeeData.xlsx    # External Test Data


Reports/
└── ExtentReport.html    # Generated Test Results
screenshots/
└── tests.png            # Automated captures

✨ Key Features
✔ End-to-End (E2E) Testing
Integrated Workflows: Validates the full flow of logging in, creating an employee record, and verifying its existence via search.
Data Integrity: Ensures that data entered in the UI is correctly persisted and retrievable.


✔ Reporting & Screenshots
Extent Reports: Detailed HTML reports with step-by-step execution logs.
Auto-Screenshot: Captures and attaches screenshots to the report on test failure.


✔ Framework Design
Page Object Model: Clean separation of UI elements and test logic.
Reusable Utilities: Centralized logic for Driver Setup, Waits, Excel, and Reporting.
Scalable Architecture: Hybrid design allowing for easy addition of new modules.

✔ Data-Driven Testing
Excel Integration: Employee test data is pulled directly from .xlsx files.
Dynamic Iteration: Supports running the same test across multiple datasets.

✔ PIM Module Automation
Add Employee: Handles auto-generated IDs from the application.
Search Employee: Verification by ID and Name.
Validation: Handles duplicate detection and flow verification.

✔ Login Module Automation
Positive Testing: Valid credential access.
Negative Scenarios:
Empty Username/Password fields.
Invalid credentials & case-sensitivity checks.
Partial credential entry (Username only/Password only).


📊 Extent Reports
The framework generates detailed HTML reports including:
Step-by-step execution logs.
Pass/Fail status with timestamps.
Automated Screenshots attached to failed test steps.
Visual dashboard of test execution summary.


⚡ CI/CD Integration
This project is integrated with GitHub Actions for continuous testing. The pipeline:
Restores NuGet dependencies.
Builds the solution.
Executes NUnit tests in Headless mode.
Publishes test artifacts (Extent Reports).


⚙️ How to Run

Clone repository

--bash--

1.git clone https://github.com/your-username/OrangeHRMSeleniumHybridFramework.git

2.Open solution in Visual Studio 22

3.Restore NuGet packages

4.Update TestData/EmployeeData.xlsx if needed.

6.Run tests via Test Explorer or via CLI:

--bash

dotnet test

