# Hubbel Report Generator

A C# console application designed to automate the generation of test reports. 
It reads test run data from a CSV file and populates a preformatted .docx template.

## Features
- Automatically reads and parses test data using `CsvHelper`.
- Maps data to placeholders in a single-page Word template and compiles them into a single report using `DocX`.
- Generates a comprehensive report containing multiple test records separated by page breaks.

## Prerequisites
- .NET SDK
- [CsvHelper](https://joshclose.github.io/CsvHelper/)
- [DocX (Xceed.Words.NET)](https://github.com/xceedsoftware/DocX)

## Usage
1. Ensure your template file (`Line_Test_Report_Template.docx`) and your data file (`TestData.csv`) are located in the application's base directory
3. The generated reports will be saved with a timestamp in the `test_output` directory.