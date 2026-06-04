using System;
using System.IO;

namespace HubbelReportGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string csvFilePath = Path.Combine(baseDir, "../../../test_input/test1.csv");
            string templateFilePath = Path.Combine(baseDir, "../../../Line_Test_Report_Template.docx");
            string outputDirectory = Path.Combine(baseDir, "../../../test_output");
            Console.WriteLine(csvFilePath);
            Console.WriteLine(templateFilePath);
            Console.WriteLine(outputDirectory);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
                Console.WriteLine($"Created output directory at: {outputDirectory}");
            }

            var coverData = new CoversheetData
            {
                ProjectNumber = "PRJ-12345",
                FileNumber = "FN-9876",
                SampleCatNumber = "CAT-001",
                DutType = "GFCI Receptacle",
                DutRating = "15A 125V",
                DutResetType = "Auto",
                TestProgram = "Standard Compliance"
            };

            Console.WriteLine("Starting report generation...");

            try
            {
                var generator = new ReportGenerator(templateFilePath, outputDirectory);

                string generatedFilePath = generator.GenerateReport(csvFilePath, coverData);

                Console.WriteLine("\nSuccess! Report generated.");
                Console.WriteLine($"Saved to: {generatedFilePath}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\nFile Error: {ex.Message}");
                Console.WriteLine("Please ensure your CSV and Word template are in the correct directories.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}