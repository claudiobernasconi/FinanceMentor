using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace FinanceMentor.Test
{
    [Binding]
    public sealed class StepDefinitions
    {
        private IWebDriver Driver { get; set; }
        private const string url = "https://localhost:44358";

        [BeforeScenario]
        public void BeforeScenario()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            Driver = new ChromeDriver(Environment.CurrentDirectory, chromeOptions);
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver.Quit();
        }

        [Then(@"the home page is loaded")]
        public void ThenTheHomePageIsLoaded()
        {
            var NavMenu = Driver.FindElement(By.Id("navmenu"));

            Assert.IsNotNull(NavMenu);
        }

        [Then(@"the navigation contains (.*) items")]
        public void ThenTheNavigationContainsItems(int itemCount)
        {
            var NavMenu = Driver.FindElement(By.Id("navmenu"));
            var NavMenuItems = NavMenu.FindElements(By.XPath("//li[contains(@class,'nav-item')]"));

            Assert.AreEqual(itemCount, NavMenuItems.Count);
        }

        [Given(@"The user is on the (.*) overview")]
        public void TheUserIsOnTheOverview(string route)
        {
            Driver.Navigate().GoToUrl($"{url}/{route}s");
            Thread.Sleep(500);
        }

        [Then(@"the page title is (.*)")]
        public void ThenThePageTitleIsEarnings(string pageTitle)
        {
            var CardHeader = Driver.FindElement(By.XPath("//div[@class='card-header']"));

            Assert.AreEqual(pageTitle, CardHeader.Text.Trim());
        }

        [When(@"The user adds a new earning")]
        public void WhenAddAnEarning()
        {
            var earningsContainer = Driver.FindElement(By.Id("earnings-container"));
            var earningsForm = earningsContainer.FindElement(By.TagName("form"));
            var subjectInput = earningsForm.FindElement(By.Id("subjectInput"));
            var categoryInput = earningsForm.FindElement(By.Id("categoryInput"));
            var amountInput = earningsForm.FindElement(By.Id("amountInput"));
            var submitEarning = earningsForm.FindElement(By.Id("submitEarning"));

            subjectInput.SendKeys("Painting Work: 2 Rooms");
            categoryInput.SendKeys("Freelancing");
            amountInput.SendKeys("355");

            submitEarning.Click();
        }

        [Then(@"the new earning should be in the table")]
        public void ThenTheNewEarningShouldBeInTheTable()
        {
            var earningsTable = Driver.FindElement(By.Id("earnings-table"));
            var tableRows = earningsTable.FindElements(By.TagName("tr"));
            var containsPaintingWork = tableRows.Any(row => row.Text.Contains("Painting Work: 2 Rooms"));

            Assert.IsTrue(containsPaintingWork);
        }
    }
}
