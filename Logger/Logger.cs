namespace Logger
{
    using System.IO;
    using Serilog;
    using Serilog.Formatting.Json;
    using WDriver;

    public static class Logger
    {
        private static bool configured;

        public static string MessageAboutError { get; set; }

        public static void Configure()
        {
            if (configured)
            {
                return;
            }

            if (!Directory.Exists(Config.FolderToLog))
            {
                Directory.CreateDirectory(Config.FolderToLog);
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .Enrich.With(new TestNameEnricher())
                .WriteTo.LiterateConsole()
                .WriteTo.File(new JsonFormatter(), Config.FileToLog)
                .CreateLogger();
            configured = true;
        }

        public static void WriteToLogAboutScreenshot(string screenshot)
        {
            var base64 = WDriver.ScreenShot.AsBase64EncodedString;
            Log.ForContext("LogType", "Screenshot")
                .ForContext("Base64", base64)
                .Warning($"{{Name}} :: {{Action}}", "Screenshot", string.Format("Screenshot taken in file {0}", screenshot));
        }
    }
}