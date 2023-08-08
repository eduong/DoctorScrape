using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace DoctorScrape
{
    internal interface ISearchResultsScraper
    {
        IEnumerable<Practitioner> GetPractitioners();
    }

    internal class SearchResultsScraper : ISearchResultsScraper
    {
        private const string DoctorDetailsUrl = "https://doctors.cpso.on.ca/DoctorDetails/";

        private IWebDriver _driver;

        public SearchResultsScraper(IWebDriver driver)
        {
            _driver = driver;
        }

        public IEnumerable<Practitioner> GetPractitioners()
        {
            for(;;)
            {
                var practitioners = GetPractitionersOnPage();
                foreach (var practitioner in practitioners)
                {
                    yield return practitioner;
                }

                var pageNumbers = GetPageNumbers();
                if (pageNumbers.LastPage()) {
                    break;
                }

                var currentPagePlusOne = GetPageAnchor(pageNumbers.Current + 1);
                if (currentPagePlusOne != null) {
                    currentPagePlusOne.Click();
                    continue;
                }

                var nextFive = GetNext5Anchor();
                nextFive?.Click();
            }
        }

        private class PageNumbers {
            public PageNumbers(int current, int last) {
                Current = current;
                Last = last;
            }
            public int Current { get; }
            public int Last { get; }
            public bool LastPage() => Current >= Last;
        }

        private PageNumbers GetPageNumbers()
        {
            // Has the form: "Page 3 of 1000"
            var text = _driver.FindElement(By.XPath("//*[contains(@class, 'doctor-search-count')]/div[2]/p")).Text;
            var rgx = new Regex(@"Page (?<CurrentPage>\d+) of (?<LastPage>\d+)");
            var matches = rgx.Matches(text);

            // Report on each match.
            var match = matches.First();
            return new PageNumbers(
                int.Parse(match.Groups["CurrentPage"].Value),
                int.Parse(match.Groups["LastPage"].Value));
        }

        private IEnumerable<Practitioner> GetPractitionersOnPage()
        {
            // Find the doctor detail links.
            var resultsDiv = _driver.FindElement(By.CssSelector("div.doctor-search-results"));
            var anchors = resultsDiv.FindElements(By.TagName("a"));
            var detailAnchors = SelectDetailAnchors(anchors);

            foreach (var detailAnchor in detailAnchors)
            {
                var detailUrl = GetUrl(detailAnchor);
                yield return new Practitioner(detailUrl);
            }
        }

        private IWebElement GetPageAnchor(int pageNumber) {
            var pagingDiv = _driver.FindElement(By.CssSelector("div.doctor-search-paging"));
            var anchors = pagingDiv.FindElements(By.TagName("a"));
            return anchors.SingleOrDefault(a => ParsePageNumber(a) == pageNumber);
        }

        private IWebElement GetNext5Anchor() {
            var pagingDiv = _driver.FindElement(By.CssSelector("div.doctor-search-paging"));
            var anchors = pagingDiv.FindElements(By.TagName("a"));
            return anchors.FirstOrDefault(a => "Next 5".Equals(a.Text));
        }

        private static int ParsePageNumber(IWebElement anchor)
        {
            return int.TryParse(anchor.Text, out var pageNumber) ? pageNumber : -1;
        }

        private static IEnumerable<IWebElement> SelectDetailAnchors(ReadOnlyCollection<IWebElement> anchors)
        {
            return anchors.Where(UrlIsDetails);
        }

        private static bool UrlIsDetails(IWebElement anchor)
        {
            var url = GetUrl(anchor);
            return url != null && url.StartsWith(DoctorDetailsUrl);
        }

        private static string GetUrl(IWebElement anchor)
        {
            return anchor.GetAttribute("href");
        }
    }
}
