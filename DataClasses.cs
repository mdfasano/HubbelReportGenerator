using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

namespace HubbelReportGenerator
{
    // Represents the Coversheet template data
    // Needs to contain a link to a valid csv file
    // Can contain info for each field, which will be used to populate the coversheet of the report
    // *** if a string is not provided for a field, the report will currently generate with "N/A" displayed ***
    public class CoversheetData
    {
        [Name("CsvFilePath")] public string? CsvFilePath { get; set; }
        [Name("Project #")] public string? ProjectNumber { get; set; }
        [Name("File #")] public string? FileNumber { get; set; }
        [Name("Tracking #")] public string? TrackingNumber { get; set; }
        [Name("Sample/CAT #")] public string? SampleCatNumber { get; set; }
        [Name("Client Name")] public string? Client { get; set; }
        [Name("Technician Name")] public string? Technician { get; set; }
        [Name("DUT Type")] public string? DutType { get; set; }
        [Name("DUT Rating")] public string? DutRating { get; set; }
        [Name("DUT Reset Type")] public string? DutResetType { get; set; }
        [Name("Test Program")] public string? TestProgram { get; set; }
    }

    // Represents a single row from your CSV / Test Run
    public class TestRunRecord
    {
        // Template Header Fields
        [Name("Line #")] public string? Line { get; set; }
        [Name("Test Mode")] public string? TestMode { get; set; }
        [Name("Temperature")] public string? Temp { get; set; }
        [Name("Fault")] public string? Fault { get; set; }
        [Name("Switch Mode")] public string? SwitchMode { get; set; }
        [Name("Additional Parameter")] public string? Parameter { get; set; }
        [Name("Voltage")] public string? LineVoltage { get; set; }

        // Test Conditions Fields
        //[Name("Time")] public string? TestConditionsAt { get; set; }
        [Name("Test V")] public string? Voltage { get; set; }
        [Name("Load")] public string? LoadCurrent { get; set; }
        [Name("Polarity")] public string? Polarity { get; set; }
        [Name("Position")] public string? Position { get; set; }
        [Name("Test mA")] public string? MeteredRbCurrent { get; set; }

        // Individual Shot Data
        [Name("Shot 1")] public double Shot1 { get; set; }
        [Name("Shot 2")] public double Shot2 { get; set; }
        [Name("Shot 3")] public double Shot3 { get; set; }
        [Name("Shot 4")] public double Shot4 { get; set; }
        [Name("Shot 5")] public double Shot5 { get; set; }
        [Name("Shot 6")] public double Shot6 { get; set; }
        [Name("Shot 7")] public double Shot7 { get; set; }
        [Name("Shot 8")] public double Shot8 { get; set; }
        [Name("Shot 9")] public double Shot9 { get; set; }
        [Name("Shot 10")] public double Shot10 { get; set; }

        // Result Fields
        [Name("Avg.")] public string? AvgTripTime { get; set; }

        // list of shots for easier doc generation
        public List<double> GetShotsAsList()
        {
            return [
                Shot1, Shot2, Shot3, Shot4, Shot5,
                Shot6, Shot7, Shot8, Shot9, Shot10
            ];
        }
    }
}