using CsvHelper; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace HubbelReportGenerator
{
    public class ReportGenerator
    {
        private readonly string _lineTemplateFilePath;
        private readonly string _outputDirectory;

        public ReportGenerator(string lineTemplateFilePath, string outputDirectory)
        {
            _lineTemplateFilePath = lineTemplateFilePath;
            _outputDirectory = outputDirectory;
        }

        // generates the complete Word document.
        public string GenerateReport(string csvFilePath, CoversheetData coverData)
        {
            var testResults = ParseCsvData(csvFilePath);
            string outputFileName = Path.Combine(_outputDirectory, $"GFCI_Report_{DateTime.Now:yyyyMMdd_HHmmss}.docx");
            CompileDocument(outputFileName, testResults);

            return outputFileName;
        }

        private List<TestRunRecord> ParseCsvData(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException("The specified CSV file was not found.", csvFilePath);

            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<TestRunRecord>().ToList();
            return records;
        }

        private void CompileDocument(string destinationPath, List<TestRunRecord> results)
        {
            using (DocX reportDoc = DocX.Create(destinationPath))
            {
                // 2. Loop through every test record
                for (int i = 0; i < results.Count; i++)
                {
                    var record = results[i];

                    // 3. Load the single-page template
                    using DocX linePage = DocX.Load(_lineTemplateFilePath);

                    // Map Header Fields
                    linePage.ReplaceText("{{Line}}", record.Line ?? "");
                    linePage.ReplaceText("{{TestMode}}", record.TestMode ?? "");
                    linePage.ReplaceText("{{Temp}}", record.Temp ?? "");
                    linePage.ReplaceText("{{Fault}}", record.Fault ?? "");
                    linePage.ReplaceText("{{SwitchMode}}", record.SwitchMode ?? "");
                    linePage.ReplaceText("{{Parameter}}", record.Parameter ?? "");
                    linePage.ReplaceText("{{LineVoltage}}", record.LineVoltage ?? "");

                    // Map Test Conditions
                    //linePage.ReplaceText("{{TestConditionsAt}}", record.TestConditionsAt ?? "");
                    linePage.ReplaceText("{{Voltage}}", record.Voltage ?? "");
                    linePage.ReplaceText("{{LoadCurrent}}", record.LoadCurrent ?? "");
                    linePage.ReplaceText("{{Polarity}}", record.Polarity ?? "");
                    linePage.ReplaceText("{{Position}}", record.Position ?? "");
                    linePage.ReplaceText("{{MeteredRbCurrent}}", record.MeteredRbCurrent ?? "");

                    // Map Shots
                    linePage.ReplaceText("{{Shot1}}", record.Shot1.ToString("F5"));
                    linePage.ReplaceText("{{Shot2}}", record.Shot2.ToString("F5"));
                    linePage.ReplaceText("{{Shot3}}", record.Shot3.ToString("F5"));
                    linePage.ReplaceText("{{Shot4}}", record.Shot4.ToString("F5"));
                    linePage.ReplaceText("{{Shot5}}", record.Shot5.ToString("F5"));
                    linePage.ReplaceText("{{Shot6}}", record.Shot6.ToString("F5"));
                    linePage.ReplaceText("{{Shot7}}", record.Shot7.ToString("F5"));
                    linePage.ReplaceText("{{Shot8}}", record.Shot8.ToString("F5"));
                    linePage.ReplaceText("{{Shot9}}", record.Shot9.ToString("F5"));
                    linePage.ReplaceText("{{Shot10}}", record.Shot10.ToString("F5"));

                    // Map Results
                    linePage.ReplaceText("{{AvgTripTime}}", record.AvgTripTime ?? "");

                    // Append to the report document
                    reportDoc.InsertDocument(linePage);

                    // Add a page break if this isn't the last record
                    if (i < results.Count - 1)
                    {
                        reportDoc.InsertSectionPageBreak();
                    }
                }

                // 6. Save the final document
                reportDoc.Save();
            }
        }
    }
}
