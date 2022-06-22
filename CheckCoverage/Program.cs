using System.Globalization;
using System.Xml;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Checking the code coverage...");
        
        var (coverageFile, coverageThreshold) = ValidateAndGetParameters(args);
        decimal currentCoverage = ExtractCurrentCoverageFromFile(coverageFile);
        
        ValidateCoverage(coverageThreshold, currentCoverage);
        
        Console.WriteLine($"Coverage ok -> current: {currentCoverage} > threshold: {coverageThreshold}");
    }

    private static void ValidateCoverage(decimal coverageThreshold, decimal currentCoverage)
    {
        if (currentCoverage < coverageThreshold)
        {
            var message = $"Not enougth coverage current: {currentCoverage} < threshoold: {coverageThreshold}";
            Console.WriteLine(message);
            throw new Exception(message);
        }
    }

    private static decimal ExtractCurrentCoverageFromFile(string coverageFile)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(coverageFile);
        var currentCoverage = Decimal.Parse(
            doc.ChildNodes![2]!.Attributes!["line-rate"]!.Value,
            new CultureInfo("en-US")
        );
        return currentCoverage;
    }

    private static (string, decimal) ValidateAndGetParameters(string[] args)
    {
        if (args.Count() != 2)
            throw new Exception("Please provide the paramethers: file path and coverage threshold");
        var coverageFile = args[0];
        var coverageThreshold = Decimal.Parse(args[1], new CultureInfo("en-US"));
        return (coverageFile, coverageThreshold);
    }
}