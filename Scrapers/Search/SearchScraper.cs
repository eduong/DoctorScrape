using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace DoctorScrape
{
    internal interface ISearchScraper
    {
        void NavigateToPage();

        void SelectCity(string city);

        void Submit();
    }

    internal class SearchScraper : ISearchScraper
    {
        private readonly IWebDriver _driver;
        public SearchScraper(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPage()
        {
            // Navigate to advance doctor search page.
            _driver.Url = "https://doctors.cpso.on.ca/?search=general";
        }

        public void SelectCity(string city)
        {
            // Select city and click submit.
            SelectDropdown("select.cpso-city", city);
        }

        public void Submit()
        {
            Click("input.submit");
        }

        private void SelectDropdown(string selector, string choiceText)
        {
            var dropDown = new SelectElement(_driver.FindElement(By.CssSelector(selector)));
            dropDown.SelectByText(choiceText);
        }

        private void Click(string selector)
        {
            var inputs = _driver.FindElements(By.CssSelector(selector));

            // https://stackoverflow.com/questions/19669786/check-if-element-is-visible-in-dom
            var visibleInputs = inputs.Where(i => i.GetDomProperty("offsetParent") != null);
            foreach (var visibleInput in visibleInputs)
            {
                visibleInput.Click();
            }
        }
    }
}
