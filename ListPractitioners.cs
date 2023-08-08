using System;
using System.Collections.Generic;
using System.Linq;
using IO;
using OpenQA.Selenium.Chrome;

namespace DoctorScrape
{
    internal class ListPractitioners
    {
        public static void Record(string city, string csvOutputFilePath)
        {
            var practitioners = new List<Practitioner>();

            // Requires Chrome.exe v115. You MUST upgrade chrome to this version.
            using (var driver = new ChromeDriver())
            {
                // Navigate to adv search, pick a city and submit.
                var searchScraper = new SearchScraper(driver);
                searchScraper.NavigateToPage();
                searchScraper.SelectCity(city);
                searchScraper.Submit();

                // Navigate the 1...n search result pages, collecting search result data.
                var searchResultsScraper = new SearchResultsScraper(driver);
                practitioners.AddRange(searchResultsScraper.GetPractitioners());
            }

            // Write the search result data to CSV.
            var writer = new CsvManager(csvOutputFilePath);
            var practitionersCsv = practitioners.Select(Converter.Convert);
            writer.Write(practitionersCsv);
        }
    }
}
