namespace Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Logger;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Reporting;
    using Serilog;
    using WDriver;

    public class BaseTestForNUnit
    {
        [SetUp]
        public void Setup()
        {
            TestExecutionContext.TestType = GetType();
            TestExecutionContext.TestName = TestContext.CurrentContext.Test.Name;
        }

        [TearDown]
        public void GetResultTest()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            TestResult testResult;
            switch (testStatus)
            {
                case TestStatus.Passed:
                    testResult = TestResult.Passed;
                    break;
                case TestStatus.Failed:
                    testResult = TestResult.Failed;
                    break;
                case TestStatus.Inconclusive:
                case TestStatus.Skipped:
                case TestStatus.Warning:
                default:
                    testResult = TestResult.Unknown;
                    break;
            }

            BaseTestForNUnit.Teardown(testResult);
        }

        private static void Teardown(TestResult testResult)
        {
            TestExecutionContext.TestResult = testResult;
            Teardown();
        }

        private static void Teardown()
        {
            Log.ForContext("LogType", "TestResult").Information("Test result - '{TestResult}'", TestExecutionContext.TestResult);

            if (TestExecutionContext.TestResult == TestResult.Failed)
            {
                Log.Error(string.Format(Resource.ResultError, Logger.MessageAboutError));
                var screenshot = WDriver.TakeScreenshot(Path.Combine(Environment.CurrentDirectory, Config.FolderToScreenshot), TestExecutionContext.TestName);
                Logger.WriteToLogAboutScreenshot(screenshot);
            }

            WDriver.Quit();
        }

        [SetUpFixture]
        public class SetupFixtureForNUnit
        {
            [OneTimeTearDown]
            public static void Cleanup()
            {
                Report.Build();
            }

            [OneTimeSetUp]
            public static void SetUpBeforeTest()
            {
                Logger.Configure();

                Debug.WriteLine(Resource.StartLog);
            }
        }
    }
}