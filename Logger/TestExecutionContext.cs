namespace Logger
{
    using System;

    public class TestExecutionContext
    {
        public static Type TestType
        {
            private get
            {
                return (Type)Context.Test.Read("TestType");
            }

            set
            {
                Context.Test.Write("TestType", value);
            }
        }

        public static string TestName
        {
            get
            {
                return (string)Context.Test.Read("TestName");
            }

            set
            {
                Context.Test.Write("TestName", value);
            }
        }

        public static TestResult TestResult
        {
            get
            {
                return (TestResult)Context.Test.Read("TestResult");
            }

            set
            {
                Context.Test.Write("TestResult", value);
            }
        }
    }
}