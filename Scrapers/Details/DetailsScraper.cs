using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using static DoctorScrape.Speciality;
using static DoctorScrape.Summary;

namespace DoctorScrape
{
    internal interface IDetailsScraper
    {
        void NavigateToPage(Practitioner practitioner);
        Basics GetBasics();
        Summary GetSummary();
        PracticeLocation GetPrimaryPractice();
        Specialities GetSpecialities();
    }

    internal class DetailsScraper : IDetailsScraper
    {
        private readonly IWebDriver _driver;
        public DetailsScraper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPage(Practitioner practitioner)
        {
            _driver.Url = practitioner.DetailsUrl;
        }

        public Basics GetBasics()
        {
            var heading = _driver.FindElement(By.CssSelector("div.name_cpso_num"));
            var nameHeading = heading.FindElement(By.Id("docTitle"));
            var cpsoNumberHeading = heading.FindElement(By.CssSelector("h3"));

            var name = nameHeading.Text.Trim();
            var cpsoNumber = cpsoNumberHeading.Text.Replace("CPSO#:", "").Trim();

            return new Basics(name, cpsoNumber);
        }

        public Summary GetSummary()
        {
            var infoDivs = _driver.FindElement(By.CssSelector("div.info"));
            var paragraphs = infoDivs.FindElements(By.CssSelector("p"));

            var summaryBuilder = new SummaryBuilder();
            foreach (var paragraph in paragraphs)
            {
                var text = paragraph.Text;
                if (text.StartsWith("Former Name:")) { summaryBuilder.FormerNames(text.Replace("Former Name:", "").Trim()); }
                else if (text.StartsWith("Gender:")) { summaryBuilder.Gender(text.Replace("Gender:", "").Trim()); }
                else if (text.StartsWith("Languages Spoken:")) { summaryBuilder.LanguagesSpoken(text.Replace("Languages Spoken:", "").Trim()); }
                else if (text.StartsWith("Education:")) { summaryBuilder.Education(text.Replace("Education:", "").Trim()); }
            }

            return summaryBuilder.Build();
        }

        public PracticeLocation GetPrimaryPractice()
        {
            var practiceDiv = _driver.FindElement(By.CssSelector("div.practice-location"));
            var detailsDiv = practiceDiv.FindElements(By.CssSelector("div.location_details"));

            // Has no HTML structure, e.g. "Suite 300/306\r\n5500 North Service Road\r\nBurlington ON  L7L 6W6\r\nPhone: 905 315 3074\r\nFax: 289 714 2021 Electoral District: 04"
            // A different method can parse and split it.
            return new PracticeLocation(detailsDiv.First().Text);
        }

        public Specialities GetSpecialities()
        {
            var specialitiesSection = _driver.FindElement(By.CssSelector("section#specialties"));
            var table = specialitiesSection.FindElement(By.CssSelector("table.stack"));

            var specialities = new List<Speciality>();
            var tableRow = table.FindElements(By.TagName("tr"));
            foreach (var row in tableRow)
            {
                var specialityBuilder = new SpecialityBuilder();
                var tableData = row.FindElements(By.TagName("td")); // Skip the table header.
                if (tableData.Count == 3) // Only seen table with 3 columns.
                {
                    specialityBuilder.SpecialtyName(tableData[0].Text);
                    specialityBuilder.IssuedOn(tableData[1].Text);
                    specialityBuilder.Type(tableData[2].Text);
                    specialities.Add(specialityBuilder.Build());
                }
            }

            return new Specialities(specialities);
        }
    }
}
