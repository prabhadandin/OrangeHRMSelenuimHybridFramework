using System;
using System.IO;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace OrangeHRMHybridAutomationFramework.Utilities
{
    public class ExtentManager
    {
        
        private static ExtentReports extent;
        private static ExtentSparkReporter sparkReporter;

        public static ExtentReports GetInstance()
        {
            if (extent == null)
            {
                //  Get the path to the Project Root (Goes up from bin/Debug/net4x to Project Folder)
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = Path.GetFullPath(Path.Combine(basePath, "..", ".."));
                string reportDirectory = Path.Combine(projectPath, "Reports");
                // Create directory if it doesn't exist
                if (!Directory.Exists(reportDirectory))
                    Directory.CreateDirectory(reportDirectory);
                string finalReportPath = Path.Combine(reportDirectory, "ExtentReport.html");
                //  Initialize Reporter
                sparkReporter = new ExtentSparkReporter(finalReportPath);
                sparkReporter.Config.Theme = Theme.Dark;
                sparkReporter.Config.DocumentTitle = "OrangeHRM Test Report";
                sparkReporter.Config.ReportName = "Automation Execution Results";
                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);

                // Add System Info
                extent.AddSystemInfo("Environment", "QA");
                extent.AddSystemInfo("Tester", "Prabha");
               }
            return extent;
        }
    }
}

