using System.Collections.Generic;
using System.Linq;
using IO;
using OpenQA.Selenium.Chrome;

namespace DoctorScrape
{
    internal class ListPractitionersDetails
    {
        public static void Record(string csvInputFilePath, string csvOutputFilePath)
        {
            // Read the search result data from csv.
            var reader = new CsvManager(csvInputFilePath);
            var practitionersCsv = reader.Read();
            var practitioners = practitionersCsv.Select(Converter.Convert);

            var detailsCsv = new List<PractitionerDetailsCsv>();

            // Requires Chrome.exe v115. You MUST upgrade chrome to this version.
            using (var driver = new ChromeDriver())
            {
                // Navigate to each practitioner's details page and get details.
                var detailsScraper = new DetailsScraper(driver);
                foreach (var practitioner in practitioners)
                {
                    detailsScraper.NavigateToPage(practitioner);

                    var basics = detailsScraper.GetBasics();
                    var summary = detailsScraper.GetSummary();
                    var practiceLocation = detailsScraper.GetPrimaryPractice();
                    var specialities = detailsScraper.GetSpecialities();

                    // Convert from models to csv model.
                    var detailCsv = Converter.Convert(basics, summary, practiceLocation, specialities);
                    detailsCsv.Add(detailCsv);
                }
            }

            // Write the details data to CSV.
            var writer = new CsvManager(csvOutputFilePath);
            writer.Write(detailsCsv);
        }
    }
}
