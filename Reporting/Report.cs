namespace Reporting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using HandlebarsDotNet;
    using Newtonsoft.Json.Linq;
    using System.Linq;

    public static class Report
    {
        public static void Build()
        {
            // Parse log of tests
            var lines = new List<JObject>();
            using (var fs = new FileStream("./log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(JObject.Parse(line));
                }
            }
            var testResults = lines.GroupBy(l => l["Properties"]["TestName"]);


            // Render report
            var indexTemplateRaw = File.ReadAllText("./templates/index.hbs");
            var indexTemplate = Handlebars.Compile(indexTemplateRaw);

            CompilePartial("test_result_row");
            CompilePartial("foundation_style");
            CompilePartial("report_style");
            CompilePartial("jquery_javascript");
            CompilePartial("foundation_javascript");

            var testResultsData = new List<dynamic>();

            foreach (var testResult in testResults)
            {
                var outcome = testResult.Single(d => d["Properties"]["LogType"]?.Value<string>() == "TestResult");

                var tempDetails = new List<dynamic>();

                foreach (var detail in testResult.Where(d => d["Properties"]["LogType"]?.Value<string>() == "ElementAction" ||
                                                             d["Properties"]["LogType"]?.Value<string>() == "Screenshot" ||
                                                             d["Properties"]["LogType"]?.Value<string>() == "TestStep"))
                {
                    switch (detail["Properties"]["LogType"]?.Value<string>())
                    {
                        case "ElementAction":
                            tempDetails.Add(new
                            {
                                is_action = true,
                                element = detail["Properties"]["Name"].Value<string>(),
                                action = detail["Properties"]["Action"].Value<string>(),
                            });
                            break;
                        case "TestStep":
                            tempDetails.Add(new
                            {
                                is_step = true,
                                step = detail["Properties"]["Step"].Value<string>(),
                                text = detail["Properties"]["Text"].Value<string>(),
                            });
                            break;
                        default:
                            continue;
                    }
                }

                var tempScreenshots = new List<dynamic>();

                foreach (var detail in testResult.Where(d => d["Properties"]["LogType"]?.Value<string>() == "Screenshot"))
                {
                    tempScreenshots.Add(new
                    {
                        base64 = detail["Properties"]["Base64"].Value<string>(),
                        screenshot_id = Guid.NewGuid().ToString("D")
                    });
                }

                var temp = new
                {
                    test_report_id = Guid.NewGuid().ToString("D"),
                    test_name = testResult.Key.Value<string>(),
                    status = outcome["Properties"]["TestResult"].Value<string>(),
                    status_color = (outcome["Properties"]["TestResult"].Value<string>() == "Passed") ? "green" : "red",
                    details = tempDetails.ToArray(),
                    screenshots = tempScreenshots.ToArray()
                };

                testResultsData.Add(temp);
            }

            var data = new
            {
                application_title = Resource.TitleApp,
                test_results = testResultsData.ToArray()
            };

            var resultRaw = indexTemplate(data);
            using (var sw = new StreamWriter("./report.html"))
            {
                sw.WriteLine(resultRaw);
            }
        }

        /// <summary>
        /// Compile template file and include it in result
        /// 
        /// Template file should be in "./templates" directory and be named same as template variable
        /// starting with underscore
        /// </summary>
        /// <param name="partialName"></param>
        private static void CompilePartial(string partialName)
        {
            var templateRaw = File.ReadAllText($"./templates/_{partialName}.hbs");
            using (var reader = new StringReader(templateRaw))
            {
                var template = Handlebars.Compile(reader);
                Handlebars.RegisterTemplate(partialName, template);
            }
        }
    }
}