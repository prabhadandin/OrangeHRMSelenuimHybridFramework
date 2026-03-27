# 🧪 OrangeHRM Selenium Hybrid Automation Framework (C#)

This is a **real-world Selenium Hybrid Automation Framework** built using **C#, NUnit, and Selenium WebDriver**. The framework follows **Page Object Model (POM)** design and supports **data-driven testing, Excel integration, Extent Reports, and CI/CD execution via GitHub Actions (YML pipeline)**.

It automates key functionalities of the OrangeHRM demo application including login validation and employee management (PIM module).
## 🚀 Tech Stack

- C# (.NET)
- Selenium WebDriver
- NUnit Framework
- ExtentReports (HTML Reporting)
- Excel Data-Driven Testing
- GitHub Actions (CI/CD)
- Page Object Model (POM)
## 📁 Project Structure
Base/
└── BaseTest.cs

Pages/
├── LoginPage.cs
└── PIMPage.cs
Tests/
├── LoginTest.cs
└── PIMTest.cs
Utilities/
├── WaitManager.cs
├── ExcelManager.cs
├── ScreenshotHelper.cs
└── ExtentReportManager.cs
TestData/
└── EmployeeData.xlsx

Reports/
└── ExtentReport.html
screenshots/
└── tests.png

---

## ⚙️ Key Features

### ✔ Framework Design
- Page Object Model (clean separation of UI & test logic)
- Reusable utilities for waits, Excel, screenshots, reporting
- Scalable hybrid test architecture

### ✔ Data Driven Testing
- Employee test data is read from Excel
- Dynamic employee creation using multiple datasets

### ✔ PIM Module Automation
- Add Employee with auto-generated Employee ID from application
- Search employee by ID
- Duplicate employee detection handling
- Validation of employee creation flow

### ✔ Login Module Automation
- Valid login test
- Multiple negative login scenarios:
  - Empty username/password
  - Invalid credentials
  - Case variation login test
- Logout validation

---

## 📊 Extent Reports

The framework generates detailed HTML reports including:

- Step-by-step execution logs
- Pass/Fail status
- Screenshots for key actions (Add Employee, Search Employee)
- Test execution summary
## ⚡ CI/CD Integration

This project is integrated with **GitHub Actions YAML pipeline** for automated execution.

Supports:
- Headless browser execution
- Automated test runs on push
- Report generation after execution
## 📌 Parallel Execution

NUnit parallel execution is enabled:

```csharp
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(2)]
🧪 Test Scenarios
🔐 Login Tests
Login with empty fields
Login with only username
Invalid credentials
Case variation login
Valid login + logout validation
👤 PIM Tests
Add employee using Excel data
Auto-generated Employee ID validation
Search employee by ID
Duplicate employee validation
📊 Test Data Flow
Employee data is read from Excel (EmployeeData.xlsx)
Application generates Employee ID automatically
That Employee ID is reused for:
Search validation
Duplicate checking
▶️ How to Run
1.Clone repository
--bash--
git clone https://github.com/your-username/OrangeHRMSeleniumHybridFramework.git
2.Open solution in Visual Studio
3.Restore NuGet packages
4.Build project
5.Run tests via:
Test Explorer OR
GitHub Actions pipeline
