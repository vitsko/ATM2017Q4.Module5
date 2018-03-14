namespace Tests
{
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using Reporting;
    using Serilog;
    using Serilog.Formatting.Json;
    using Tiver.Fowl.Core.Context;
    using Tiver.Fowl.Core.Enums;
    using Tiver.Fowl.Logging;
    using Tiver.Fowl.TestingBase;
    using Tiver.Fowl.WebDriverExtended.Browsers;

    [TestFixture]
    public class BaseTestForNUnit : IBaseTest
    {
        [SetUp]
        public void Setup()
        {
            //Flow.Setup(GetType(), TestContext.CurrentContext.Test.Name);


            BaseTestForNUnit.Configure();

            TestExecutionContext.TestType = GetType();
            TestExecutionContext.TestName = TestContext.CurrentContext.Test.Name;

            if (TestExecutionContext.IsWebDriverTest)
            {
                TestExecutionContext.Browser = BrowserFactory.GetBrowser();
                TestExecutionContext.BrowserActions.NavigateToStartUri();
            }

        }

        private static void Configure()
        {
            if (configured)
            {
                return;
            }

            Log.Logger = new LoggerConfiguration()
                .Enrich.With(new TestNameEnricher())
                .WriteTo.LiterateConsole()
                .WriteTo.File(new JsonFormatter(), "./log.txt")
                .CreateLogger();
            configured = true;
        }

        private static bool configured;

        [TearDown]
        public void Teardown()
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

            Flow.Teardown(testResult);
        }

        public int? Step
        {
            get;
            set;
        }
    }

    [SetUpFixture]
    public class SetupFixtureForNUnit
    {
        [OneTimeTearDown]
        public static void Cleanup()
        {
            Report.Build();
        }
    }
}