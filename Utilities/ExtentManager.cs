using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public class ExtentManager
    {

        private static ExtentReports? extent;
        private static ExtentSparkReporter? sparkReporter;
        public static ExtentReports GetInstance()
        {
            if (extent == null)
            {
                // Use AppContext.BaseDirectory for cross-platform stability
                string reportDirectory = Path.Combine(AppContext.BaseDirectory, "Reports");
                // Create directory if it doesn't exist
                if (!Directory.Exists(reportDirectory))
                    Directory.CreateDirectory(reportDirectory);
                //string finalReportPath = Path.Combine(reportDirectory, "ExtentReport.html");

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string finalReportPath = Path.Combine(reportDirectory, $"ExtentReport_{timestamp}.html");
                //  Initialize Report
                sparkReporter = new ExtentSparkReporter(finalReportPath);
                sparkReporter.Config.Theme = Theme.Dark;
                sparkReporter.Config.DocumentTitle = "OrangeHRM Test Report";
                sparkReporter.Config.ReportName = "Automation Execution Results";
                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);
                // Add System Info
                extent.AddSystemInfo("Environment", "QA");
                extent.AddSystemInfo("Tester", "Prabha");
                extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            }
            return extent;
        }
    }
}

