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
        private readonly string _coverTemplateFilePath;
        private readonly string _outputDirectory;
        private readonly CoversheetData _coversheetData;

        public ReportGenerator(string outputDirectory, CoversheetData coversheetData, string lineTemplateFilePath, string coverTemplateFilePath)

        {
            _lineTemplateFilePath = lineTemplateFilePath;
            _coverTemplateFilePath = coverTemplateFilePath;
            _outputDirectory = outputDirectory;
            _coversheetData = coversheetData ?? throw new ArgumentNullException(nameof(coversheetData));
            if (string.IsNullOrWhiteSpace(_coversheetData.CsvFilePath))
            {
                throw new ArgumentException("Data file path (csv) must be specified in provided coversheet data");
            }
        }

        // generates the complete Word document.
        public string GenerateReport()
        {
            var testResults = ParseCsvData(_coversheetData.CsvFilePath);
            string outputFileName = Path.Combine(_outputDirectory, $"GFCI_Report_{DateTime.Now:yyyyMMdd_HHmmss}.docx");
            CompileCoversheet(outputFileName, _coversheetData);
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

        private void CompileCoversheet(string destinationPath, CoversheetData coversheetData)
        {
            using DocX coverPage = DocX.Load(_coverTemplateFilePath);

            MapField(coverPage, "{{ProjectNumber}}", coversheetData.ProjectNumber ?? "");
            MapField(coverPage, "{{FileNumber}}", coversheetData.FileNumber ?? "");
            MapField(coverPage, "{{TrackingNumber}}", coversheetData.TrackingNumber ?? "");
            MapField(coverPage, "{{SampleCatNumber}}", coversheetData.SampleCatNumber ?? "");
            MapField(coverPage, "{{Client}}", coversheetData.Client ?? "");
            MapField(coverPage, "{{Technician}}", coversheetData.Technician ?? "");
            MapField(coverPage, "{{DutType}}", coversheetData.DutType ?? "");
            MapField(coverPage, "{{DutRating}}", coversheetData.DutRating ?? "");
            MapField(coverPage, "{{DutResetType}}", coversheetData.DutResetType ?? "");
            MapField(coverPage, "{{TestProgram}}", coversheetData.TestProgram ?? "");

            coverPage.SaveAs(destinationPath);
        }
        private void CompileDocument(string destinationPath, List<TestRunRecord> results)
        {
            using DocX reportDoc = DocX.Load(destinationPath);
            // Loop through every test record
            for (int i = 0; i < results.Count; i++)
            {
                var record = results[i];
                CompileSingleLinePage(reportDoc, record);
            }
            reportDoc.Save();
        }

        private void CompileSingleLinePage(DocX reportDoc, TestRunRecord record)
        {
            // Load the single-page template
            using DocX linePage = DocX.Load(_lineTemplateFilePath);

            // Map Header Fields
            MapField(linePage, "{{Line}}", record.Line ?? "");
            MapField(linePage, "{{TestMode}}", record.TestMode ?? "");
            MapField(linePage, "{{Temp}}", record.Temp ?? "");
            MapField(linePage, "{{Fault}}", record.Fault ?? "");
            MapField(linePage, "{{SwitchMode}}", record.SwitchMode ?? "");
            MapField(linePage, "{{Parameter}}", record.Parameter ?? "");
            MapField(linePage, "{{LineVoltage}}", record.LineVoltage ?? "");

            // Map Test Conditions
            //linePage.ReplaceText("{{TestConditionsAt}}", record.TestConditionsAt ?? "");
            MapField(linePage, "{{Voltage}}", record.Voltage ?? "");
            MapField(linePage, "{{LoadCurrent}}", record.LoadCurrent ?? "");
            MapField(linePage, "{{Polarity}}", record.Polarity ?? "");
            MapField(linePage, "{{Position}}", record.Position ?? "");
            MapField(linePage, "{{MeteredRbCurrent}}", record.MeteredRbCurrent ?? "");

            // Map Shots
            MapField(linePage, "{{Shot1}}", record.Shot1.ToString("F5"));
            MapField(linePage, "{{Shot2}}", record.Shot2.ToString("F5"));
            MapField(linePage, "{{Shot3}}", record.Shot3.ToString("F5"));
            MapField(linePage, "{{Shot4}}", record.Shot4.ToString("F5"));
            MapField(linePage, "{{Shot5}}", record.Shot5.ToString("F5"));
            MapField(linePage, "{{Shot6}}", record.Shot6.ToString("F5"));
            MapField(linePage, "{{Shot7}}", record.Shot7.ToString("F5"));
            MapField(linePage, "{{Shot8}}", record.Shot8.ToString("F5"));
            MapField(linePage, "{{Shot9}}", record.Shot9.ToString("F5"));
            MapField(linePage, "{{Shot10}}", record.Shot10.ToString("F5"));

            // Map Results
            MapField(linePage, "{{AvgTripTime}}", record.AvgTripTime ?? "");

            reportDoc.InsertSectionPageBreak();
            reportDoc.InsertDocument(linePage);
        }

        // helper function for doing the find/replace when generating report pages
        private static void MapField(DocX document, string tag, string value)
        {
            var options = new StringReplaceTextOptions()
            {
                SearchValue = tag,
                NewValue = value ?? string.Empty
            };
            document.ReplaceText(options);
        }
    }
}
